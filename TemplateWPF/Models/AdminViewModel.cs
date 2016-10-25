namespace TemplateWpf.Models
{
    public class AdminViewModel : BaseViewModel
    {
        private User _currentUser;

        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; OnPropertyChanged(); } }
    }
}