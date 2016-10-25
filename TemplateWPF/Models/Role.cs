namespace TemplateWpf.Models
{
    public class Role : BaseViewModel
    {
        private int _id;
        private string _name;
        private RoleItem _roleItem;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        public RoleItem RoleItem { get { return _roleItem; } set { _roleItem = value; OnPropertyChanged(); } }
        public bool IsDirty { get; set; }

        public Role Copy()
        {
            var newRole = (Role)MemberwiseClone();
            newRole.IsDirty = true;
            return newRole;
        }
    }
}