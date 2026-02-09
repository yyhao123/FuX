using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Demo.Windows.Core.mvvm
{
    /// <summary>
    /// 通知对象
    /// </summary>
    public class NotifyObject : ObservableObject
    {
        #region 属性包

        private Dictionary<string, object> _propertyBag;
        private Dictionary<string, object> PropertyBag => _propertyBag ?? (_propertyBag = new Dictionary<string, object>());

        protected virtual bool SetPropertyCore<T>(string propertyName, T value, out T oldValue)
        {
            VerifyAccess();
            oldValue = default(T);
            if (PropertyBag.TryGetValue(propertyName, out object? val))
            {
                oldValue = (T)val;
            }
            if (CompareValues<T>(oldValue, value))
                return false;
            lock (PropertyBag)
            {
                PropertyBag[propertyName] = value;
            }
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void VerifyAccess()
        {
        }

        private static bool CompareValues<T>(T storage, T value)
        {
            return EqualityComparer<T>.Default.Equals(storage, value);
        }

        private T GetPropertyCore<T>(string propertyName)
        {
            if (PropertyBag.TryGetValue(propertyName, out object? val))
            {
                return (T)val;
            }
            return default(T);
        }

        private bool SetPropertyCore<T>(string propertyName, T value, Action changedCallback)
        {
            bool res = SetPropertyCore(propertyName, value, out T oldValue);
            if (res)
            {
                changedCallback?.Invoke();
            }
            return res;
        }

        private bool SetPropertyCore<T>(string propertyName, T value, Action<T> changedCallback)
        {
            bool res = SetPropertyCore(propertyName, value, out T oldValue);
            if (res)
            {
                changedCallback?.Invoke(oldValue);
            }
            return res;
        }

        #endregion 属性包

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return GetPropertyNameFast(expression);
        }

        internal static string GetPropertyNameFast(LambdaExpression expression)
        {
            MemberExpression? memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("成员表达式应该在表达式体中", "NotifyObject");
            }
            MemberInfo member = memberExpression.Member;
            const string vblocalPrefix = "$VB$Local_";
            if (member.MemberType == System.Reflection.MemberTypes.Field && member.Name != null && member.Name.StartsWith(vblocalPrefix))
            {
                return member.Name.Substring(vblocalPrefix.Length);
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

        protected bool SetProperty<T>(Expression<Func<T>> expression, T value)
        {
            return SetProperty(expression, value, (Action)null);
        }

        protected bool SetProperty<T>(Expression<Func<T>> expression, T value, Action changedCallback)
        {
            return SetPropertyCore(GetPropertyName(expression), value, changedCallback);
        }
    }
}