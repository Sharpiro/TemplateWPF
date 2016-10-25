using TemplateWpf.Models;
using System;
using System.Windows;

namespace TemplateWPF.Windows
{
    public partial class AdminWindow : Window
    {
        private const string deleteConfirmation = "Are you sure you want to delete this user?";

        private readonly MessageBoxFacade _messageBoxFacade;
        private readonly AdminViewModel _viewModel;

        public AdminWindow(AdminViewModel viewModel, MessageBoxFacade messageBoxFacade)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (messageBoxFacade == null) throw new ArgumentNullException(nameof(messageBoxFacade));
            InitializeComponent();

            DataContext = viewModel;
            _viewModel = viewModel;
            _messageBoxFacade = messageBoxFacade;
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.CurrentUser == null) throw new ArgumentNullException(nameof(_viewModel.CurrentUser));

                if (!_messageBoxFacade.Confirm(deleteConfirmation)) return;
                //doDelete
                _viewModel.CurrentUser = null;
            }
            catch (Exception ex)
            {
                _messageBoxFacade.ShowError(ex.Message);
            }
        }
    }
}