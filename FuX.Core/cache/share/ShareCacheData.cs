using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Core.cache.share
{
    public class ShareCacheData
    {
        [Description("默认路径")]
        public string Path { get; set; } = System.IO.Path.GetTempPath();


        [Description("文件名称")]
        public string FileName { get; set; } = "SnetShareCache.dat";


        [Description("映射名称")]
        public string MapName { get; set; } = "SnetShareCache";


        [Description("容量")]
        public long Capacity { get; set; } = 10485760L;


        [Description("缓存文件访问类型")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MemoryMappedFileAccess Access { get; set; }
    }
}
