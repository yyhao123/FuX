using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public class AddressMq
    {
        public string? Topic { get; set; }

        public string? ContentFormat { get; set; }

        public List<string>? ISns { get; set; }

        public AddressMq()
        {
        }

        public AddressMq(string topic, string contentFormat, List<string> iSns)
        {
            Topic = topic;
            ContentFormat = contentFormat;
            ISns = iSns;
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
