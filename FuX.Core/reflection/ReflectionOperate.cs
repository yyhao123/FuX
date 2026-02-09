using FuX.Core.extend;
using FuX.Model.data;
using FuX.Unility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.reflection
{
    public class ReflectionOperate : CoreUnify<ReflectionOperate, ReflectionData.Basics>, IDisposable
    {
        private bool ReflectionState;

        private ConcurrentDictionary<string, ReflectionData.ReflectionMethodResult> MethodIocContainer = new ConcurrentDictionary<string, ReflectionData.ReflectionMethodResult>();

        private ConcurrentDictionary<string, ReflectionData.ReflectionEventResult> EventIocContainer = new ConcurrentDictionary<string, ReflectionData.ReflectionEventResult>();

        private ConcurrentDictionary<string, object> ObjectIocContainer = new ConcurrentDictionary<string, object>();

        public ReflectionOperate(ReflectionData.Basics basics)
            : base(basics)
        {
        }

        public bool GetStatus()
        {
            return ReflectionState;
        }

        public OperateResult Init()
        {
            BegOperate("Init");
            try
            {
                foreach (ReflectionData.DllData dllData in base.basics.DllDatas)
                {
                    Assembly assembly = null;
                    if (dllData.IsAbsolutePath)
                    {
                        if (!File.Exists(dllData.DllPath))
                        {
                            return EndOperate(status: false, dllData.DllPath + " 文件不存在", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 80);
                        }
                        assembly = Assembly.LoadFile(dllData.DllPath);
                    }
                    else
                    {
                        string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllData.DllPath);
                        if (!File.Exists(text))
                        {
                            return EndOperate(status: false, text + " 文件不存在", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 94);
                        }
                        assembly = Assembly.LoadFile(text);
                    }
                    foreach (ReflectionData.NamespaceData namespaceData in dllData.NamespaceDatas)
                    {
                        foreach (ReflectionData.ClassData classData in namespaceData.ClassDatas)
                        {
                            string name = namespaceData.Namespace + "." + classData.ClassName;
                            Type type = assembly.GetType(name, throwOnError: false, ignoreCase: true);
                            if (type != null)
                            {
                                object obj = CreateInstance(type, classData.ConstructorParam);
                                if (obj != null && !ObjectIocContainer.ContainsKey(type.FullName))
                                {
                                    ObjectIocContainer.TryAdd(type.FullName, obj);
                                }
                                if (classData.MethodDatas != null)
                                {
                                    foreach (ReflectionData.MethodData methodData in classData.MethodDatas)
                                    {
                                        MethodInfo method = type.GetMethod(methodData.MethodName);
                                        if (method != null)
                                        {
                                            ReflectionData.ReflectionMethodResult reflectResult2 = new ReflectionData.ReflectionMethodResult
                                            {
                                                InstanceObject = ObjectIocContainer[type.FullName],
                                                Method = method
                                            };
                                            MethodIocContainer.AddOrUpdate(classData.SN + methodData.SN, reflectResult2, (string K, ReflectionData.ReflectionMethodResult V) => reflectResult2);
                                            if (methodData.WhetherExecute)
                                            {
                                                method?.Invoke(ObjectIocContainer[type.FullName], ParamTypeConvert(methodData.MethodParam, method));
                                            }
                                            continue;
                                        }
                                        return EndOperate(status: false, $"{dllData.DllPath} -> {namespaceData.Namespace}.{classData.ClassName} -> {methodData.MethodName} 方法不存在", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 148);
                                    }
                                }
                                if (classData.EventDatas == null)
                                {
                                    continue;
                                }
                                foreach (ReflectionData.EventData eventData in classData.EventDatas)
                                {
                                    EventInfo @event = type.GetEvent(eventData.EventName);
                                    if (@event != null)
                                    {
                                        ReflectionData.ReflectionEventResult reflectResult = new ReflectionData.ReflectionEventResult
                                        {
                                            InstanceObject = ObjectIocContainer[type.FullName],
                                            Event = @event
                                        };
                                        EventIocContainer.AddOrUpdate(classData.SN + eventData.SN, reflectResult, (string K, ReflectionData.ReflectionEventResult V) => reflectResult);
                                        continue;
                                    }
                                    return EndOperate(status: false, $"{dllData.DllPath} -> {namespaceData.Namespace}.{classData.ClassName} -> {eventData.EventName} 事件不存在", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 172);
                                }
                                continue;
                            }
                            return EndOperate(status: false, $"请检查 {dllData.DllPath} -> {namespaceData.Namespace}.{classData.ClassName} 类名的准确性", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 179);
                        }
                    }
                }
                ReflectionState = true;
                return EndOperate(status: true, "反射初始化成功", MethodIocContainer, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 185);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "Init", 189);
            }
        }

        private object? CreateInstance(Type? NamespaceAndClassNameType, object[]? ConstructorParam)
        {
            object[] args = null;
            if (ConstructorParam != null)
            {
                ConstructorInfo info = NamespaceAndClassNameType?.GetConstructors().FirstOrDefault();
                args = ParamTypeConvert(ConstructorParam, info);
            }
            return Activator.CreateInstance(NamespaceAndClassNameType, args);
        }

        private object[]? ParamTypeConvert(object[] Data, object Info)
        {
            if (Data != null)
            {
                ParameterInfo[] array = null;
                List<object> list = new List<object>();
                if (Info.GetType().ToString().Contains("MethodInfo"))
                {
                    array = (Info as MethodInfo).GetParameters();
                    if (array != null)
                    {
                        for (int i = 0; i < array.Count(); i++)
                        {
                            if (string.IsNullOrEmpty(array[i].ParameterType.FullName))
                            {
                                list.Add(Data[i]);
                            }
                            else
                            {
                                list.Add(Convert.ChangeType(Data[i], array[i].ParameterType));
                            }
                        }
                    }
                }
                if (Info.GetType().ToString().Contains("ConstructorInfo"))
                {
                    array = (Info as ConstructorInfo).GetParameters();
                    if (array != null)
                    {
                        for (int j = 0; j < array.Count(); j++)
                        {
                            object obj = Activator.CreateInstance(array[j].ParameterType);
                            PropertyInfo[] properties = array[j].ParameterType.GetProperties();
                            foreach (PropertyInfo propertyInfo in properties)
                            {
                                JObject jObject = JsonConvert.DeserializeObject<JObject>(Data[j].ToJson());
                                propertyInfo.SetValue(obj, Convert.ChangeType(jObject[propertyInfo.Name], propertyInfo.PropertyType));
                            }
                            if (obj != null)
                            {
                                list.Add(obj);
                            }
                        }
                    }
                }
                if (list.Count > 0)
                {
                    return list.ToArray();
                }
            }
            return null;
        }

        public object? ExecuteMethod(string SN, object[]? MethodParam = null)
        {
            ReflectionData.ReflectionMethodResult method = GetMethod(SN);
            if (method != null)
            {
                object[] parameters = ParamTypeConvert(MethodParam, method.Method);
                return method.Method?.Invoke(method.InstanceObject, parameters);
            }
            return EndOperate(status: false, "执行失败，未找到反射的数据", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", BegOperate("ExecuteMethod"), 292);
        }

        public ConcurrentDictionary<string, ReflectionData.ReflectionMethodResult>? GetMethod()
        {
            if (MethodIocContainer != null && MethodIocContainer.Count > 0)
            {
                return MethodIocContainer;
            }
            return null;
        }

        public ReflectionData.ReflectionMethodResult? GetMethod(string SN)
        {
            if (MethodIocContainer != null && MethodIocContainer.Count > 0)
            {
                return MethodIocContainer[SN];
            }
            return null;
        }

        public OperateResult RegisterEvent(string SN, bool Register, Action<object>? P1 = null, Action<object, object>? P2 = null, Action<object, object, object>? P3 = null, Action<object, object, object, object>? P4 = null, Action<object, object, object, object, object>? P5 = null, Action<object, object, object, object, object, object>? P6 = null)
        {
            BegOperate("RegisterEvent");
            ReflectionData.ReflectionEventResult @event = GetEvent(SN);
            if (@event != null)
            {
                if (@event.Event.EventHandlerType != null)
                {
                    Delegate handler = null;
                    if (P1 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P1.Target, P1.Method);
                    }
                    if (P2 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P2.Target, P2.Method);
                    }
                    if (P3 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P3.Target, P3.Method);
                    }
                    if (P4 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P4.Target, P4.Method);
                    }
                    if (P5 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P5.Target, P5.Method);
                    }
                    if (P6 != null)
                    {
                        handler = Delegate.CreateDelegate(@event.Event.EventHandlerType, P6.Target, P6.Method);
                    }
                    if (Register)
                    {
                        @event.Event.AddEventHandler(@event.InstanceObject, handler);
                        return EndOperate(status: true, "事件注册成功", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "RegisterEvent", 376);
                    }
                    @event.Event.RemoveEventHandler(@event.InstanceObject, handler);
                    return EndOperate(status: true, "事件移除成功", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "RegisterEvent", 381);
                }
                return EndOperate(status: false, "事件操作失败，未找到反射的数据类型", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "RegisterEvent", 386);
            }
            return EndOperate(status: false, "事件操作失败，未找到反射的数据", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\reflection\\ReflectionOperate.cs", "RegisterEvent", 391);
        }

        public object? ReflectionInstance(string SN)
        {
            if (ObjectIocContainer.Count > 0)
            {
                return ObjectIocContainer[SN];
            }
            return null;
        }

        public ConcurrentDictionary<string, ReflectionData.ReflectionEventResult>? GetEvent()
        {
            if (EventIocContainer != null && EventIocContainer.Count > 0)
            {
                return EventIocContainer;
            }
            return null;
        }

        public ReflectionData.ReflectionEventResult? GetEvent(string SN)
        {
            if (EventIocContainer != null && EventIocContainer.Count > 0)
            {
                return EventIocContainer[SN];
            }
            return null;
        }

        public override void Dispose()
        {
            ReflectionState = false;
            ObjectIocContainer.Clear();
            MethodIocContainer.Clear();
            EventIocContainer.Clear();
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            Dispose();
            await base.DisposeAsync();
        }
    }
}
