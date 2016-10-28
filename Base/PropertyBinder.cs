using UnityEngine;

namespace Framework
{
    [ExecuteInEditMode]
    public class PropertyBinder : MonoBehaviour
    {
        public Property _source;

        public Property _target;
        
        void OnEnable()
        {
            if (_source != null && _source.isValid)
            {
                ViewModelBase vm = _source.target as ViewModelBase;
                if (vm != null)
                    vm.PropertyChanged += OnPropertyChanged;
            }
        }

        void OnDisable()
        {
            if (_source != null && _source.isValid)
            {
                ViewModelBase vm = _source.target as ViewModelBase;
                if (vm != null)
                    vm.PropertyChanged -= OnPropertyChanged;
            }
        }

        void OnPropertyChanged(object sender, string propertyName)
        {
            if(_target != null && _target.isValid )
            {
                _target.Set(_source.Get());
            }
        }
        void OnValidate()
        {
            if (_source != null)
                _source.Reset();
            if (_target != null)
                _target.Reset();
        }
    }
}