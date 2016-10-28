using System;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;

namespace Framework
{
    [System.Serializable]
    public class Property
    {
        [UnityEngine.SerializeField]
        Component _target;
        [UnityEngine.SerializeField] 
        string _name;               // field name or property name

        FieldInfo _fieldInfo = null;
        PropertyInfo _propertyInfo = null;

        public Property() { }
        public Property(Component target, string propertyName)
        {
            _target = target;
            _name = propertyName;
        }

        public Component target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
                _propertyInfo = null;
                _fieldInfo = null;
            }
        }

        public string propertyName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                _propertyInfo = null;
                _fieldInfo = null;
            }
        }

        public bool isValid { get { return (_target != null && !string.IsNullOrEmpty(_name)); } }
        bool Cache()
        {
            if (target != null && !string.IsNullOrEmpty(propertyName))
            {
                Type type = target.GetType();
                _fieldInfo = type.GetField(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                _propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            else
            {
                _fieldInfo = null;
                _propertyInfo = null;
            }
            return (_fieldInfo != null || _propertyInfo != null);
        }

        public Type GetPropertyType()
        {
            if (_propertyInfo == null && _fieldInfo == null && isValid) Cache();
            if (_propertyInfo != null) return _propertyInfo.PropertyType;
            if (_fieldInfo != null) return _fieldInfo.FieldType;
#if UNITY_EDITOR || !UNITY_FLASH
            return typeof(void);
#else
		    return null;
#endif
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return !isValid;
            }

            if (obj is Property)
            {
                Property p = obj as Property;

                return (ReferenceEquals(target, p.target) && string.Compare(propertyName, p.propertyName, true) == 0);
            }
            return false;
        }

        static int s_Hash = "Framework.Property".GetHashCode();
        public override int GetHashCode()
        {
            return s_Hash;
        }
        public void Reset()
        {
            _fieldInfo = null;
            _propertyInfo = null;
        }
        public override string ToString() { return ToString(target, propertyName); }

        static public string ToString(Component vm, string property)
        {
            if (vm != null)
            {
                string typeName = vm.GetType().ToString();
                int period = typeName.LastIndexOf('.');
                if (period > 0) typeName = typeName.Substring(period + 1);

                if (!string.IsNullOrEmpty(property)) return typeName + "." + property;
                else return typeName + ".[property]";
            }
            return null;
        }

        //[DebuggerHidden]
        //[DebuggerStepThrough]
        public object Get()
        {
            if (_propertyInfo == null && _fieldInfo == null && isValid) Cache();

            if (_propertyInfo != null)
            {
                if (_propertyInfo.CanRead)
                    return _propertyInfo.GetValue(target, null);
            }
            else if (_fieldInfo != null)
            {
                return _fieldInfo.GetValue(target);
            }
            return null;
        }

        //[DebuggerHidden]
        //[DebuggerStepThrough]
        public bool Set(object value)
        {
            if (_propertyInfo == null && _fieldInfo == null && isValid) Cache();
            if (_propertyInfo == null && _fieldInfo == null) return false;

            if (value == null)
            {
                try
                {
                    if (_propertyInfo != null)
                    {
                        if (_propertyInfo.CanWrite)
                        {
                            _propertyInfo.SetValue(target, null, null);
                            return true;
                        }
                    }
                    else
                    {
                        _fieldInfo.SetValue(target, null);
                        return true;
                    }
                }
                catch (Exception) { return false; }
            }

            // Can we set the value?
            if (!Convert(ref value))
            {
                if (Application.isPlaying)
                    UnityEngine.Debug.LogError("Unable to convert " + value.GetType() + " to " + GetPropertyType());
            }
            else if (_fieldInfo != null)
            {
                _fieldInfo.SetValue(target, value);
                return true;
            }
            else if (_propertyInfo.CanWrite)
            {
                _propertyInfo.SetValue(target, value, null);
                return true;
            }
            return false;
        }
        bool Convert(ref object value)
        {
            if (target == null) return false;

            Type to = GetPropertyType();
            Type from;

            if (value == null)
            {
                if (!to.IsClass) return false;

                from = to;
            }
            else from = value.GetType();
            return Convert(ref value, from, to);
        }
        static public bool Convert(Type from, Type to)
        {
            object temp = null;
            return Convert(ref temp, from, to);
        }

        static public bool Convert(object value, Type to)
        {
            if (value == null)
            {
                value = null;
                return Convert(ref value, to, to);
            }
            return Convert(ref value, value.GetType(), to);
        }

        static public bool Convert(ref object value, Type from, Type to)
        {
            if (to.IsAssignableFrom(from)) return true;

            // If the target type is a string, just convert the value
            if (to == typeof(string))
            {
                value = (value != null) ? value.ToString() : "null";
                return true;
            }

            // If the value is null we should not proceed further
            if (value == null) return false;

            if (to == typeof(int))
            {
                if (from == typeof(string))
                {
                    int val;

                    if (int.TryParse((string)value, out val))
                    {
                        value = val;
                        return true;
                    }
                }
                else if (from == typeof(float))
                {
                    value = Mathf.RoundToInt((float)value);
                    return true;
                }
            }
            else if (to == typeof(float))
            {
                if (from == typeof(string))
                {
                    float val;

                    if (float.TryParse((string)value, out val))
                    {
                        value = val;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}