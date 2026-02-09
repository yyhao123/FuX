using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.Communication.net.http.service
{
    public class HttpServiceData
    {
        public class Basics : WAModel
        {
            [Description("唯一标识符")]
            public string SN { get; set; } = Guid.NewGuid().ToUpperNString();


            [Description("请求与响应的方式")]
            public HttpMethod Method { get; set; } = HttpMethod.Get;


            [Description("请求与响应的内容类型")]
            public string ContentType { get; set; } = "application/json";


            [Description("接口的绝对路径集合")]
            public List<string> AbsolutePaths { get; set; } = new List<string> { "/api/sample1", "/api/sample2" };


            public void SET(WAModel wAModel)
            {
                base.CrossDomain = wAModel.CrossDomain;
                base.IpAddress = wAModel.IpAddress;
                base.Port = wAModel.Port;
            }
        }

        public class WaitHandler
        {
            public HttpListenerRequest Request { get; set; }

            public HttpListenerResponse Response { get; set; }

            public string? BodyData { get; set; }
        }
    }
}
