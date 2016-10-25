using System;
using System.Collections.ObjectModel;

namespace TemplateWpf.Models
{
    public class AddUserViewModel : BaseViewModel
    {
        private User _currentUser;
        private User _newUser;

        public DateTime UtcNow => DateTime.Now;
        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; OnPropertyChanged(); } }
        public User NewUser { get { return _newUser; } set { _newUser = value; OnPropertyChanged(); } }

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();
    }
}