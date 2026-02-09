using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace FuX.Core.mq
{
    public class MqOperate : CoreUnify<MqOperate, MqData.Basics>, IDisposable
    {
        private class QueueData
        {
            public string? Topic { get; set; }

            public object? Content { get; set; }

            public List<string>? ISns { get; set; }
        }

        private class WatcherData
        {
            public enum WatcherType
            {
                Deleted,
                Created
            }

            public FileSystemEventArgs e { get; set; }

            public int Type { get; set; }

            public WatcherType WType { get; set; }
        }

        private ConcurrentDictionary<string, IMq> InstanceIoc = new ConcurrentDictionary<string, IMq>();

        private ConcurrentDictionary<string, Type>? TypeIoc;

        private FileSystemWatcher? watcherLibFolder;

        private FileSystemWatcher? watcherLibConfigFolder;

        private ConcurrentDictionary<CancellationTokenSource, Task>? TaskArray;

        private Channel<QueueData>? DataQueue;

        private BoundedChannelOptions channelOptions = new BoundedChannelOptions(int.MaxValue)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };

        private Channel<WatcherData>? WatcherQueue;

        private CancellationTokenSource? WatcherToken;

        private ConcurrentDictionary<string, IMq> OnFailIoc = new ConcurrentDictionary<string, IMq>();

        private CancellationTokenSource? OnFailToken;

        private int OnFailTaskHandlerInterval = 1000;

        public MqOperate(MqData.Basics basics)
            : base(basics)
        {
            Monitor();
        }

        private void Monitor()
        {
            if (OnFailToken == null)
            {
                OnFailToken = new CancellationTokenSource();
                OnFailTaskHandler(OnFailToken.Token);
            }
            if (TypeIoc == null)
            {
                TypeIoc = new ConcurrentDictionary<string, Type>();
            }
            if (base.basics != null)
            {
                if (!Directory.Exists(base.basics.LibFolder))
                {
                    Directory.CreateDirectory(base.basics.LibFolder);
                }
                if (!Directory.Exists(base.basics.LibConfigFolder))
                {
                    Directory.CreateDirectory(base.basics.LibConfigFolder);
                }
                Search();
                watcherLibFolder = new FileSystemWatcher(base.basics.LibFolder);
                watcherLibFolder.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security;
                watcherLibFolder.Created += delegate (object sender, FileSystemEventArgs e)
                {
                    Watcher_Created(sender, e, 0);
                };
                watcherLibFolder.Deleted += delegate (object sender, FileSystemEventArgs e)
                {
                    Watcher_Deleted(sender, e, 0);
                };
                watcherLibFolder.EnableRaisingEvents = true;
                watcherLibConfigFolder = new FileSystemWatcher(base.basics.LibConfigFolder);
                watcherLibConfigFolder.Filter = base.basics.ConfigWatcherFormat;
                watcherLibFolder.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.CreationTime | NotifyFilters.Security;
                watcherLibConfigFolder.Created += delegate (object sender, FileSystemEventArgs e)
                {
                    Watcher_Created(sender, e, 1);
                };
                watcherLibConfigFolder.Deleted += delegate (object sender, FileSystemEventArgs e)
                {
                    Watcher_Deleted(sender, e, 1);
                };
                watcherLibConfigFolder.EnableRaisingEvents = true;
            }
            else
            {
                OnInfoEventHandler(this, new EventInfoResult(status: false, "配置文件不存在"));
            }
        }

        public async Task WatcherTask(CancellationToken token)
        {
            try
            {
                await Task.Run(async delegate
                {
                    while (await WatcherQueue.Reader.WaitToReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
                    {
                        WatcherData watcherData;
                        while (WatcherQueue.Reader.TryRead(out watcherData))
                        {
                            await Task.Delay(1000);
                            FileSystemEventArgs e = watcherData.e;
                            int type = watcherData.Type;
                            _ = string.Empty;
                            switch (watcherData.WType)
                            {
                                case WatcherData.WatcherType.Deleted:
                                    {
                                        IMq value;
                                        switch (type)
                                        {
                                            case 0:
                                                if (Directory.Exists(e.FullPath))
                                                {
                                                    foreach (string item in Directory.GetFiles(e.FullPath, base.basics.DllWatcherFormat, SearchOption.AllDirectories).ToList())
                                                    {
                                                        string text4 = FileHandler.GetFileName(item).Replace(".dll", string.Empty);
                                                        foreach (KeyValuePair<string, IMq> item2 in InstanceIoc)
                                                        {
                                                            if (item2.Key.Contains(text4))
                                                            {
                                                                InstanceIoc[item2.Key].Dispose();
                                                                if (InstanceIoc.Remove(item2.Key, out value))
                                                                {
                                                                    OnInfoEventHandler(this, new EventInfoResult(status: true, e.Name + " 移除配置实例 " + item2.Key + " 成功"));
                                                                }
                                                                else
                                                                {
                                                                    OnInfoEventHandler(this, new EventInfoResult(status: false, e.Name + " 移除配置实例 " + item2.Key + " 失败"));
                                                                }
                                                            }
                                                        }
                                                        foreach (KeyValuePair<string, Type> item3 in TypeIoc)
                                                        {
                                                            if (item3.Key.Contains(text4))
                                                            {
                                                                if (TypeIoc.Remove(item3.Key, out var _))
                                                                {
                                                                    OnInfoEventHandler(this, new EventInfoResult(status: true, e.Name + " 移除程序集 " + item3.Key + " 成功"));
                                                                }
                                                                else
                                                                {
                                                                    OnInfoEventHandler(this, new EventInfoResult(status: false, e.Name + " 移除程序集 " + item3.Key + " 失败"));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case 1:
                                                {
                                                    OnInfoEventHandler(this, new EventInfoResult(status: true, e.Name + " 文件被删除，移除对应配置实例"));
                                                    string text5 = e.Name.Replace(base.basics.ConfigReplaceFormat, string.Empty);
                                                    string text4 = text5.Replace("." + text5.Split('.')[text5.Split('.').Length - 1], string.Empty);
                                                    if (InstanceIoc.ContainsKey(text5))
                                                    {
                                                        InstanceIoc[text5].Dispose();
                                                        if (InstanceIoc.Remove(text5, out value))
                                                        {
                                                            OnInfoEventHandler(this, new EventInfoResult(status: true, e.Name + " 移除配置实例成功"));
                                                        }
                                                        else
                                                        {
                                                            OnInfoEventHandler(this, new EventInfoResult(status: false, e.Name + " 移除配置实例失败"));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        OnInfoEventHandler(this, new EventInfoResult(status: false, e.Name + " 移除配置实例失败 " + text4 + " 实例不存在"));
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case WatcherData.WatcherType.Created:
                                    switch (type)
                                    {
                                        case 0:
                                            if (Directory.Exists(e.FullPath))
                                            {
                                                Search(e.FullPath);
                                            }
                                            break;
                                        case 1:
                                            {
                                                OnInfoEventHandler(this, new EventInfoResult(status: true, e.Name + " 文件新增，新增对应配置实例"));
                                                int until = 5;
                                                int i = 0;
                                                bool success = false;
                                                while (!success && i < until)
                                                {
                                                    try
                                                    {



                                                        string path = e.FullPath;
                                                       // string path = string.Format(e.FullPath, default(ReadOnlySpan<object>));
                                                        Path.GetFileName(path);
                                                        using (Stream stream = File.OpenRead(path))
                                                        {
                                                            StreamReader streamReader = new StreamReader(stream);
                                                            string text = string.Empty;
                                                            while (streamReader.Peek() > -1)
                                                            {
                                                                string text2 = streamReader.ReadLine();
                                                                text += text2;
                                                            }
                                                            streamReader.Close();
                                                            streamReader.Dispose();
                                                            string text3 = e.Name.Replace(base.basics.ConfigReplaceFormat, string.Empty);
                                                            string text4 = text3.Replace("." + text3.Split('.')[text3.Split('.').Length - 1], string.Empty);
                                                            if (TypeIoc.ContainsKey(text4))
                                                            {
                                                                if (!InstanceIoc.ContainsKey(text3))
                                                                {
                                                                    ConfigCreateInstance(TypeIoc[text4], text);
                                                                }
                                                                else
                                                                {
                                                                    OnInfoEventHandler(this, new EventInfoResult(status: false, " " + e.Name + " 此配置实例已存在"));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                OnInfoEventHandler(this, new EventInfoResult(status: false, $" {e.Name} 新增对应配置创建实例失败 {text4} 程序集不存在"));
                                                            }
                                                            stream.Close();
                                                            stream.Dispose();
                                                        }
                                                        success = true;
                                                    }
                                                    catch
                                                    {
                                                        i++;
                                                        await Task.Delay(TimeSpan.FromSeconds(1L));
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                    break;
                            }
                        }
                    }
                }, token);
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e, int Type)
        {
            if (WatcherToken == null && WatcherQueue == null)
            {
                WatcherQueue = Channel.CreateBounded<WatcherData>(channelOptions);
                WatcherToken = new CancellationTokenSource();
                WatcherTask(WatcherToken.Token);
            }
            WatcherQueue?.Writer.WriteAsync(new WatcherData
            {
                e = e,
                Type = Type,
                WType = WatcherData.WatcherType.Deleted
            }, WatcherToken.Token).ConfigureAwait(continueOnCapturedContext: false);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e, int Type)
        {
            if (WatcherToken == null && WatcherQueue == null)
            {
                WatcherQueue = Channel.CreateBounded<WatcherData>(channelOptions);
                WatcherToken = new CancellationTokenSource();
                WatcherTask(WatcherToken.Token);
            }
            WatcherQueue?.Writer.WriteAsync(new WatcherData
            {
                e = e,
                Type = Type,
                WType = WatcherData.WatcherType.Created
            }, WatcherToken.Token).ConfigureAwait(continueOnCapturedContext: false);
        }

        private void Search(string? path = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = base.basics.LibFolder;
                }
                foreach (string item in Directory.GetFiles(path, base.basics.DllWatcherFormat, SearchOption.AllDirectories).ToList())
                {
                    SearchType(item);
                }
                foreach (KeyValuePair<string, Type> item2 in TypeIoc)
                {
                    TypeSearchConfig(item2.Value);
                }
            }
            catch (Exception ex)
            {
                OnInfoEventHandler(this, new EventInfoResult(status: false, "检索异常：" + ex.Message));
            }
        }

        private void SearchType(string lib)
        {
            try
            {
                Type[] array = (from c in Assembly.LoadFrom(lib).GetExportedTypes()
                                where !c.IsAbstract && c.IsClass
                                select c).ToArray();
                TypeFilter filter = InterfaceFilter;
                List<Type> list = new List<Type>();
                Type[] array2 = array;
                foreach (Type type in array2)
                {
                    if (type.FindInterfaces(filter, base.basics.InterfaceFullName).Count() > 0)
                    {
                        list.Add(type);
                    }
                }
                foreach (Type item in list)
                {
                    TypeIoc.TryAdd(item.FullName, item);
                    OnInfoEventHandler(this, new EventInfoResult(status: true, item.FullName + " 程序集添加成功"));
                }
            }
            catch (Exception)
            {
                Task.Delay(1).Wait();
                SearchType(lib);
            }
        }

        private void TypeSearchConfig(Type type)
        {
            string searchPattern = string.Format(base.basics.ConfigFileNameFormat, type.FullName);
            List<FileInfo> list = new DirectoryInfo(base.basics.LibConfigFolder).GetFiles(searchPattern, SearchOption.AllDirectories).ToList();
            if (list.Count <= 0)
            {
                return;
            }
            foreach (FileInfo item in list)
            {
                ConfigCreateInstance(type, FileHandler.FileToString(item.FullName));
            }
        }

        private async Task OnFailTaskHandler(CancellationToken token)
        {
            try
            {
                await Task.Run(async delegate
                {
                    List<string> OnSucceedSN = new List<string>();
                    while (!token.IsCancellationRequested)
                    {
                        foreach (KeyValuePair<string, IMq> item in OnFailIoc)
                        {
                            if (item.Value.On().Status)
                            {
                                OnSucceedSN.Add(item.Key);
                            }
                        }
                        foreach (string item2 in OnSucceedSN)
                        {
                            OnFailIoc.Remove(item2, out var _);
                        }
                        OnSucceedSN.Clear();
                        await Task.Delay(OnFailTaskHandlerInterval);
                    }
                }, token);
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private void ConfigCreateInstance(Type type, string content)
        {
            JObject jObject = JsonConvert.DeserializeObject<JObject>(content);
            string text = $"{type.FullName}.{jObject[base.basics.LibConfigSNKey]}";
            IMq instance = CreateInstance(type, new object[1] { jObject }) as IMq;
            if (instance != null)
            {
                InstanceIoc.TryAdd(text, instance);
                if (base.basics.AutoOn)
                {
                    OperateResult operateResult = instance.On();
                    if (!operateResult.Status)
                    {
                        OnFailIoc.AddOrUpdate(text, instance, (string k, IMq v) => instance);
                    }
                    OnInfoEventHandler(this, new EventInfoResult(operateResult.Status, text + " 实例创建成功，自动打开" + (operateResult.Status ? "成功" : "失败")));
                }
                else
                {
                    OnInfoEventHandler(this, new EventInfoResult(status: true, text + " 实例创建成功"));
                }
            }
            else
            {
                OnInfoEventHandler(this, new EventInfoResult(status: false, text + " 实例创建失败"));
            }
        }

        private object? CreateInstance(Type? NamespaceAndClassNameType, object[]? ConstructorParam)
        {
            object[] args = null;
            if (ConstructorParam != null)
            {
                ConstructorInfo info = (from c in NamespaceAndClassNameType?.GetConstructors()
                                        where c.GetParameters().Count() > 0
                                        select c).FirstOrDefault();
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
                                JObject jObject = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Data[j]));
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

        private bool InterfaceFilter(Type typeObj, object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
            {
                return true;
            }
            return false;
        }

        private async Task TaskHandle(CancellationToken token)
        {
            try
            {
                await Task.Run(async delegate
                {
                    while (await DataQueue.Reader.WaitToReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
                    {
                        QueueData item;
                        while (DataQueue.Reader.TryRead(out item))
                        {
                            if (item != null)
                            {
                                if (item.ISns == null || item.ISns.Count <= 0)
                                {
                                    foreach (KeyValuePair<string, IMq> item2 in InstanceIoc)
                                    {
                                        if (item2.Value.GetStatus().Status)
                                        {
                                            OperateResult operateResult = null;
                                            if (item.Content is byte[])
                                            {
                                                operateResult = item2.Value.Produce(item.Topic, item.Content.GetSource<byte[]>());
                                            }
                                            else if (item.Content is string)
                                            {
                                                operateResult = item2.Value.Produce(item.Topic, item.Content.GetSource<string>());
                                            }
                                            OnInfoEventHandler(this, new EventInfoResult(operateResult.Status, operateResult.Message));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (string iSn in item.ISns)
                                    {
                                        if (InstanceIoc.ContainsKey(iSn) && InstanceIoc[iSn].GetStatus().Status)
                                        {
                                            OperateResult operateResult2 = null;
                                            if (item.Content is byte[])
                                            {
                                                operateResult2 = InstanceIoc[iSn].Produce(item.Topic, item.Content.GetSource<byte[]>());
                                            }
                                            else if (item.Content is string)
                                            {
                                                operateResult2 = InstanceIoc[iSn].Produce(item.Topic, item.Content.GetSource<string>());
                                            }
                                            OnInfoEventHandler(this, new EventInfoResult(operateResult2.Status, operateResult2.Message));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }, token);
            }
            catch (TaskCanceledException)
            {
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        public override void Dispose()
        {
            if (OnFailToken != null)
            {
                OnFailToken.Cancel();
                OnFailToken.Dispose();
                OnFailToken = null;
            }
            if (WatcherToken != null)
            {
                WatcherToken.Cancel();
                WatcherToken.Dispose();
                WatcherToken = null;
            }
            if (WatcherQueue != null)
            {
                WatcherQueue.Writer.Complete();
                WatcherData item;
                while (WatcherQueue.Reader.TryRead(out item))
                {
                }
                WatcherQueue = null;
            }
            if (TaskArray != null)
            {
                foreach (KeyValuePair<CancellationTokenSource, Task> item3 in TaskArray)
                {
                    item3.Key.Cancel();
                }
                TaskArray.Clear();
                TaskArray = null;
            }
            foreach (KeyValuePair<string, IMq> item4 in InstanceIoc)
            {
                item4.Value.Dispose();
            }
            if (DataQueue != null)
            {
                DataQueue.Writer.Complete();
                QueueData item2;
                while (DataQueue.Reader.TryRead(out item2))
                {
                }
                DataQueue = null;
            }
            if (InstanceIoc != null)
            {
                InstanceIoc.Clear();
            }
            if (TypeIoc != null)
            {
                TypeIoc.Clear();
                TypeIoc = null;
            }
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            Dispose();
            await base.DisposeAsync();
        }

        public OperateResult Dispose(string ISn)
        {
            BegOperate("Dispose");
            try
            {
                if (InstanceIoc.ContainsKey(ISn))
                {
                    OperateResult operateResult = Remove(new List<string> { ISn });
                    return EndOperate(operateResult.Status, operateResult.Message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Dispose", 840);
                }
                return EndOperate(status: false, "未找到 " + ISn + " 的实例", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Dispose", 844);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Dispose", 849);
            }
        }

        public List<string>? TypeSns()
        {
            if (TypeIoc != null)
            {
                return TypeIoc.Keys.ToList();
            }
            return null;
        }

        public List<string>? InstanceSns()
        {
            if (InstanceIoc != null)
            {
                return InstanceIoc.Keys.ToList();
            }
            return null;
        }

        public OperateResult Remove(List<string>? ISns = null)
        {
            BegOperate("Remove");
            try
            {
                try
                {
                    List<string> list = new List<string>();
                    if (ISns != null)
                    {
                        foreach (string ISn in ISns)
                        {
                            if (InstanceIoc.ContainsKey(ISn))
                            {
                                if (InstanceIoc.Remove(ISn, out var value))
                                {
                                    value?.Dispose();
                                }
                                else
                                {
                                    list.Add(ISn + " 的实例移除失败");
                                }
                            }
                            else
                            {
                                list.Add("未找到 " + ISn + " 的实例");
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, IMq> item in InstanceIoc)
                        {
                            item.Value.Dispose();
                        }
                        InstanceIoc.Clear();
                    }
                    if (list.Count > 0)
                    {
                        return EndOperate(status: false, $"存在 {list.Count} 失败信息", list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Remove", 929);
                    }
                    return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Remove", 933);
                }
                catch (Exception ex)
                {
                    return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Remove", 938);
                }
            }
            catch (Exception ex2)
            {
                return EndOperate(status: false, ex2.Message, null, ex2, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Remove", 943);
            }
        }

        public OperateResult On(List<string>? ISns = null)
        {
            BegOperate("On");
            try
            {
                List<string> list = new List<string>();
                if (ISns != null)
                {
                    foreach (string ISn in ISns)
                    {
                        if (InstanceIoc.ContainsKey(ISn))
                        {
                            OperateResult operateResult = InstanceIoc[ISn].On();
                            if (!operateResult.Status)
                            {
                                list.Add(operateResult.Message);
                            }
                        }
                        else
                        {
                            list.Add("未找到 " + ISn + " 的实例");
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, IMq> item in InstanceIoc)
                    {
                        OperateResult operateResult2 = item.Value.On();
                        if (!operateResult2.Status)
                        {
                            list.Add(operateResult2.Message);
                        }
                    }
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息", list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "On", 989);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "On", 993);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "On", 998);
            }
        }

        public OperateResult Off(List<string>? ISns = null)
        {
            BegOperate("Off");
            try
            {
                List<string> list = new List<string>();
                if (ISns != null)
                {
                    foreach (string ISn in ISns)
                    {
                        if (InstanceIoc.ContainsKey(ISn))
                        {
                            OperateResult operateResult = InstanceIoc[ISn].Off();
                            if (!operateResult.Status)
                            {
                                list.Add(operateResult.Message);
                            }
                        }
                        else
                        {
                            list.Add("未找到 " + ISn + " 的实例");
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, IMq> item in InstanceIoc)
                    {
                        OperateResult operateResult2 = item.Value.Off();
                        if (!operateResult2.Status)
                        {
                            list.Add(operateResult2.Message);
                        }
                    }
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息", list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Off", 1045);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Off", 1049);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Off", 1055);
            }
        }

        public OperateResult Produce(string Topic, string Content, List<string>? ISns = null)
        {
            BegOperate("Produce");
            try
            {
                if (DataQueue == null)
                {
                    DataQueue = Channel.CreateBounded<QueueData>(channelOptions);
                }
                if (TaskArray == null)
                {
                    TaskArray = new ConcurrentDictionary<CancellationTokenSource, Task>();
                    for (int i = 0; i < base.basics.TaskNumber; i++)
                    {
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                        TaskArray.TryAdd(cancellationTokenSource, TaskHandle(cancellationTokenSource.Token));
                    }
                }
                List<string> list = new List<string>();
                if (ISns == null || ISns.Count <= 0)
                {
                    DataQueue?.Writer.WriteAsync(new QueueData
                    {
                        Topic = Topic,
                        Content = Content,
                        ISns = ISns
                    }).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    for (int j = 0; j < ISns.Count; j++)
                    {
                        if (!InstanceIoc.ContainsKey(ISns[j]))
                        {
                            list.Add(ISns[j] + " 实例未找到");
                        }
                    }
                    DataQueue?.Writer.WriteAsync(new QueueData
                    {
                        Topic = Topic,
                        Content = Content,
                        ISns = ISns
                    }).ConfigureAwait(continueOnCapturedContext: false);
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息，{list.ToJson()}", list, null, logOutput: true, consoleOutput: false, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1108);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1112);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1117);
            }
        }

        public OperateResult Produce(string Topic, byte[] Content, List<string>? ISns = null)
        {
            BegOperate("Produce");
            try
            {
                if (DataQueue == null)
                {
                    DataQueue = Channel.CreateBounded<QueueData>(channelOptions);
                }
                if (TaskArray == null)
                {
                    TaskArray = new ConcurrentDictionary<CancellationTokenSource, Task>();
                    for (int i = 0; i < base.basics.TaskNumber; i++)
                    {
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                        TaskArray.TryAdd(cancellationTokenSource, TaskHandle(cancellationTokenSource.Token));
                    }
                }
                List<string> list = new List<string>();
                if (ISns == null || ISns.Count <= 0)
                {
                    DataQueue?.Writer.WriteAsync(new QueueData
                    {
                        Topic = Topic,
                        Content = Content,
                        ISns = ISns
                    }).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    for (int j = 0; j < ISns.Count; j++)
                    {
                        if (!InstanceIoc.ContainsKey(ISns[j]))
                        {
                            list.Add(ISns[j] + " 实例未找到");
                        }
                    }
                    DataQueue?.Writer.WriteAsync(new QueueData
                    {
                        Topic = Topic,
                        Content = Content,
                        ISns = ISns
                    }).ConfigureAwait(continueOnCapturedContext: false);
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息，{list.ToJson()}", list, null, logOutput: true, consoleOutput: false, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1172);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1176);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Produce", 1181);
            }
        }

        public OperateResult Consume(string Topic, List<string>? ISns = null)
        {
            BegOperate("Consume");
            try
            {
                List<string> list = new List<string>();
                if (ISns != null)
                {
                    foreach (string ISn in ISns)
                    {
                        if (InstanceIoc.ContainsKey(ISn))
                        {
                            if (!InstanceIoc[ISn].Consume(Topic).GetDetails(out string message))
                            {
                                list.Add(message);
                            }
                        }
                        else
                        {
                            list.Add("未找到 " + ISn + " 的实例");
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, IMq> item in InstanceIoc)
                    {
                        if (!item.Value.Consume(Topic).GetDetails(out string message2))
                        {
                            list.Add(message2);
                        }
                    }
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息", list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Consume", 1226);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Consume", 1230);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "Consume", 1235);
            }
        }

        public OperateResult UnConsume(string Topic, List<string>? ISns = null)
        {
            BegOperate("UnConsume");
            try
            {
                List<string> list = new List<string>();
                if (ISns != null)
                {
                    foreach (string ISn in ISns)
                    {
                        if (InstanceIoc.ContainsKey(ISn))
                        {
                            if (!InstanceIoc[ISn].UnConsume(Topic).GetDetails(out string message))
                            {
                                list.Add(message);
                            }
                        }
                        else
                        {
                            list.Add("未找到 " + ISn + " 的实例");
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, IMq> item in InstanceIoc)
                    {
                        if (!item.Value.UnConsume(Topic).GetDetails(out string message2))
                        {
                            list.Add(message2);
                        }
                    }
                }
                if (list.Count > 0)
                {
                    return EndOperate(status: false, $"存在 {list.Count} 失败信息", list, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "UnConsume", 1280);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "UnConsume", 1284);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\mq\\MqOperate.cs", "UnConsume", 1289);
            }
        }
    }
}
