using System;
using System.Collections.ObjectModel;
using TemplateWPF.Tools;

namespace TemplateWpf.Models
{
    public class User : BaseViewModel
    {
        private int _id;
        private string _name;
        private string _email;
        private bool _isDone;
        private DateTime _dateHired = Extensions.GetEpochStartTime;
        public Department Department { get; set; }
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();

        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged(); } }
        public bool IsDone { get { return _isDone; } set { _isDone = value; OnPropertyChanged(); } }
        public DateTime DateHired { get { return _dateHired; } set { _dateHired = value; OnPropertyChanged(); } }
        public ObservableCollection<Role> Roles { get { return _roles; } set { _roles = value; OnPropertyChanged(); } }


        public void AddRole(Role newRole)
        {
            if (_roles.Contains(newRole))
                throw new InvalidOperationException("The role has already been assigned to the user");

            _roles.Add(newRole);
            OnPropertyChanged("Roles");
        }

        public User CreateTemplate()
        {
            var newUser = new User();
            newUser.Department = Department;
            foreach (var role in Roles)
            {
                newUser.Roles.Add(role.Copy());
            }
            return newUser;
        }
    }
}