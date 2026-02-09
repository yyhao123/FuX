using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Controls.handler;
using Demo.Windows.Core.mvvm;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.@interface;
using FuX.Sim;
using FuX.Unility;
using FuX.Core.handler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using FuX.Core.extend;
using FuX.Model.Specenum;

namespace TestTool.tc261
{
    public class SimViewModel : BindNotify
    {
        private Visibility _getAutoAllocatingParamVisibility;

        public AddressType AddressType
        {
            get
            {
                return GetProperty(() => AddressType);
            }
            set
            {
                SetProperty(() => AddressType, value);
            }
        }

        public IDaq Daq { get; set; }

        public string FileName { get; set; }

        public string Key { get; set; }

        public string ToolTitle
        {
            get
            {
                return GetProperty(() => ToolTitle);
            }
            set
            {
                SetProperty(() => ToolTitle, value);
            }
        }

        public SimData.Basics BasicsData
        {
            get
            {
                return GetProperty(() => BasicsData);
            }
            set
            {
                SetProperty(() => BasicsData, value);
            }
        }

        public DataType DataType
        {
            get
            {
                return GetProperty(() => DataType);
            }
            set
            {
                SetProperty(() => DataType, value);
            }
        }

        public string Address
        {
            get
            {
                return GetProperty(() => Address);
            }
            set
            {
                SetProperty(() => Address, value);
            }
        }

        public string Data
        {
            get
            {
                return GetProperty(() => Data);
            }
            set
            {
                SetProperty(() => Data, value);
            }
        }

        public string InfoEvent
        {
            get
            {
                return GetProperty(() => InfoEvent);
            }
            set
            {
                SetProperty(() => InfoEvent, value);
            }
        }

        public string DataEvent
        {
            get
            {
                return GetProperty(() => DataEvent);
            }
            set
            {
                SetProperty(() => DataEvent, value);
            }
        }

        public Visibility GetAutoAllocatingParamVisibility
        {
            get
            {
                return _getAutoAllocatingParamVisibility;
            }
            set
            {
                SetProperty(ref _getAutoAllocatingParamVisibility, value, "GetAutoAllocatingParamVisibility");
            }
        }

        public IAsyncRelayCommand DataEventTextChanged => new AsyncRelayCommand<TextChangedEventArgs>(DataEventTextChangedAsync);

        public IAsyncRelayCommand InfoEventTextChanged => new AsyncRelayCommand<TextChangedEventArgs>(InfoEventTextChangedAsync);

        public IAsyncRelayCommand On => new AsyncRelayCommand(OnAsync);

        public IAsyncRelayCommand Off => new AsyncRelayCommand(OffAsync);

        public IAsyncRelayCommand Read => new AsyncRelayCommand(ReadAsync);

        public IAsyncRelayCommand Write => new AsyncRelayCommand(WriteAsync);

        public IAsyncRelayCommand Subscribe => new AsyncRelayCommand(SubscribeAsync);

        public IAsyncRelayCommand UnSubscribe => new AsyncRelayCommand(UnSubscribeAsync);

        public IAsyncRelayCommand GetAutoAllocatingParam => new AsyncRelayCommand(GetAutoAllocatingParamAsync);

        public IAsyncRelayCommand Inc => new AsyncRelayCommand(IncAsync);

        public IAsyncRelayCommand Exp => new AsyncRelayCommand(ExpAsync);

        public IAsyncRelayCommand InfoClear => new AsyncRelayCommand(InfoClearAsync);

        public IAsyncRelayCommand DataClear => new AsyncRelayCommand(DataClearAsync);

        public SimViewModel()
        {
            LanguageHandler.OnLanguageEventAsync -= LanguageHandler_OnLanguageEventAsync;
            LanguageHandler.OnLanguageEventAsync += LanguageHandler_OnLanguageEventAsync;
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.InvokeAsync((Func<Task>)async delegate
                {
                    BasicsData = new SimData.Basics();
                    FileName = typeof(SimData).Name;
                    Daq = CoreUnify<SimOperate, SimData.Basics>.Instance(BasicsData);
                    GetAutoAllocatingParamVisibility = ((!Daq.ExistsAutoAllocatingParam().Status) ? Visibility.Collapsed : Visibility.Visible);
                    Key = "Sim";
                    DataType = DataType.Double;
                    AddressType = AddressType.VirtualStatic;
                    //ToolTitle = await GetTitleAsync();
                }, DispatcherPriority.Loaded);
            }
        }

        private async Task LanguageHandler_OnLanguageEventAsync(object? sender, EventLanguageResult e)
        {
            ToolTitle = await GetTitleAsync(e.Language);
        }

        public Task DataEventTextChangedAsync(TextChangedEventArgs? e)
        {
            TextBox source = e.Source.GetSource<TextBox>();
            source.SelectionStart = source.Text.Length;
            source.SelectionLength = 0;
            source.ScrollToEnd();
            return Task.CompletedTask;
        }

        public async Task DataEventLogShow(string? msg, bool isDateTime = true)
        {
            string msg2 = msg;
            if (msg2.IsNullOrWhiteSpace() || Application.Current == null)
            {
                return;
            }
            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                string dataEvent = DataEvent;
                if (dataEvent != null && dataEvent.Length > 10000)
                {
                    DataEvent = string.Empty;
                }
                if (isDateTime)
                {
                    DataEvent += $" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} : {msg2}\r\n";
                }
                else
                {
                    DataEvent = DataEvent + msg2 + "\r\n";
                }
            });
        }

        public Task InfoEventTextChangedAsync(TextChangedEventArgs? e)
        {
            TextBox source = e.Source.GetSource<TextBox>();
            source.SelectionStart = source.Text.Length;
            source.SelectionLength = 0;
            source.ScrollToEnd();
            return Task.CompletedTask;
        }

        public async Task InfoEventLogShow(string? msg, bool isDateTime = true)
        {
            string msg2 = msg;
            if (msg2.IsNullOrWhiteSpace() || Application.Current == null)
            {
                return;
            }
            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                string infoEvent = InfoEvent;
                if (infoEvent != null && infoEvent.Length > 10000)
                {
                    InfoEvent = string.Empty;
                }
                if (isDateTime)
                {
                    InfoEvent += $" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} : {msg2}\r\n";
                }
                else
                {
                    InfoEvent = InfoEvent + msg2 + "\r\n";
                }
            });
        }

        private async Task Daq_OnDataEventAsync(object? sender, EventDataResult e)
        {
            if (e.GetDetails(out string message, out ConcurrentDictionary<string, AddressValue> data))
            {
                foreach (KeyValuePair<string, AddressValue> item in data)
                {
                    if (item.Value.AddressDataType == FuX.Model.Specenum.DataType.ByteArray)
                    {
                        await DataEventLogShow($"{e.Message}\r\n键：{item.Key}\r\n值：{ByteHandler.ByteToHexString(item.Value.ResultValue.GetSource<byte[]>(), ' ')}\r\n消息：{item.Value.Message}\r\n", isDateTime: false);
                    }
                    else if (item.Value.AddressDataType.ToString().Contains("Array"))
                    {
                        await DataEventLogShow($"{e.Message}\r\n键：{item.Key}\r\n值：{item.Value.ResultValue.ToJson()}\r\n消息：{item.Value.Message}\r\n", isDateTime: false);
                    }
                    else
                    {
                        await DataEventLogShow($"{e.Message}\r\n键：{item.Key}\r\n值：{item.Value.ResultValue}\r\n消息：{item.Value.Message}\r\n", isDateTime: false);
                    }
                }
            }
            else
            {
                if (!e.GetDetails(out message, out List<ConcurrentDictionary<string, AddressValue>> datas))
                {
                    return;
                }
                foreach (ConcurrentDictionary<string, AddressValue> items in datas)
                {
                    foreach (KeyValuePair<string, AddressValue> item2 in items)
                    {
                        if (item2.Value.AddressDataType == FuX.Model.Specenum.DataType.ByteArray)
                        {
                            await DataEventLogShow($"{e.Message}\r\n键：{item2.Key}\r\n值：{ByteHandler.ByteToHexString(item2.Value.ResultValue.GetSource<byte[]>())}\r\n消息：{item2.Value.Message}\r\n", isDateTime: false);
                        }
                        else if (item2.Value.AddressDataType.ToString().Contains("Array"))
                        {
                            await DataEventLogShow($"{e.Message}\r\n键：{item2.Key}\r\n值：{item2.Value.ResultValue.ToJson()}\r\n消息：{item2.Value.Message}\r\n", isDateTime: false);
                        }
                        else
                        {
                            await DataEventLogShow($"{e.Message}\r\n键：{item2.Key}\r\n值：{item2.Value.ResultValue}\r\n消息：{item2.Value.Message}\r\n", isDateTime: false);
                        }
                    }
                }
            }
        }

        private async Task Daq_OnInfoEventAsync(object? sender, EventInfoResult e)
        {
            await InfoEventLogShow(e.ToJson(formatting: true));
        }

        public async Task OnAsync()
        {
            OperateResult result = await Daq.OnAsync();
            await InfoEventLogShow(result.ToJson(formatting: true));
            if (result.Status)
            {
                Daq.OnInfoEventAsync -= Daq_OnInfoEventAsync;
                Daq.OnInfoEventAsync += Daq_OnInfoEventAsync;
                Daq.OnDataEventAsync -= Daq_OnDataEventAsync;
                Daq.OnDataEventAsync += Daq_OnDataEventAsync;
            }
        }

        public async Task OffAsync()
        {
            OperateResult result = await Daq.OffAsync();
            await InfoEventLogShow(result.ToJson(formatting: true));
            if (result.Status)
            {
                Daq.OnInfoEventAsync -= Daq_OnInfoEventAsync;
                Daq.OnDataEventAsync -= Daq_OnDataEventAsync;
            }
        }

        public async Task ReadAsync()
        {
            await InfoEventLogShow((await Daq.ReadAsync(OrganizationAddress())).ToJson(formatting: true));
        }

        public async Task WriteAsync()
        {
            ConcurrentDictionary<string, WriteModel> pairs = new ConcurrentDictionary<string, WriteModel>();
            DataType dataType = (DataType)DataType;
            pairs.TryAdd(Address, new WriteModel(Data, dataType, EncodingType.ANSI));
            await InfoEventLogShow((await Daq.WriteAsync(pairs)).ToJson(formatting: true));
        }

        public async Task SubscribeAsync()
        {
            await InfoEventLogShow((await Daq.SubscribeAsync(OrganizationAddress())).ToJson(formatting: true));
        }

        public async Task UnSubscribeAsync()
        {
            await InfoEventLogShow((await Daq.UnSubscribeAsync(OrganizationAddress())).ToJson(formatting: true));
        }

        public async Task GetAutoAllocatingParamAsync()
        {
            if ((await Daq.GetAutoAllocatingParamAsync()).GetDetails(out string message, out object rData))
            {
                await InfoEventLogShow(rData.ToJson(formatting: true));
            }
            else
            {
                await InfoEventLogShow(message);
            }
        }

        public async Task IncAsync()
        {
            string file = SelectFiles("json");
            if (!string.IsNullOrEmpty(file))
            {
                SimData.Basics basics = FileHandler.FileToString(file).ToJsonEntity<SimData.Basics>();
                if (basics == null)
                {
                    await InfoEventLogShow(App.LanguageOperate.GetLanguageValue("导入失败"));
                    return;
                }
                await InfoEventLogShow(App.LanguageOperate.GetLanguageValue("导入成功"));
                BasicsData = basics;
            }
        }

        public async Task ExpAsync()
        {
            string path = SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                FileHandler.StringToFile(Path.Combine(path, FileName + "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "].json"), BasicsData.ToJson());
                await InfoEventLogShow(App.LanguageOperate.GetLanguageValue("导出成功"));
            }
        }

        public Task InfoClearAsync()
        {
            InfoEvent = string.Empty;
            return Task.CompletedTask;
        }

        public Task DataClearAsync()
        {
            DataEvent = string.Empty;
            return Task.CompletedTask;
        }

        public async Task<string> GetTitleAsync(LanguageType? language = null)
        {
            if (!language.HasValue)
            {
                language = LanguageHandler.GetLanguage();
            }
            string name = string.Empty;
            switch (language)
            {
                case LanguageType.zh:
                    name = "{0}调试工具";
                    break;
                case LanguageType.en:
                    name = "{0}DebugTool";
                    break;
            }
            return string.Format(name, (await App.LanguageOperate.GetLanguageValueAsync(Key)).Replace(" ", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty));
        }

        private Address OrganizationAddress()
        {
            DataType dataType = (DataType)DataType;
            return new Address
            {
                AddressArray = new List<AddressDetails>
            {
                new AddressDetails
                {
                    AddressName = Address,
                    AddressDataType = dataType,
                    EncodingType = EncodingType.ANSI,
                    AddressType = (AddressType)AddressType
                }
            }
            };
        }

        public string SelectFiles(string fileExt)
        {
            Dictionary<string, string> filters = new Dictionary<string, string> {
        {
            "(*." + fileExt + ")",
            "*." + fileExt
        } };
            return Win32Handler.Select(App.LanguageOperate.GetLanguageValue("请选择文件"), selectFolder: false, filters);
        }

        public static string SelectFolder()
        {
            return Win32Handler.Select(App.LanguageOperate.GetLanguageValue("请选择文件夹"), selectFolder: true);
        }
    }
}
