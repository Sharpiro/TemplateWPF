using TemplateWpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace TemplateWPF.Windows
{
    public partial class AddRoleWindow : Window
    {
        private readonly AddRoleViewModel _viewModel;
        private readonly MessageBoxFacade _messageBoxFacade;

        public AddRoleWindow(AddRoleViewModel viewModel, MessageBoxFacade messageBoxFacade)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (messageBoxFacade == null) throw new ArgumentNullException(nameof(messageBoxFacade));

            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
            _messageBoxFacade = messageBoxFacade;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentUser == null) throw new ArgumentNullException(nameof(_viewModel.CurrentUser));
                if (_viewModel.CurrentRole == null) throw new ArgumentNullException(nameof(_viewModel.CurrentRole));

                var control = (Control)sender;
                if (control.Name == "CancelButton")
                    DialogResult = false;
                else
                {
                    _viewModel.CurrentRole.IsDirty = true;
                    _viewModel.CurrentUser.AddRole(_viewModel.CurrentRole);
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex.Message);
            }
        }
    }
}