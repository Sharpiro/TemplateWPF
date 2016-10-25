using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using Ninject;
using Ninject.Parameters;
using TemplateWpf.Models;
using TemplateWPF.Tools;

namespace TemplateWPF.Windows
{
    public partial class MainWindow : Window
    {
        private const string switchRoleConfirmation = "Switching roles will reset the current changes to the role.  Do you wish to proceed?";
        private const string revokeRoleConfirmation = "Are you sure you want to revoke and remove this role from the user?";

        private readonly MessageBoxFacade _messageBoxFacade;
        private readonly IKernel _kernel;
        private readonly MainViewModel _viewModel;
        private readonly DepartmentMoverFactory _departmentMoverFactory;

        public MainWindow(MessageBoxFacade messageBoxFacade, IKernel kernel,
            MainViewModel viewModel, DepartmentMoverFactory departmentMoverFactory)
        {
            if (messageBoxFacade == null) throw new ArgumentNullException(nameof(messageBoxFacade));
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (departmentMoverFactory == null) throw new ArgumentNullException(nameof(departmentMoverFactory));

            DataContext = viewModel;
            InitializeComponent();
            _messageBoxFacade = messageBoxFacade;
            _kernel = kernel;
            _viewModel = viewModel;
            _departmentMoverFactory = departmentMoverFactory;
        }

        public async Task Initialize()
        {
            IsEnabled = false;
            IsEnabled = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var control = (Control)sender;
                if (control.Name == "FilterBox")
                    UpdateFilter();
                else if (control.Name == "MainComponent" && Keyboard.IsKeyDown(Key.LeftCtrl) && e.Key == Key.U)
                {
                    var adminViewModel = new AdminViewModel { CurrentUser = _viewModel.CurrentUser };
                    var adminWindow = _kernel.Get<AdminWindow>(new ConstructorArgument("viewModel", adminViewModel));
                    adminWindow.Owner = this;
                    adminWindow.ShowInTaskbar = false;
                    if (!adminWindow.ShowDialog().Value)
                        return;
                }
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SearchUser();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private void MoveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentRole == null) throw new ArgumentNullException(nameof(_viewModel.CurrentRole));

                var selectedRemDepartments = _viewModel.SelectedFilteredDepartments.Cast<Department>().ToList();
                var selectedUserDepartments = _viewModel.SelectedUserDepartments.Cast<Department>().ToList();
                var mover = _departmentMoverFactory.GetItemMover(((Control)sender).Name, _viewModel.RemainingDepartments,
                    _viewModel.UserDepartments, selectedRemDepartments, selectedUserDepartments);

                _viewModel.CurrentRole.IsDirty = mover.Transfer();
                UpdateFilter();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }
        private async void Roles_MouseDown(object sender, EventArgs e)
        {
            try
            {
                if (_viewModel.CurrentRole?.IsDirty != true) return;
                if (!_messageBoxFacade.Confirm(switchRoleConfirmation)) return;
                if (string.IsNullOrEmpty(_viewModel.SearchUserBox))
                    _viewModel.SearchUserBox = _viewModel.CurrentUser?.Email;
                await SearchUser();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private async void Roles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentRole == null || _viewModel.CurrentUser == null)
                {
                    _viewModel.ClearDepartments();
                    return;
                }

                IsEnabled = false;
                _viewModel.CurrentRole.IsDirty = false;
                _viewModel.RemainingDepartments = _viewModel.RawDepartments.Where(rd => !_viewModel.UserDepartments.Contains(rd)).ToList();
                _viewModel.FilteredDepartments = _viewModel.RemainingDepartments.ToObservableCollection();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private void Scopes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentUser == null || _viewModel.CurrentRole == null) return;

                var itemsChanged = e.RemovedItems.Cast<RoleItem>().Any();
                var oldScope = e.RemovedItems.Cast<RoleItem>().SingleOrDefault();
                if (!itemsChanged || oldScope == _viewModel.CurrentRole.RoleItem) return;

                _viewModel.CurrentRole.IsDirty = true;
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentUser == null)
                    throw new ArgumentException("No user currently selected");

                IsEnabled = false;
                _viewModel.CurrentRole.IsDirty = false;
                _messageBoxFacade.ShowInformational("Saved Current User/Role");
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsEnabled = false;
                _viewModel.Clear();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SearchUser();

                var addUserViewModel = new AddUserViewModel
                {
                    CurrentUser = _viewModel.CurrentUser,
                    NewUser = _viewModel.CurrentUser.CreateTemplate(),
                    Departments = _viewModel.RawDepartments.OrderBy(d => d.Name).ToObservableCollection()
                };
                var addUserWindow = _kernel.Get<AddUserWindow>(new ConstructorArgument("viewModel", addUserViewModel));
                addUserWindow.Owner = this;
                addUserWindow.ShowInTaskbar = false;
                if (!addUserWindow.ShowDialog().Value) return;
                _viewModel.SearchUserBox = addUserViewModel.NewUser.Email;
                await SearchUser();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private async void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SearchUser();

                var allRoles = Enumerable.Empty<Role>();

                var addRoleViewModel = new AddRoleViewModel
                {
                    CurrentUser = _viewModel.CurrentUser,
                    AllRoles = allRoles,
                    CurrentRole = allRoles.FirstOrDefault()
                };
                var addRoleWindow = _kernel.Get<AddRoleWindow>(new ConstructorArgument("viewModel", addRoleViewModel));
                addRoleWindow.Owner = this;
                addRoleWindow.ShowInTaskbar = false;
                if (!addRoleWindow.ShowDialog().Value)
                    return;
                _viewModel.CurrentRole = _viewModel.CurrentUser.Roles.LastOrDefault();
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
        }

        private async void RevokeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentRole == null) throw new ArgumentNullException(nameof(_viewModel.CurrentRole));
                if (_viewModel.CurrentUser == null) throw new ArgumentNullException(nameof(_viewModel.CurrentUser));
                if (!_messageBoxFacade.Confirm(revokeRoleConfirmation)) return;

                IsEnabled = false;
                _viewModel.CurrentUser.Roles.Remove(_viewModel.CurrentRole);
                _viewModel.CurrentRole = _viewModel.CurrentUser.Roles.FirstOrDefault();
                _messageBoxFacade.ShowInformational("The role was successfully removed from the user");
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private async Task SearchUser()
        {
            if (string.IsNullOrEmpty(_viewModel.SearchUserBox))
                throw new ArgumentException("A userId must be provided");
            throw new NotImplementedException();
            _viewModel.ClearDepartments();
            _viewModel.CurrentRole = _viewModel.CurrentUser.Roles.FirstOrDefault();
        }

        private void UpdateFilter()
        {
            if (_viewModel.FilterBox == null) throw new ArgumentNullException(nameof(_viewModel.FilterBox));
            if (_viewModel.RemainingDepartments == null) return;

            _viewModel.FilteredDepartments = _viewModel.RemainingDepartments
                .Where(g => g.Name.ToLower().Contains(_viewModel.FilterBox.ToLower()))
                .OrderBy(d => d.Name).ToObservableCollection();
            _viewModel.UserDepartments = _viewModel.UserDepartments.OrderBy(d => d.Name).ToObservableCollection();
        }
    }
}