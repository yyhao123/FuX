using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Core.script
{
    public class ScriptData
    {
        public class Basics
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public ScriptType ScriptType { get; set; }

            public string? ScriptCode { get; set; }

            public string? ScriptFunction { get; set; }
        }

        public enum ScriptType
        {
            JavaScript
        }
    }
}
