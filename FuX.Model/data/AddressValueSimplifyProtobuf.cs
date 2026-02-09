using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    [ProtoContract]
    public class AddressValueSimplifyProtobuf
    {
        [ProtoMember(1)]
        public long Ts { get; set; } = DateTime.UtcNow.Ticks;


        [ProtoMember(2)]
        public IEnumerable<AddressValueSimplify> ITM { get; set; }

        public AddressValueSimplifyProtobuf()
        {
        }

        public AddressValueSimplifyProtobuf(IEnumerable<AddressValueSimplify> simplifys)
        {
            ITM = simplifys;
        }
    }
}
