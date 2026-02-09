using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public class WriteModel
    {
        public object Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EncodingType EncodingType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataType AddressDataType { get; set; } = DataType.String;


        public WriteModel()
        {
        }

        public WriteModel(object value, DataType addressDataType, EncodingType? encodingType = EncodingType.ANSI)
        {
            Value = value;
            AddressDataType = addressDataType;
            EncodingType = encodingType.GetValueOrDefault();
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
