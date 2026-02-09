using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
    public class AddressParse
    {
        public object[]? ReflectionParam { get; set; }

        public object? ScriptParam { get; set; }

        public AddressParse()
        {
        }

        public AddressParse(object[] reflectionParam, object scriptParam)
        {
            ReflectionParam = reflectionParam;
            ScriptParam = scriptParam;
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
