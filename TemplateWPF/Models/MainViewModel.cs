using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateWpf.Models
{
    public class MainViewModel : BaseViewModel
    {
        private string _searchUserBox = string.Empty;
        private User _currentUser;
        private Role _currentRole;

        private ObservableCollection<Department> _filteredDepartments = new ObservableCollection<Department>();
        private ObservableCollection<Department> _userDepartments = new ObservableCollection<Department>();
        private IList _selectedFilteredDepartments = new List<Department>();
        private IList _selectedUserDepartments = new List<Department>();

        public string SearchUserBox { get { return _searchUserBox; } set { _searchUserBox = value; OnPropertyChanged(); } }
        public string FilterBox { get; set; } = string.Empty;

        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; OnPropertyChanged(); } }
        public Role CurrentRole { get { return _currentRole; } set { _currentRole = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentRoleActive)); } }
        public bool CurrentRoleActive { get { return _currentRole != null; } }
        public IEnumerable<RoleItem> AllScopeTypes { get; set; } = Enum.GetValues(typeof(RoleItem)).Cast<RoleItem>().Skip(1).Take(3);

        public IEnumerable<Department> RawDepartments { get; set; }
        public List<Department> RemainingDepartments { get; set; }
        public ObservableCollection<Department> FilteredDepartments { get { return _filteredDepartments; } set { _filteredDepartments = value; OnPropertyChanged(); OnPropertyChanged(nameof(FilteredDepartmentsCount)); } }
        public ObservableCollection<Department> UserDepartments { get { return _userDepartments; } set { _userDepartments = value; OnPropertyChanged(); OnPropertyChanged(nameof(UserDepartmentsCount)); } }
        public IList SelectedFilteredDepartments { get { return _selectedFilteredDepartments; } set { _selectedFilteredDepartments = value; OnPropertyChanged(); } }
        public IList SelectedUserDepartments { get { return _selectedUserDepartments; } set { _selectedUserDepartments = value; OnPropertyChanged(); } }

        public int FilteredDepartmentsCount { get { return _filteredDepartments.Count; } }
        public int UserDepartmentsCount { get { return _userDepartments.Count; } }

        public void Clear()
        {
            CurrentUser = null;
            CurrentRole = null;
            SearchUserBox = string.Empty;
            RemainingDepartments = new List<Department>();
            FilteredDepartments = new ObservableCollection<Department>();
            UserDepartments = new ObservableCollection<Department>();
        }

        public void ClearDepartments()
        {
            RemainingDepartments = new List<Department>();
            FilteredDepartments = new ObservableCollection<Department>();
            UserDepartments = new ObservableCollection<Department>();
        }
    }
}