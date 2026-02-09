using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
    //
    // 摘要:
    //     反射处理
    public class ReflexHandler
    {
        //
        // 摘要:
        //     库文件实例参数
        public class LibInstanceParam
        {
            //
            // 摘要:
            //     名称
            public string Name { get; set; }

            //
            // 摘要:
            //     描述
            public string Describe { get; set; }

            //
            // 摘要:
            //     参数类型
            public string ParamType { get; set; }

            //
            // 摘要:
            //     枚举集合
            public object EnumArray { get; set; }

            //
            // 摘要:
            //     对象数据集合
            public object ObjArray { get; set; }
        }

        //
        // 摘要:
        //     为指定对象分配参数
        //
        // 参数:
        //   dic:
        //     字段/值
        //
        // 类型参数:
        //   T:
        //     对象类型
        public T Assign<T>(Dictionary<string, object> dic) where T : new()
        {
            Type? typeFromHandle = typeof(T);
            T val = new T();
            PropertyInfo[] properties = typeFromHandle.GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (dic.Keys.Contains(propertyInfo.Name))
                {
                    object value = Convert.ChangeType(dic[propertyInfo.Name.ToString()], propertyInfo.PropertyType);
                    propertyInfo.SetValue(val, value, null);
                }
            }

            return val;
        }

        //
        // 摘要:
        //     取对象属性值
        //
        // 参数:
        //   FieldName:
        //
        //   obj:
        public static string GetModelValue(string FieldName, object obj)
        {
            try
            {
                object value = obj.GetType().GetProperty(FieldName).GetValue(obj, null);
                if (value == null)
                {
                    return null;
                }

                string text = Convert.ToString(value);
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                return text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // 摘要:
        //     设置对象属性值
        //
        // 参数:
        //   FieldName:
        //
        //   Value:
        //
        //   obj:
        public static object SetModelValue(string FieldName, string Value, object obj)
        {
            try
            {
                Type type = obj.GetType();
                if (type.GetProperty(FieldName) != null)
                {
                    object value = Convert.ChangeType(Value, type.GetProperty(FieldName).PropertyType);
                    type.GetProperty(FieldName).SetValue(obj, value, null);
                    return obj;
                }
            }
            catch
            {
            }

            return obj;
        }

        //
        // 摘要:
        //     获取方法
        //
        // 参数:
        //   MethodName:
        //     方法名
        //
        //   obj:
        //     对象
        public static MethodInfo GetMethod(string MethodName, object obj)
        {
            try
            {
                return obj.GetType().GetMethod(MethodName);
            }
            catch
            {
                return null;
            }
        }

        //
        // 摘要:
        //     将object对象转换为实体对象
        //
        // 参数:
        //   asObject:
        //     object对象
        //
        // 类型参数:
        //   T:
        //     实体对象类名
        public static T ConvertObject_One<T>(object asObject) where T : new()
        {
            T val = new T();
            if (asObject != null)
            {
                Type type = asObject.GetType();
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object obj = null;
                    object obj2 = type.GetProperty(propertyInfo.Name)?.GetValue(asObject);
                    if (obj2 != null)
                    {
                        propertyInfo.SetValue(value: (!propertyInfo.PropertyType.IsGenericType) ? Convert.ChangeType(obj2, propertyInfo.PropertyType) : ((!(propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))) ? Convert.ChangeType(obj2, propertyInfo.PropertyType) : Convert.ChangeType(obj2, Nullable.GetUnderlyingType(propertyInfo.PropertyType))), obj: val, index: null);
                    }
                }
            }

            return val;
        }

        //
        // 摘要:
        //     获取类中所有属性信息
        //
        // 类型参数:
        //   T:
        public static List<LibInstanceParam> GetClassAllPropertyData<T>()
        {
            List<LibInstanceParam> list = new List<LibInstanceParam>();
            PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                LibInstanceParam libInstanceParam = new LibInstanceParam();
                libInstanceParam.Name = propertyInfo.Name;
                libInstanceParam.Describe = propertyInfo.GetDescription();
                libInstanceParam.ParamType = propertyInfo.PropertyType.Name.ToString();
                if (propertyInfo.PropertyType.BaseType.Name.Equals("Enum"))
                {
                    libInstanceParam.ParamType = "Enum";
                    libInstanceParam.EnumArray = propertyInfo.PropertyType.GetAllItems();
                }
                else
                {
                    libInstanceParam.ObjArray = GetClassAllPropertyData(propertyInfo.PropertyType);
                }

                list.Add(libInstanceParam);
            }

            return list;
        }

        //
        // 摘要:
        //     获取类中所有属性信息
        //
        // 类型参数:
        //   T:
        public static List<LibInstanceParam>? GetClassAllPropertyData(Type type)
        {
            try
            {
                List<LibInstanceParam> list = new List<LibInstanceParam>();
                PropertyInfo[] properties = Activator.CreateInstance(type).GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in properties)
                {
                    LibInstanceParam libInstanceParam = new LibInstanceParam();
                    libInstanceParam.Name = propertyInfo.Name;
                    libInstanceParam.Describe = propertyInfo.GetDescription();
                    libInstanceParam.ParamType = propertyInfo.PropertyType.Name.ToString();
                    if (propertyInfo.PropertyType.BaseType.Name.Equals("Enum"))
                    {
                        libInstanceParam.ParamType = "Enum";
                        libInstanceParam.EnumArray = propertyInfo.PropertyType.GetAllItems();
                    }

                    list.Add(libInstanceParam);
                }

                if (list.Count > 0)
                {
                    return list;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
