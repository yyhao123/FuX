using FuX.Core.handler;
using FuX.Unility;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Model.@enum;
using FuX.Log;
using FuX.Model.attribute;

namespace FuX.Core.extend
{
    //
    // 摘要:
    //     核心统一类，统一实现单例、事件、统一出入参、函数过程时间记录、详细日志输出、创建实例、获取参数、日志管理、多语言；
    //     O、D都约定为类；
    //
    // 类型参数:
    //   O:
    //     操作类
    //
    //   D:
    //     基础数据类，构造参数类
    public class CoreUnify<O, D> : IGetParam, ICreateInstance, IEvent, ILog, ILanguage, IDisposable where O : class where D : class
    {
        //
        // 摘要:
        //     锁
        private static readonly object objLock = new object();

        //
        // 摘要:
        //     最大实例数量
        protected static readonly int maxInstanceCount = 200;

        //
        // 摘要:
        //     实例数超过限制提示
        private static readonly string exceedMaxInstanceCountTips = string.Format("exceedMaxInstanceCountTips".GetLanguageValue(), maxInstanceCount);

        //
        // 摘要:
        //     创建实例失败
        private static readonly string createInstanceErrorTips = string.Format("createInstanceErrorTips".GetLanguageValue(), maxInstanceCount);

        //
        // 摘要:
        //     单例集合；
        //     Key:基础数据；
        //     Value:操作对象；
        protected static readonly ConcurrentDictionary<D, O> objList = new ConcurrentDictionary<D, O>();

        //
        // 摘要:
        //     数据传递包装器异步
        private EventingWrapperAsync<EventDataResult> OnDataEventWrapperAsync;

        //
        // 摘要:
        //     信息传递包装器异步
        private EventingWrapperAsync<EventInfoResult> OnInfoEventWrapperAsync;

        //
        // 摘要:
        //     信息传递包装器异步
        private EventingWrapperAsync<EventLanguageResult> OnLanguageEventWrapperAsync;

        //
        // 摘要:
        //     基础参数
        protected D basics { get; set; }

        //
        // 摘要:
        //     全局标识，用于统一返回
        protected string TAG => typeof(O).Name;

        //
        // 摘要:
        //     中文名称；
        //     虚属性，可选重写；
        //     如果无法使用中文就使用英文即可；
        protected virtual string CN { get; }

        //
        // 摘要:
        //     中文描述；
        //     虚属性，可选重写
        protected virtual string CD { get; }

        //
        // 摘要:
        //     额外添加属性值，获取参数时使用；
        //     虚属性，可选重写
        protected virtual List<ParamModel.propertie> AP { get; }

        public virtual LanguageModel LanguageOperate { get; set; }

        public event EventHandler<EventDataResult> OnDataEvent;

        public event EventHandlerAsync<EventDataResult> OnDataEventAsync
        {
            add
            {
                OnDataEventWrapperAsync.AddHandler(value);
            }
            remove
            {
                OnDataEventWrapperAsync.RemoveHandler(value);
            }
        }

        public event EventHandler<EventInfoResult> OnInfoEvent;

        public event EventHandlerAsync<EventInfoResult> OnInfoEventAsync
        {
            add
            {
                OnInfoEventWrapperAsync.AddHandler(value);
            }
            remove
            {
                OnInfoEventWrapperAsync.RemoveHandler(value);
            }
        }

        public event EventHandler<EventLanguageResult> OnLanguageEvent;

        public event EventHandlerAsync<EventLanguageResult> OnLanguageEventAsync
        {
            add
            {
                OnLanguageEventWrapperAsync.AddHandler(value);
            }
            remove
            {
                OnLanguageEventWrapperAsync.RemoveHandler(value);
            }
        }

        //
        // 摘要:
        //     无惨构造函数
        protected CoreUnify()
        {
            RegisterLanguageEvent();
        }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   param:
        //     参数
        protected CoreUnify(D param)
        {
            basics = param;
            RegisterLanguageEvent();
        }

        //
        // 摘要:
        //     释放所有资源
        //     注意:
        //     外部重写此方法并且释放完自身资源后
        //     请在执行如下方法
        //     base.Dispose();
        public virtual void Dispose()
        {
            objList.Remove(basics, out var _);
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        //
        // 摘要:
        //     异步释放所有资源
        //     注意:
        //     外部重写此方法并且释放完自身资源后
        //     请在执行如下方法
        //     base.DisposeAsync();
        public virtual Task DisposeAsync()
        {
            Dispose();
            return Task.CompletedTask;
        }

        //
        // 摘要:
        //     数据源传递
        //
        // 参数:
        //   sender:
        //     自身对象
        //
        //   e:
        //     事件结果
        protected void OnDataEventHandler(object? sender, EventDataResult e)
        {
            this.OnDataEvent?.Invoke(sender, e);
            OnDataEventWrapperAsync.InvokeAsync(sender, e);
        }

        //
        // 摘要:
        //     消息源传递
        //
        // 参数:
        //   sender:
        //     自身对象
        //
        //   e:
        //     事件结果
        protected void OnInfoEventHandler(object? sender, EventInfoResult e)
        {
            this.OnInfoEvent?.Invoke(sender, e);
            OnInfoEventWrapperAsync.InvokeAsync(sender, e);
        }

        //
        // 摘要:
        //     消息源传递
        //
        // 参数:
        //   sender:
        //     自身对象
        //
        //   e:
        //     事件结果
        protected void OnLanguageEventHandler(object? sender, EventLanguageResult e)
        {
            this.OnLanguageEvent?.Invoke(sender, e);
            OnLanguageEventWrapperAsync.InvokeAsync(sender, e);
        }

        //
        // 摘要:
        //     注册语言事件
        private void RegisterLanguageEvent()
        {
            EventHandler<EventLanguageResult> value = delegate (object? sender, EventLanguageResult e)
            {
                this.OnLanguageEvent?.Invoke(sender, e);
            };
            EventHandlerAsync<EventLanguageResult> value2 = delegate (object? sender, EventLanguageResult e)
            {
                object sender2 = sender;
                EventLanguageResult e2 = e;
                return Task.Run(delegate
                {
                    OnLanguageEventWrapperAsync.InvokeAsync(sender2, e2);
                });
            };
            LanguageHandler.OnLanguageEvent -= value;
            LanguageHandler.OnLanguageEvent += value;
            LanguageHandler.OnLanguageEventAsync -= value2;
            LanguageHandler.OnLanguageEventAsync += value2;
        }

        //
        // 摘要:
        //     控制台输出标题
        //     虚方法，可选重写
        //
        // 返回结果:
        //     控制台输出标题
        public virtual string Title()
        {
            return "https://Shunnet.top";
        }

        //
        // 摘要:
        //     全局配置文件默认路径
        //     虚方法，可选重写
        //
        // 返回结果:
        //     返回定义好的路径，供全局使用
        public virtual string GlobalConfigDefaultPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "config";
        }

        //
        // 摘要:
        //     创建单例
        //
        // 参数:
        //   param:
        //     参数
        //
        // 返回结果:
        //     对象
        private static O? CreateInstance(object param)
        {
            ConstructorInfo constructor = typeof(O).GetConstructor(new Type[1] { typeof(D) });
            if (constructor != null)
            {
                return constructor.Invoke(new object[1] { param }) as O;
            }

            return null;
        }

        //
        // 摘要:
        //     单例模式
        //
        // 参数:
        //   param:
        //     基础参数
        //     不传入则使用默认，直接创建默认对象
        //     如果这个单例还想使用时，请使用InstanceBasics方法设置基础数据
        //
        // 返回结果:
        //     对象
        public static O Instance(D? param = null)
        {
            D param2 = param;
            if (objList.Count >= maxInstanceCount)
            {
                throw new Exception(exceedMaxInstanceCountTips);
            }

            if (param2 == null)
            {
                param2 = Activator.CreateInstance(typeof(D)) as D;
                if (param2 == null)
                {
                    throw new Exception(typeof(D).Name + " " + "实例无法创建".GetLanguageValue());
                }
            }

            O value = objList.FirstOrDefault((KeyValuePair<D, O> c) => c.Key.Comparer(param2).result).Value;
            if (value == null)
            {
                lock (objLock)
                {
                    O val = CreateInstance((object)param2);
                    if (val == null)
                    {
                        throw new Exception(createInstanceErrorTips);
                    }

                    objList.TryAdd(param2, val);
                    return val;
                }
            }

            return value;
        }

        //
        // 摘要:
        //     设置单例基础数据
        //     此功能用于创建的单例没有传入基础数据的情况,并且还要继续使用此单例对象
        //     此方法对应 Instance 方法未传入基础数据后使用
        //
        // 参数:
        //   param:
        //     基础参数
        //
        // 返回结果:
        //     true : 此单例对象基础数据已被重置为新的对象
        //     false : 重置失败,此单例对象不在单例容器中,可能是直接new的对象
        public bool InstanceBasics(D param)
        {
            if (objList.TryGetValue(basics, out var value))
            {
                lock (objLock)
                {
                    objList.Remove(basics, out var _);
                    basics = param;
                    objList.TryAdd(basics, value);
                }

                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     异步单例模式
        //
        // 参数:
        //   param:
        //     基础参数
        //     不传入则使用默认，直接创建默认对象
        //     如果这个单例还想使用时，请使用InstanceBasics方法设置基础数据
        //
        //   token:
        //     传播消息取消通知
        //
        // 返回结果:
        //     对象
        public static Task<O> InstanceAsync(D? param = null, CancellationToken token = default(CancellationToken))
        {
            D param2 = param;
            return Task.Run(() => Instance(param2), token);
        }

        //
        // 摘要:
        //     异步设置单例基础数据
        //     此功能用于创建的单例没有传入基础数据的情况,并且还要继续使用此单例对象
        //     此方法对应 Instance 方法未传入基础数据后使用
        //
        // 参数:
        //   param:
        //     基础参数
        //
        //   token:
        //     传播消息取消通知
        //
        // 返回结果:
        //     true : 此单例对象基础数据已被重置为信的对象
        //     false : 重置失败,此单例对象不在单例容器中,可能是直接new的对象
        public Task<bool> InstanceBasicsAsync(D param, CancellationToken token = default(CancellationToken))
        {
            D param2 = param;
            return Task.Run(() => InstanceBasics(param2), token);
        }

        //
        // 摘要:
        //     开始操作
        //     记录函数开始时间
        //
        // 参数:
        //   methodName:
        //     方法名
        //
        // 返回结果:
        //     方法名
        protected string BegOperate([CallerMemberName] string methodName = "")
        {
            TimeHandler.Instance(TAG + "." + methodName).StartRecord();
            return methodName;
        }

        //
        // 摘要:
        //     异步开始操作
        //     记录函数开始时间
        //
        // 参数:
        //   token:
        //     传播消息取消通知
        //
        //   methodName:
        //     方法名
        //
        // 返回结果:
        //     方法名
        protected Task<string> BegOperateAsync(CancellationToken token = default(CancellationToken), [CallerMemberName] string methodName = "")
        {
            string methodName2 = methodName;
            return Task.Run(() => BegOperate(methodName2), token);
        }

        //
        // 摘要:
        //     结束操作
        //     记录函数运行结束时间，并返回运行时间
        //
        // 参数:
        //   status:
        //     状态
        //
        //   message:
        //     消息
        //
        //   resultData:
        //     结果数据
        //
        //   exception:
        //     异常信息
        //
        //   logOutput:
        //     日志输出
        //     true : 本地路径日志输出
        //     false : 控制台输出也会失效
        //
        //   consoleOutput:
        //     控制台输出
        //
        //   filePath:
        //     文件路径
        //
        //   methodName:
        //     方法名
        //
        //   lineNumber:
        //     行号
        //
        // 返回结果:
        //     组织好的统一结果
        protected OperateResult EndOperate(bool status, string? message = null, object? resultData = null, Exception? exception = null, bool logOutput = true, bool consoleOutput = true, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            int item = TimeHandler.Instance(TAG + "." + methodName).StopRecord().milliseconds;
            string value = ((GetLanguage() == LanguageType.en) ? "," : "，");
            string input = $"[ {TAG} ]( {methodName} {(status ? GetLanguageValue("成功") : (GetLanguageValue("异常") + $" < {GetLanguageValue("在")} {Path.GetFileName(filePath)} {GetLanguageValue("文件")}{value}{GetLanguageValue("第")} {lineNumber} {GetLanguageValue("行")} >"))} ){(string.IsNullOrEmpty(message) ? string.Empty : (" : " + message))}";
            input = input.MessageStandard();
            if (!status && logOutput)
            {
                LogHelper.Error(input, TAG + "/" + methodName + ".log", exception, consoleOutput);
            }

            return new OperateResult(status, input, item, resultData);
        }

        //
        // 摘要:
        //     异步结束操作
        //     记录函数运行结束时间，并返回运行时间
        //
        // 参数:
        //   status:
        //     状态
        //
        //   message:
        //     消息
        //
        //   resultData:
        //     结果数据
        //
        //   exception:
        //     异常信息
        //
        //   logOutput:
        //     日志输出
        //     true : 本地路径日志输出
        //     false : 控制台输出也会失效
        //
        //   consoleOutput:
        //     控制台输出
        //
        //   token:
        //     传播消息取消通知
        //
        //   filePath:
        //     文件路径
        //
        //   methodName:
        //     方法名
        //
        //   lineNumber:
        //     行号
        //
        // 返回结果:
        //     组织好的统一结果
        protected Task<OperateResult> EndOperateAsync(bool status, string? message = null, object? resultData = null, Exception? exception = null, bool logOutput = true, bool consoleOutput = true, CancellationToken token = default(CancellationToken), [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            string message2 = message;
            object resultData2 = resultData;
            Exception exception2 = exception;
            string filePath2 = filePath;
            string methodName2 = methodName;
            return Task.Run(() => EndOperate(status, message2, resultData2, exception2, logOutput, consoleOutput, filePath2, methodName2, lineNumber), token);
        }

        //
        // 摘要:
        //     结束操作
        //
        // 参数:
        //   result:
        //     已经组织到的结果数据
        //
        //   logOutput:
        //     日志输出
        //     true : 本地路径日志输出
        //     false : 控制台输出也会失效
        //
        //   consoleOutput:
        //     控制台输出
        //
        //   filePath:
        //     文件路径
        //
        //   methodName:
        //     方法名
        //
        //   lineNumber:
        //     行号
        //
        // 返回结果:
        //     组织好的统一结果
        protected OperateResult EndOperate(OperateResult result, bool logOutput = true, bool consoleOutput = true, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            int item = TimeHandler.Instance(TAG + "." + methodName).StopRecord().milliseconds;
            string value = ((GetLanguage() == LanguageType.en) ? "," : "，");
            string input = $"[ {TAG} ]( {methodName} {(result.Status ? GetLanguageValue("成功") : (GetLanguageValue("异常") + $" < {GetLanguageValue("在")} {Path.GetFileName(filePath)} {GetLanguageValue("文件")}{value}{GetLanguageValue("第")} {lineNumber} {GetLanguageValue("行")} >"))} ){(string.IsNullOrEmpty(result.Message) ? string.Empty : (" : " + result.Message))}";
            input = input.MessageStandard();
            if (!result.Status && logOutput)
            {
                LogHelper.Error(input, TAG + "/" + methodName + ".log", null, consoleOutput);
            }

            return new OperateResult(result, item);
        }

        //
        // 摘要:
        //     异步结束操作
        //
        // 参数:
        //   result:
        //     已经组织到的结果数据
        //
        //   logOutput:
        //     日志输出
        //     true : 本地路径日志输出
        //     false : 控制台输出也会失效
        //
        //   consoleOutput:
        //     控制台输出
        //
        //   token:
        //     传播消息取消通知
        //
        //   filePath:
        //     文件路径
        //
        //   methodName:
        //     方法名
        //
        //   lineNumber:
        //     行号
        //
        // 返回结果:
        //     组织好的统一结果
        protected Task<OperateResult> EndOperateAsync(OperateResult result, bool logOutput = true, bool consoleOutput = true, CancellationToken token = default(CancellationToken), [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
            OperateResult result2 = result;
            string filePath2 = filePath;
            string methodName2 = methodName;
            return Task.Run(() => EndOperate(result2, logOutput, consoleOutput, filePath2, methodName2, lineNumber), token);
        }

        public OperateResult GetParam(bool getBasicsParam = false)
        {
            BegOperate("GetParam");
            try
            {
                D val = Activator.CreateInstance<D>();
                if (getBasicsParam)
                {
                    return EndOperate(status: true, val.ToJson(formatting: true), val, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetParam", 519);
                }

                return EndOperate(ParamHandler.Get(val, string.IsNullOrWhiteSpace(CN) ? typeof(O).FullName : CN, CD ?? GetLanguageValue("暂无描述"), AP), logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetParam", 521);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetParam", 525);
            }
        }

        public OperateResult GetAutoAllocatingParam()
        {
            BegOperate("GetAutoAllocatingParam");
            try
            {
                if (ExistsAutoAllocatingParam().GetDetails(out string message, out object resultData))
                {
                    string pName = resultData?.GetSource<Tuple<Type, string, ReflexHandler.LibInstanceParam>>()?.Item2.ToString();
                    if (GetParam().GetDetails(out message, out resultData))
                    {
                        ParamModel.subsetSimplify subsetSimplify = resultData?.GetSource<ParamModel>()?.Subset?.FirstOrDefault((ParamModel.subset c) => c.Name == typeof(D)?.GetProperty(pName)?.GetValue(basics)?.ToString()).ToJson()?.ToJsonEntity<ParamModel.subsetSimplify>();
                        return EndOperate(status: true, subsetSimplify?.ToJson(), subsetSimplify, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetAutoAllocatingParam", 542);
                    }
                }

                return EndOperate(status: false, message + "，" + GetLanguageValue("无法获取自动分配标识的属性值所对应参数集合"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetAutoAllocatingParam", 545);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "GetAutoAllocatingParam", 549);
            }
        }

        public OperateResult ExistsAutoAllocatingParam()
        {
            BegOperate("ExistsAutoAllocatingParam");
            try
            {
                if (basics != null)
                {
                    Tuple<Type, string, ReflexHandler.LibInstanceParam> tuple = ReflexHandler.GetClassAllPropertyData<D>().Select(delegate (ReflexHandler.LibInstanceParam c)
                    {
                        AutoAllocatingTagAttribute customAttribute = typeof(D).GetProperty(c.Name).GetCustomAttribute<AutoAllocatingTagAttribute>();
                        return (customAttribute != null) ? new Tuple<Type, string, ReflexHandler.LibInstanceParam>(customAttribute.EnumType, c.Name, c) : null;
                    }).FirstOrDefault((Tuple<Type, string, ReflexHandler.LibInstanceParam> c) => c != null);
                    if (tuple != null)
                    {
                        return EndOperate(status: true, null, tuple, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "ExistsAutoAllocatingParam", 575);
                    }

                    return EndOperate(status: false, GetLanguageValue("ExistsAutoAllocatingParam_msg1"), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "ExistsAutoAllocatingParam", 577);
                }

                return EndOperate(status: false, GetLanguageValue("ExistsAutoAllocatingParam_msg2"), null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "ExistsAutoAllocatingParam", 579);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "ExistsAutoAllocatingParam", 583);
            }
        }

        public OperateResult CreateInstance<T>(T param)
        {
            BegOperate("CreateInstance");
            try
            {
                if (typeof(T).FullName.Equals(typeof(D).FullName))
                {
                    return EndOperate(status: true, null, Instance(param as D), null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "CreateInstance", 595);
                }

                return EndOperate(status: false, GetLanguageValue("对象类型错误，无法创建实例"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "CreateInstance", 599);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "CreateInstance", 604);
            }
        }

        public OperateResult CreateInstance(string json)
        {
            D val = json.ToJsonEntity<D>();
            if (val == null)
            {
                return OperateResult.CreateFailureResult(GetLanguageValue("JSON反序列化失败，JSON数据对象类型错误，无法创建实例"));
            }

            return CreateInstance(val);
        }

        public OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null)
        {
            BegOperate("LogOperateSet");
            try
            {
                LogModel logModel = LogHelper.Get();
                logModel.Out = logOut;
                logModel.ConsoleOut = consoleOut;
                LogHelper.Set(logModel);
                return EndOperate(status: true, GetLanguageValue("日志参数设置成功"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 627);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 631);
            }
        }

        public OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null, Action<string, LogEventLevel, string?, Exception?>? notice = null)
        {
            BegOperate("LogOperateSet");
            try
            {
                LogModel logModel = LogHelper.Get();
                logModel.Notice = notice;
                logModel.Out = logOut;
                logModel.ConsoleOut = consoleOut;
                LogHelper.Set(logModel);
                return EndOperate(status: true, GetLanguageValue("日志参数设置成功"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 645);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 649);
            }
        }

        public OperateResult LogOperateSet(bool logOut = true, bool? consoleOut = null, Func<string, LogEventLevel, string?, Exception?, Task>? noticeAsync = null)
        {
            BegOperate("LogOperateSet");
            try
            {
                LogModel logModel = LogHelper.Get();
                logModel.NoticeAsync = noticeAsync;
                logModel.Out = logOut;
                logModel.ConsoleOut = consoleOut;
                LogHelper.Set(logModel);
                return EndOperate(status: true, GetLanguageValue("日志参数设置成功"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 663);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 667);
            }
        }

        public OperateResult LogOperateSet(LogModel logModel)
        {
            BegOperate("LogOperateSet");
            try
            {
                LogHelper.Set(logModel);
                return EndOperate(status: true, GetLanguageValue("日志参数设置成功"), null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 677);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateSet", 681);
            }
        }

        public OperateResult LogOperateGet()
        {
            BegOperate("LogOperateGet");
            try
            {
                return EndOperate(status: true, GetLanguageValue("日志参数获取成功"), LogHelper.Get(), null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateGet", 690);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\extend\\CoreUnify.cs", "LogOperateGet", 694);
            }
        }

        public OperateResult GetBasicsData()
        {
            if (basics == null)
            {
                return OperateResult.CreateFailureResult(GetLanguageValue("获取失败，基础数据尚未实例化"));
            }

            return OperateResult.CreateSuccessResult(GetLanguageValue("获取成功"), basics);
        }

        public async Task<OperateResult> GetParamAsync(bool getBasicsParam = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetParam(getBasicsParam), token);
        }

        public async Task<OperateResult> GetAutoAllocatingParamAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetAutoAllocatingParam(), token);
        }

        public async Task<OperateResult> ExistsAutoAllocatingParamAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => ExistsAutoAllocatingParam(), token);
        }

        public async Task<OperateResult> CreateInstanceAsync<T>(T param, CancellationToken token = default(CancellationToken))
        {
            T param2 = param;
            return await Task.Run(() => CreateInstance(param2), token);
        }

        public async Task<OperateResult> CreateInstanceAsync(string json, CancellationToken token = default(CancellationToken))
        {
            string json2 = json;
            return await Task.Run(() => CreateInstance(json2), token);
        }

        public async Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => LogOperateSet(logOut, consoleOut), token);
        }

        public async Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, Action<string, LogEventLevel, string?, Exception?>? notice = null, CancellationToken token = default(CancellationToken))
        {
            Action<string, LogEventLevel, string?, Exception?> notice2 = notice;
            return await Task.Run(() => LogOperateSet(logOut, consoleOut, notice2), token);
        }

        public async Task<OperateResult> LogOperateSetAsync(bool logOut = true, bool? consoleOut = null, Func<string, LogEventLevel, string?, Exception?, Task>? noticeAsync = null, CancellationToken token = default(CancellationToken))
        {
            Func<string, LogEventLevel, string?, Exception?, Task> noticeAsync2 = noticeAsync;
            return await Task.Run(() => LogOperateSet(logOut, consoleOut, noticeAsync2), token);
        }

        public async Task<OperateResult> LogOperateSetAsync(LogModel logModel, CancellationToken token = default(CancellationToken))
        {
            LogModel logModel2 = logModel;
            return await Task.Run(() => LogOperateSet(logModel2), token);
        }

        public async Task<OperateResult> LogOperateGetAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => LogOperateGet(), token);
        }

        public async Task<OperateResult> GetBasicsDataAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetBasicsData(), token);
        }

        public string? GetLanguageValue(string key, LanguageModel? languageModel = null)
        {
            return key.GetLanguageValue(languageModel);
        }

        public LanguageType GetLanguage()
        {
            return LanguageHandler.GetLanguage();
        }

        public bool SetLanguage(LanguageType language)
        {
            return language.SetLanguage();
        }

        public async Task<string?> GetLanguageValueAsync(string key, LanguageModel? languageModel = null, CancellationToken token = default(CancellationToken))
        {
            return await key.GetLanguageValueAsync(languageModel, token);
        }

        public async Task<LanguageType> GetLanguageAsync(CancellationToken token = default(CancellationToken))
        {
            return await LanguageHandler.GetLanguageAsync(token);
        }

        public async Task<bool> SetLanguageAsync(LanguageType language, CancellationToken token = default(CancellationToken))
        {
            return await language.SetLanguageAsync(token);
        }
    }
}
