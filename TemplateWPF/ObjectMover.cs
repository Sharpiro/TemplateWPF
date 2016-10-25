using System;
using System.Collections.Generic;
using System.Linq;
using TemplateWpf.Models;

namespace TemplateWPF.Tools
{
    public interface IObjectMover
    {
        bool Transfer();
    }

    public class ObjectMoverSingle<T> : IObjectMover
    {
        private IList<T> _destinationList;
        private IList<T> _iterationList;
        private IList<T> _sourceList;

        public ObjectMoverSingle(IList<T> iterationList, IList<T> sourceList, IList<T> destinationList)
        {
            if (iterationList == null) throw new ArgumentNullException(nameof(iterationList));
            if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
            if (destinationList == null) throw new ArgumentNullException(nameof(destinationList));

            _iterationList = iterationList;
            _sourceList = sourceList;
            _destinationList = destinationList;
        }

        public bool Transfer()
        {
            _iterationList = _iterationList ?? Enumerable.Empty<T>().ToList();
            foreach (var department in _iterationList)
            {
                _sourceList.Remove(department);
                _destinationList.Add(department);
            }
            return _iterationList.Any();
        }
    }

    public class ObjectMoverAll<T> : IObjectMover
    {
        private IList<T> _destinationList;
        private IList<T> _sourceList;

        public ObjectMoverAll(IList<T> sourceList, IList<T> destinationList)
        {
            if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));
            if (destinationList == null) throw new ArgumentNullException(nameof(destinationList));

            _sourceList = sourceList;
            _destinationList = destinationList;
        }

        public bool Transfer()
        {
            _sourceList = _sourceList ?? Enumerable.Empty<T>().ToList();
            foreach (var department in _sourceList)
            {
                _destinationList.Add(department);
            }
            _sourceList.Clear();
            return true;
        }
    }

    public class DepartmentMoverFactory
    {
        public IObjectMover GetItemMover(string controlName, IList<Department> remainingDepartments,
            IList<Department> userDepartments, IList<Department> selectedRemainingDepartments, IList<Department> selectedUserDepartments)
        {
            switch (controlName)
            {
                case "MoveAllRight":
                    return new ObjectMoverAll<Department>(remainingDepartments, userDepartments);
                case "MoveOneRight":
                    return new ObjectMoverSingle<Department>(selectedRemainingDepartments,
                        remainingDepartments, userDepartments);
                case "MoveOneLeft":
                    return new ObjectMoverSingle<Department>(selectedUserDepartments,
                        userDepartments, remainingDepartments);
                case "MoveAllLeft":
                    return new ObjectMoverAll<Department>(userDepartments, remainingDepartments);
                default:
                    throw new ArgumentException($"the control name, '{controlName}',  is invalid");
            }
        }
    }
}