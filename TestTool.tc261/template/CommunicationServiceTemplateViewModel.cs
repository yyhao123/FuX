using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Controls.handler;
using Demo.Windows.Core.mvvm;
using FuX.Unility;
using FuX.Core.handler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using FuX.Model.data;
using System.Windows;
using FuX.Model.@enum;
using System.IO;
using FuX.Core.Communication.net.tcp.service;
using FuX.Core.Communication.net.ws.service;
using FuX.Model.@interface;

namespace TestTool.tc261.template
{
    public class CommunicationServiceTemplateViewModel<T> : BindNotify
    {
        public class DataGridStructuralBody : BindNotify
        {
            public string IpAddress
            {
                get
                {
                    return GetProperty(() => IpAddress);
                }
                set
                {
                    SetProperty(() => IpAddress, value);
                }
            }

            public int Port
            {
                get
                {
                    return GetProperty(() => Port);
                }
                set
                {
                    SetProperty(() => Port, value);
                }
            }

            public string IPENDPORT { get; set; }
        }

        private ObservableCollection<DataGridStructuralBody> dataGridItemsSource = new ObservableCollection<DataGridStructuralBody>();

        public DataGridStructuralBody DataGridSelectionItem = new DataGridStructuralBody();

        public ICommunicationService Communication { get; set; }

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

        public T BasicsData
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

        public int DataFormat
        {
            get
            {
                return GetProperty(() => DataFormat);
            }
            set
            {
                SetProperty(() => DataFormat, value);
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

        public bool MassSend
        {
            get
            {
                return GetProperty(() => MassSend);
            }
            set
            {
                SetProperty(() => MassSend, value);
            }
        }

        public ObservableCollection<DataGridStructuralBody> DataGridItemsSource
        {
            get
            {
                return dataGridItemsSource;
            }
            set
            {
                SetProperty(ref dataGridItemsSource, value, "DataGridItemsSource");
            }
        }

        public IAsyncRelayCommand GridDataSelectionChanged => new AsyncRelayCommand<SelectionChangedEventArgs>(GridDataSelectionChangedAsync);

        public IAsyncRelayCommand InfoEventTextChanged => new AsyncRelayCommand<TextChangedEventArgs>(InfoEventTextChangedAsync);

        public IAsyncRelayCommand On => new AsyncRelayCommand(OnAsync);

        public IAsyncRelayCommand Off => new AsyncRelayCommand(OffAsync);

        public IAsyncRelayCommand Send => new AsyncRelayCommand(SendAsync);

        public IAsyncRelayCommand Inc => new AsyncRelayCommand(IncAsync);

        public IAsyncRelayCommand Exp => new AsyncRelayCommand(ExpAsync);

        public IAsyncRelayCommand InfoClear => new AsyncRelayCommand(InfoClearAsync);

        public CommunicationServiceTemplateViewModel()
        {
            LanguageHandler.OnLanguageEventAsync -= LanguageHandler_OnLanguageEventAsync;
            LanguageHandler.OnLanguageEventAsync += LanguageHandler_OnLanguageEventAsync;
            Application.Current.Dispatcher.InvokeAsync((Func<Task>)async delegate
            {
                ToolTitle = await GetTitleAsync();
            }, DispatcherPriority.Loaded);
        }

        private Task GridDataSelectionChangedAsync(SelectionChangedEventArgs? e)
        {
            DataGridSelectionItem = e.Source.GetSource<DataGrid>().SelectedItem.GetSource<DataGridStructuralBody>();
            return Task.CompletedTask;
        }

        private async Task LanguageHandler_OnLanguageEventAsync(object? sender, EventLanguageResult e)
        {
            ToolTitle = await GetTitleAsync(e.Language);
        }

        public Task InfoEventTextChangedAsync(TextChangedEventArgs? e)
        {
            TextBox? source = e.Source.GetSource<TextBox>();
            source.SelectionStart = source.Text.Length;
            source.SelectionLength = 0;
            source.ScrollToEnd();
            return Task.CompletedTask;
        }

        public Task InfoEventLogShow(string? msg, bool isDateTime = true)
        {
            if (msg.IsNullOrWhiteSpace())
            {
                return Task.CompletedTask;
            }
            return Application.Current.Dispatcher.InvokeAsync(delegate
            {
                string infoEvent = InfoEvent;
                if (infoEvent != null && infoEvent.Length > 10000)
                {
                    InfoEvent = string.Empty;
                }
                if (isDateTime)
                {
                    InfoEvent += $" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} : {msg}\r\n";
                }
                else
                {
                    InfoEvent = InfoEvent + msg + "\r\n";
                }
                return Task.CompletedTask;
            }).Result;
        }

        private async Task Communication_OnDataEventAsync(object? sender, EventDataResult e)
        {
            await InfoEventLogShow(e.Message.Replace("[", "[ ").Replace("]", " ] "));
            if (Key == "TcpService")
            {
                TcpServiceData.ClientMessage message = e.GetSource<TcpServiceData.ClientMessage>();
                if (message == null)
                {
                    return;
                }
                switch (message.Step)
                {
                    case TcpServiceData.Steps.客户端连接:
                        {
                            string[] ipport = message.IpPort.Split(':');
                            Application.Current?.Dispatcher.InvokeAsync(delegate
                            {
                                DataGridItemsSource.Add(new DataGridStructuralBody
                                {
                                    IpAddress = ipport[0],
                                    Port = ipport[1].ToInt(),
                                    IPENDPORT = message.IpPort
                                });
                            });
                            break;
                        }
                    case TcpServiceData.Steps.客户端断开:
                        Application.Current?.Dispatcher.InvokeAsync(delegate
                        {
                            for (int i = 0; i < DataGridItemsSource.Count; i++)
                            {
                                if (DataGridItemsSource[i].IPENDPORT.Equals(message.IpPort))
                                {
                                    DataGridItemsSource.RemoveAt(i);
                                }
                            }
                        });
                        break;
                    case TcpServiceData.Steps.消息接收:
                        if (message.Bytes != null)
                        {
                            if (DataFormat == 0)
                            {
                                await InfoEventLogShow("[ " + message.IpPort + " ] -> " + Encoding.ASCII.GetString(message.Bytes), isDateTime: false);
                            }
                            else
                            {
                                await InfoEventLogShow("[ " + message.IpPort + " ] -> " + message.Bytes.ToHexString(), isDateTime: false);
                            }
                        }
                        break;
                }
            }
            else
            {
                if (!(Key == "WsService"))
                {
                    return;
                }
                WsServiceData.ClientMessage message2 = e.GetSource<WsServiceData.ClientMessage>();
                if (message2 == null)
                {
                    return;
                }
                switch (message2.Step)
                {
                    case WsServiceData.Steps.客户端连接:
                        {
                            string[] ipport2 = message2.IpPort.Split(':');
                            Application.Current?.Dispatcher.InvokeAsync(delegate
                            {
                                DataGridItemsSource.Add(new DataGridStructuralBody
                                {
                                    IpAddress = ipport2[0],
                                    Port = ipport2[1].ToInt(),
                                    IPENDPORT = message2.IpPort
                                });
                            });
                            break;
                        }
                    case WsServiceData.Steps.客户端断开:
                        Application.Current?.Dispatcher.InvokeAsync(delegate
                        {
                            for (int i = 0; i < DataGridItemsSource.Count; i++)
                            {
                                if (DataGridItemsSource[i].IPENDPORT.Equals(message2.IpPort))
                                {
                                    DataGridItemsSource.RemoveAt(i);
                                }
                            }
                        });
                        break;
                    case WsServiceData.Steps.消息接收:
                        if (message2.Bytes != null)
                        {
                            if (DataFormat == 0)
                            {
                                await InfoEventLogShow("[ " + message2.IpPort + " ] -> " + Encoding.ASCII.GetString(message2.Bytes), isDateTime: false);
                            }
                            else
                            {
                                await InfoEventLogShow("[ " + message2.IpPort + " ] -> " + message2.Bytes.ToHexString(), isDateTime: false);
                            }
                        }
                        break;
                }
            }
        }

        private async Task Communication_OnInfoEventAsync(object? sender, EventInfoResult e)
        {
            await InfoEventLogShow(e.ToJson(formatting: true));
        }

        public async Task OnAsync()
        {
            OperateResult result = await Communication.OnAsync();
            await InfoEventLogShow(result.ToJson(formatting: true));
            if (result.Status)
            {
                Communication.OnInfoEventAsync -= Communication_OnInfoEventAsync;
                Communication.OnInfoEventAsync += Communication_OnInfoEventAsync;
                Communication.OnDataEventAsync -= Communication_OnDataEventAsync;
                Communication.OnDataEventAsync += Communication_OnDataEventAsync;
            }
        }

        public async Task OffAsync()
        {
            OperateResult result = await Communication.OffAsync();
            await InfoEventLogShow(result.ToJson(formatting: true));
            if (result.Status)
            {
                Communication.OnInfoEventAsync -= Communication_OnInfoEventAsync;
                Communication.OnDataEventAsync -= Communication_OnDataEventAsync;
                DataGridItemsSource.Clear();
            }
        }

        public async Task SendAsync()
        {
            byte[] sendData = null;
            if (DataFormat.Equals(0))
            {
                sendData = Encoding.ASCII.GetBytes(Data);
            }
            else if (!Data.IsHexadecimal())
            {
                await InfoEventLogShow("“" + Data + "”" + App.LanguageOperate.GetLanguageValue("不是有效的 Hex 数据"));
            }
            else
            {
                sendData = Data.ToHex(strict: false);
            }
            if (MassSend)
            {
                await InfoEventLogShow((await Communication.SendAsync(sendData)).ToJson(formatting: true));
            }
            else if (DataGridSelectionItem == null || string.IsNullOrEmpty(DataGridSelectionItem.IPENDPORT))
            {
                await InfoEventLogShow(App.LanguageOperate.GetLanguageValue("请选择一个已连接的客户端"));
            }
            else
            {
                await InfoEventLogShow((await Communication.SendAsync(sendData, DataGridSelectionItem.IPENDPORT)).ToJson(formatting: true));
            }
        }

        public async Task IncAsync()
        {
            string file = SelectFiles("json");
            if (!string.IsNullOrEmpty(file))
            {
                T basics = FileHandler.FileToString(file).ToJsonEntity<T>();
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
