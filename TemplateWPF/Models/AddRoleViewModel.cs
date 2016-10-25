using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateWpf.Models
{
    public class AddRoleViewModel : BaseViewModel
    {
        private IEnumerable<Role> _allRoles;
        private Role _currentRole;

        public User CurrentUser { get; set; }
        public Role CurrentRole { get { return _currentRole; } set { _currentRole = value; OnPropertyChanged(); } }

        public IEnumerable<Role> AllRoles { get { return _allRoles; } set { _allRoles = value; OnPropertyChanged(); } }
        public IEnumerable<RoleItem> AllRoleItems { get; set; } = Enum.GetValues(typeof(RoleItem)).Cast<RoleItem>().Skip(1).Take(3);
    }
}