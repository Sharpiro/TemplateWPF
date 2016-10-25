using System;

namespace TemplateWpf.Models
{
    public class Department : BaseViewModel, IEquatable<Department>
    {
        private int _id;
        private string _name;

        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

        public bool Equals(Department other)
        {
            return Id == other.Id;
        }
    }
}