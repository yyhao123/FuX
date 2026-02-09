using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.Communication.net.http.service
{
    public class HttpServiceOperate : CoreUnify<HttpServiceOperate, HttpServiceData.Basics>, IOn, IOff, IGetStatus, IDisposable
    {
        private HttpListener? httpListener;

        private CancellationTokenSource? Token;

        public HttpServiceOperate(HttpServiceData.Basics basics)
            : base(basics)
        {
        }

        public HttpServiceOperate()
        {
            base.basics = new HttpServiceData.Basics();
        }

        public void Write(HttpListenerResponse response, object obj)
        {
            if (obj != null)
            {
                string s = obj.ToJson();
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                response.ContentLength64 = bytes.Length;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                Close(response);
            }
        }

        public async Task WriteAsync(HttpListenerResponse response, object obj, CancellationToken token)
        {
            if (obj != null)
            {
                string s = obj.ToJson();
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                response.ContentLength64 = bytes.Length;
                await response.OutputStream.WriteAsync(bytes, 0, bytes.Length, token);
                Close(response);
            }
        }

        public void Close(HttpListenerResponse response)
        {
            response.OutputStream.Close();
            response.OutputStream.Dispose();
        }

        private async Task HandlerIncomingConnections(HttpListener listener, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    HttpListenerContext httpListenerContext = await listener.GetContextAsync();
                    string sn = Guid.NewGuid().ToUpperNString();
                    TimeHandler.Instance(sn).StartRecord();
                    HttpListenerRequest request = httpListenerContext.Request;
                    HttpListenerResponse response = httpListenerContext.Response;
                    response.ContentType = base.basics.ContentType;
                    string text = string.Empty;
                    if (request.HttpMethod.ToUpper() != base.basics.Method.ToString())
                    {
                        text = new OperateResult(status: false, "请求方式错误，必须为 [ " + base.basics.Method.ToString() + " ]", TimeHandler.Instance(sn).StopRecord().milliseconds).ToJson();
                    }
                    else if (base.basics.AbsolutePaths.FirstOrDefault((string c) => c == request.Url.AbsolutePath) == null)
                    {
                        text = new OperateResult(status: false, "[ " + request.Url.AbsolutePath + " ] 接口不存在", TimeHandler.Instance(sn).StopRecord().milliseconds).ToJson();
                    }
                    else if (request.ContentType != null && !request.ContentType.Contains(base.basics.ContentType))
                    {
                        text = new OperateResult(status: false, "内容类型错误，必须为 [ " + base.basics.ContentType + " ]", TimeHandler.Instance(sn).StopRecord().milliseconds).ToJson();
                    }
                    else
                    {
                        TimeHandler.Instance(sn).StopRecord();
                    }
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(text);
                        response.StatusCode = 400;
                        response.ContentLength64 = bytes.Length;
                        response.OutputStream.Write(bytes, 0, bytes.Length);
                        response.OutputStream.Close();
                        continue;
                    }
                    response.StatusCode = 200;
                    if (base.basics.CrossDomain)
                    {
                        response.AppendHeader("Access-Control-Allow-Origin", request.Headers["Origin"]);
                        response.AppendHeader("Access-Control-Allow-Headers", "*");
                        response.AppendHeader("Access-Control-Allow-Methods", "POST,GET,PUT,OPTIONS,DELETE");
                        response.AppendHeader("Access-Control-Allow-Credentials", "true");
                        response.AppendHeader("Access-Control-Max-Age", "3600");
                        if (request.HttpMethod == "OPTIONS")
                        {
                            response.OutputStream.Close();
                        }
                        else
                        {
                            Handler(request, response);
                        }
                    }
                    else
                    {
                        Handler(request, response);
                    }
                }
                catch (TaskCanceledException)
                {
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex3)
                {
                    OnInfoEventHandler(this, new EventInfoResult(status: false, "HandlerIncomingConnections 处理异常 : " + ex3.Message));
                }
            }
        }

        private void Handler(HttpListenerRequest request, HttpListenerResponse response)
        {
            using Stream stream = request.InputStream;
            using StreamReader streamReader = new StreamReader(stream, request.ContentEncoding);
            OnDataEventHandler(this, new EventDataResult(status: true, "Api Request", new HttpServiceData.WaitHandler
            {
                BodyData = streamReader.ReadToEnd(),
                Request = request,
                Response = response
            }));
        }

        public static void Write<T>(HttpListenerResponse response, T obj)
        {
            if (obj != null)
            {
                string s = obj.ToJson();
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                response.ContentLength64 = bytes.Length;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }

        public override void Dispose()
        {
            Off(hardClose: true);
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            await OffAsync(hardClose: true);
            await base.DisposeAsync();
        }

        public OperateResult On()
        {
            BegOperate("On");
            try
            {
                if (GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "On", 265);
                }
                if (NetHandler.IsPortInUse(base.basics.Port))
                {
                    return EndOperate(status: false, "端口被占用", null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "On", 269);
                }
                string text = $"http://{base.basics.IpAddress}:{base.basics.Port}";
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(text + "/");
                httpListener.Start();
                foreach (string absolutePath in base.basics.AbsolutePaths)
                {
                    OnInfoEventHandler(this, EventInfoResult.CreateSuccessResult(text + absolutePath));
                }
                if (Token == null)
                {
                    Token = new CancellationTokenSource();
                    HandlerIncomingConnections(httpListener, Token.Token);
                }
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "On", 285);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "On", 289);
            }
        }

        public OperateResult Off(bool hardClose = false)
        {
            BegOperate("Off");
            try
            {
                if (!hardClose && !GetStatus().GetDetails(out string message))
                {
                    return EndOperate(status: false, message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "Off", 302);
                }
                Token?.Cancel();
                Token = null;
                httpListener?.Stop();
                httpListener?.Close();
                httpListener = null;
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "Off", 310);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "Off", 314);
            }
        }

        public OperateResult GetStatus()
        {
            BegOperate("GetStatus");
            if (httpListener == null)
            {
                return EndOperate(status: false, "未启动", null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "GetStatus", 323);
            }
            if (httpListener.IsListening)
            {
                return EndOperate(status: true, "已启动", null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "GetStatus", 329);
            }
            return EndOperate(status: false, "未启动", null, null, logOutput: false, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\communication\\net\\http\\service\\HttpServiceOperate.cs", "GetStatus", 331);
        }

        public async Task<OperateResult> OnAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => On(), token);
        }

        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => Off(hardClose), token);
        }

        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetStatus(), token);
        }
    }

}
