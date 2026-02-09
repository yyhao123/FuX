using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Windows.Core.mvvm
{
    public class BindNotify : ObservableObject
    {
        private Dictionary<string, object> _propertyBag;

        private Dictionary<string, object> PropertyBag => _propertyBag ?? (_propertyBag = new Dictionary<string, object>());

        private T GetPropertyCore<T>(string propertyName)
        {
            if (PropertyBag.TryGetValue(propertyName, out object value))
            {
                return (T)value;
            }

            return default(T);
        }

        protected virtual bool SetPropertyCore<T>(string propertyName, T value, out T oldValue)
        {
            VerifyAccess();
            oldValue = default(T);
            if (PropertyBag.TryGetValue(propertyName, out object value2))
            {
                oldValue = (T)value2;
            }

            if (EqualityComparer<T>.Default.Equals(oldValue, value))
            {
                return false;
            }

            lock (PropertyBag)
            {
                PropertyBag[propertyName] = value;
            }

            OnPropertyChanged(propertyName);
            return true;
        }

        private bool SetPropertyCore<T>(string propertyName, T value, Action changedCallback)
        {
            T oldValue;
            bool num = SetPropertyCore(propertyName, value, out oldValue);
            if (num)
            {
                changedCallback?.Invoke();
            }

            return num;
        }

        private bool SetPropertyCore<T>(string propertyName, T value, Action<T> changedCallback)
        {
            T oldValue;
            bool num = SetPropertyCore(propertyName, value, out oldValue);
            if (num)
            {
                changedCallback?.Invoke(oldValue);
            }

            return num;
        }

        protected virtual void VerifyAccess()
        {
        }

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return GetPropertyNameFast(expression);
        }

        internal static string GetPropertyNameFast(LambdaExpression expression)
        {
            MemberInfo member = ((expression.Body as MemberExpression) ?? throw new ArgumentException("表达式体应为成员访问表达式", "expression")).Member;
            if (member.MemberType == MemberTypes.Field && member.Name != null && member.Name.StartsWith("$VB$Local_"))
            {
                return member.Name.Substring("$VB$Local_".Length);
            }

            return member.Name;
        }

        protected T GetProperty<T>(Expression<Func<T>> expression)
        {
            return GetPropertyCore<T>(GetPropertyName(expression));
        }

        protected bool SetProperty<T>(Expression<Func<T>> expression, T value, Action<T> changedCallback)
        {
            return SetPropertyCore(GetPropertyName(expression), value, changedCallback);
        }

        protected bool SetProperty<T>(Expression<Func<T>> expression, T value, Action changedCallback)
        {
            return SetPropertyCore(GetPropertyName(expression), value, changedCallback);
        }

        protected bool SetProperty<T>(Expression<Func<T>> expression, T value)
        {
            return SetProperty(expression, value, (Action)null);
        }
    }
}
