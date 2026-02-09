using Demo.AutoTest.view.userControls;
using Demo.AutoTest.view.windows;
using Demo.AutoTest.viewModel.windows;
using Demo.Windows.Controls.filterDataGrid;
using Demo.Windows.Core.handler;
using FuX.Log;
using FuX.Model.data;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace TestTool.tc261
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static readonly LanguageModel LanguageOperate = new LanguageModel("TestTool.tc261", "Language", "TestTool.tc261.dll");
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
          //  FilterDataGrid.Properties.Resources.Culture = new System.Globalization.CultureInfo("zh_CN");
            //启动全局异常捕捉
            RegisterEvents();


           
            //打开主窗口
            //InjectionWpf.Window<UITestView, UITestViewModel>(true).Show();
            //InjectionWpf.Window<DataGridTest, DataGridViewModel>(true).Show();
            // InjectionWpf.Window<WindowView, WindowViewModel>(true).Show();
            InjectionWpf.Window<MainView, MainViewModel>(true).Show();
            //InjectionWpf.Window<TestView, TestViewModel>(true).Show();

            // InjectionWpf.Window<MinimalWindowSample, MinimalWindowSampleViewModel>(true).Show();





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

            LogHelper.Error(msg, "Demo.AutoTest.log", e);
        }

        #endregion
    }

}
