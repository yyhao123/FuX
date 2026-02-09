using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@nterface
{
    public interface IProducer
    {
        OperateResult Produce(string topic, string content, Encoding? encoding = null);

        Task<OperateResult> ProduceAsync(string topic, string content, Encoding? encoding = null, CancellationToken token = default(CancellationToken));

        OperateResult Produce(string topic, byte[] content);

        Task<OperateResult> ProduceAsync(string topic, byte[] content, CancellationToken token = default(CancellationToken));
    }
}
