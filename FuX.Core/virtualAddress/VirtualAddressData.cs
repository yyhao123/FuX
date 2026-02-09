using FuX.Model.@enum;
using FuX.Model.Specenum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Core.virtualAddress
{
    public class VirtualAddressData
    {
        public string? AddressName { get; set; }

        public AddressType AddressType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataType DataType { get; set; }
    }
}
