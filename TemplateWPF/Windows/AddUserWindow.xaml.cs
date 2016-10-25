using TemplateWpf.Models;
using TemplateWPF.Tools;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TemplateWPF.Windows
{
    public partial class AddUserWindow : Window
    {
        private const string confirmMessage = "Are you sure you want to create this user?";

        private readonly MessageBoxFacade _messageBoxFacade;
        private readonly AddUserViewModel _viewModel;

        public AddUserWindow(AddUserViewModel viewModel, MessageBoxFacade messageBoxFacade)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (messageBoxFacade == null) throw new ArgumentNullException(nameof(messageBoxFacade));
            if (!viewModel.Departments.Any()) throw new ArgumentNullException(nameof(viewModel.Departments));

            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
            _messageBoxFacade = messageBoxFacade;
        }

        private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentUser == null) throw new ArgumentNullException(nameof(_viewModel.CurrentUser));
                if (_viewModel.NewUser == null) throw new ArgumentNullException(nameof(_viewModel.NewUser));

                if (_viewModel.NewUser.Department == null) throw new ArgumentNullException("department");
                if (string.IsNullOrEmpty(_viewModel.NewUser.Name)) throw new ArgumentNullException("name");
                if (string.IsNullOrEmpty(_viewModel.NewUser.Email)) throw new ArgumentNullException("email");
                if (_viewModel.NewUser.DateHired < Extensions.GetEpochStartTime)
                    throw new ArgumentException("Hire date must be after Jan 1, 1970");
                if (!_messageBoxFacade.Confirm(confirmMessage)) return;

                IsEnabled = false;
                //await Task.Run(() => makeCall);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex.Message);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}