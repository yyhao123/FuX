using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    [ProtoContract]
    public class AddressValueSimplify
    {
        [ProtoMember(1)]
        public string? SN { get; set; }

        [ProtoMember(2)]
        public string? Add { get; set; }

        [ProtoMember(3)]
        public ushort L { get; set; } = 1;


        [ProtoMember(4)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EncodingType ENC { get; set; }

        [ProtoMember(5)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QualityType Q { get; set; } = QualityType.None;


        [ProtoMember(6)]
        public string? Msg { get; set; }

        [ProtoMember(7)]
        public string? VAL { get; set; }

        [ProtoMember(8)]
        public long Ts { get; set; } = DateTime.UtcNow.Ticks;


        [ProtoMember(9)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DataType DT { get; set; } = DataType.String;


        public AddressValueSimplify()
        {
        }

        public AddressValueSimplify(string? sn, string? add, ushort l, EncodingType? enc, QualityType q, string? val, string? msg, long ts, DataType dt)
        {
            SN = sn;
            Add = add;
            L = l;
            Q = q;
            VAL = val;
            Msg = msg;
            Ts = ts;
            DT = dt;
            ENC = enc.GetValueOrDefault();
        }

        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}
