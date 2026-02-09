using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Core.cache.process
{
    public class ProcessCacheData
    {
        [Description("绝对过期时间(分钟)")]
        public int AbsoluteExpiration { get; set; } = 60;


        [Description("滑动过期时间(分钟)")]
        public int SlidingExpiration { get; set; } = 20;


        [Description("优先级")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

    }
}
