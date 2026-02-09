using Demo.AutoTest.Core.handler;
using Demo.AutoTest.services;
using Demo.AutoTest.view.Module;
using Demo.AutoTest.view.userControls;
using Demo.AutoTest.view.userControls.bars;
using Demo.AutoTest.view.windows;
using Demo.AutoTest.viewModel.Module;
using Demo.AutoTest.viewModel.userControls;
using Demo.AutoTest.viewModel.userControls.bars;
using Demo.AutoTest.viewModel.windows;
using Demo.Driver.service;
using Demo.Windows.Core.handler;
using FuX.Core.db;
using FuX.Core.services;
using FuX.Log;
using FuX.Model.data;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFLocalizeExtension.Providers;

namespace Demo.AutoTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private ConnectedDeviceService service;

        /// <summary>
        /// 语言操作
        /// </summary>
        public readonly static LanguageModel LanguageOperate = new LanguageModel("Demo.AutoTest.Language", "Language", "Demo.AutoTest.Language.dll");

        /// <summary>
        /// 在应用程序关闭时发生
        /// </summary>
        private void OnExit(object sender, ExitEventArgs e)
        {
            InjectionWpf.ClearService();
        }

        /// <summary>
        /// 在加载应用程序时发生
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {

            //启动全局异常捕捉
            RegisterEvents();

          
            //初始化数据库
            DBOperate dbOperate = DBOperate.Instance(new DBData());
            dbOperate.On();

            if (InjectionWpf.AddService(s =>
            {
                
                //s.AddSingleton<IUserCfg, UserCfg>();
                s.AddSingleton<ILocalize,Localize>();
                s.AddSingleton<IAcquireModuleService, AcquireModuleService>();
                s.AddSingleton<ISpectrumService, SpectrumService>();
                s.AddSingleton<ISingleSpectrumService,SingleSpectrumService>();
                s.AddSingleton<IDeviceService, DeviceService>();         
                s.AddSingleton<IConnectedDeviceService, ConnectedDeviceService>((sp) =>
                {
                    if(service ==null)
                    {
                       service = new ConnectedDeviceService();
                       service.DeviceServiceCreator = InjectionWpf.GetService<IDeviceService>;
                    }
                    return service;
                });
                


            }))
            {
                //注入usercontrol
                
                InjectionWpf.UserControl<RightPram, RightPramViewModel>(true);
                InjectionWpf.UserControl<RightSpectrumListView, RightSpectrumListViewModel>(true);
                InjectionWpf.UserControl<subTopLeftView, TopLeftViewModel>(true);
                InjectionWpf.UserControl<subTopRightView, TopRightViewModel>(true);
                InjectionWpf.UserControl<subRightBottomView, RightBottomViewModel>(true);
                InjectionWpf.UserControl<subLeftBottomView, LeftBottomViewModel>(true);
                InjectionWpf.UserControl<BottomHistoryAreaView, BottomHistoryAreaViewModel>(true);
                InjectionWpf.UserControl<DevCmdInfoView, DevCmdInfoViewModel>(true);
                InjectionWpf.UserControl<MiddleGraphArea, MiddleGraphAreaViewModel>(true);
                //打开主窗口
                InjectionWpf.Window<MainView, MainViewModel>(true).Show();

               


            


            }
            else
            {
                throw new Exception("程序注入异常!!!");
            }
        }

        #region 全局异常捕捉

        /// <summary>
        /// 全局异常捕捉
        /// </summary>
        private void RegisterEvents()
        {
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //UI线程未捕获异常处理事件（UI主线程）
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        //Task线程报错
        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.SetObserved();
            }
        }

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //ignore
            }
        }

        //UI线程未捕获异常处理事件（UI主线程）
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                HandleException(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //处理完后，我们需要将Handler=true表示已此异常已处理过
                e.Handled = true;
            }
        }

        //处理异常到界面显示与本地日志记录
        private async Task HandleException(Exception e)
        {
            string source = e.Source ?? string.Empty;
            string message = e.Message ?? string.Empty;
            string stackTrace = e.StackTrace ?? string.Empty;
            string msg;
            if (!string.IsNullOrEmpty(source))
            {
                msg = source;
                if (!string.IsNullOrEmpty(message))
                    msg += $"\r\n{message}";
                if (!string.IsNullOrEmpty(stackTrace))
                    msg += $"\r\n\r\n{stackTrace}";
            }
            else if (!string.IsNullOrEmpty(message))
            {
                msg = message;
                if (!string.IsNullOrEmpty(stackTrace))
                    msg += $"\r\n\r\n{stackTrace}";
            }
            else if (!string.IsNullOrEmpty(stackTrace))
                msg = stackTrace;
            else
                msg = "未知异常";

            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Demo.Windows.Controls.message.MessageBox.Show(msg, "全局异常捕获", Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Exclamation);
            });

            LogHelper.Error(msg, "Optosky.Raman.AutoTest.log", e);
        }

        #endregion
    }
}
