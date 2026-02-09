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
    public class CommunicationClientTemplateViewModel<T> : BindNotify
    {
        public ICommunication Communication { get; set; }

       // public UdpOperate CommunicationUDP { get; set; }

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

        public Visibility Visibility
        {
            get
            {
                return GetProperty(() => Visibility);
            }
            set
            {
                SetProperty(() => Visibility, value);
            }
        }

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

        public IAsyncRelayCommand InfoEventTextChanged => new AsyncRelayCommand<TextChangedEventArgs>(InfoEventTextChangedAsync);

        public IAsyncRelayCommand On => new AsyncRelayCommand(OnAsync);

        public IAsyncRelayCommand Off => new AsyncRelayCommand(OffAsync);

        public IAsyncRelayCommand Send => new AsyncRelayCommand(SendAsync);

        public IAsyncRelayCommand Inc => new AsyncRelayCommand(IncAsync);

        public IAsyncRelayCommand Exp => new AsyncRelayCommand(ExpAsync);

        public IAsyncRelayCommand InfoClear => new AsyncRelayCommand(InfoClearAsync);

        public CommunicationClientTemplateViewModel()
        {
            LanguageHandler.OnLanguageEventAsync -= LanguageHandler_OnLanguageEventAsync;
            LanguageHandler.OnLanguageEventAsync += LanguageHandler_OnLanguageEventAsync;
            Application.Current.Dispatcher.InvokeAsync((Func<Task>)async delegate
            {
                if (Key == "Udp")
                {
                    Visibility = Visibility.Visible;
                    IpAddress = "127.0.0.1";
                    Port = 8866;
                }
                else
                {
                    Visibility = Visibility.Collapsed;
                }
                ToolTitle = await GetTitleAsync();
            }, DispatcherPriority.Loaded);
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
            //if (Key == "Udp")
            //{
            //    UdpData.TerminalMessage message = e.GetSource<UdpData.TerminalMessage>();
            //    if (message != null && message.Bytes != null)
            //    {
            //        if (DataFormat == 0)
            //        {
            //            await InfoEventLogShow("[ " + message.IpPort + " ] -> " + Encoding.ASCII.GetString(message.Bytes), isDateTime: false);
            //        }
            //        else
            //        {
            //            await InfoEventLogShow("[ " + message.IpPort + " ] -> " + message.Bytes.ToHexString(), isDateTime: false);
            //        }
            //    }
            //    return;
            //}
            _ = string.Empty;
            dynamic person = BasicsData;
            string addr = ((Key == "WsClient") ? $"{(object?)person.Host}" : ((!(Key == "TcpClient")) ? $"{(object?)person.PortName}" : $"{(object?)person.IpAddress}:{(object?)person.Port}"));
            await InfoEventLogShow(e.Message.Replace("[", "[ ").Replace("]", " ] "));
            if (e.ResultData != null && e.ResultData is byte[])
            {
                if (DataFormat == 0)
                {
                    await InfoEventLogShow("[ " + addr + " ] -> " + Encoding.ASCII.GetString(e.GetSource<byte[]>()), isDateTime: false);
                }
                else
                {
                    await InfoEventLogShow("[ " + addr + " ] -> " + e.GetSource<byte[]>().ToHexString(), isDateTime: false);
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
            }
        }

        public async Task SendAsync()
        {
            byte[] sendData = null;
            if (DataFormat.Equals(0))
            {
                sendData = Encoding.ASCII.GetBytes(Data);
            }
            else if (Data.IsHexadecimal())
            {
                sendData = Data.ToHex(strict: false);
            }
            else
            {
                InfoEventLogShow("“" + Data + "”" + App.LanguageOperate.GetLanguageValue("不是有效的 Hex 数据"));
            }
            if (Key == "Udp")
            {
                if (string.IsNullOrWhiteSpace(IpAddress) || Port == 0)
                {
                    await InfoEventLogShow((await Communication.SendAsync(sendData)).ToJson(formatting: true));
                    return;
                }
                //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
                //await InfoEventLogShow((await CommunicationUDP.SendAsync(sendData, iPEndPoint)).ToJson(formatting: true));
            }
            else
            {
                await InfoEventLogShow((await Communication.SendAsync(sendData)).ToJson(formatting: true));
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
