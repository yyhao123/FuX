using FuX.Model.data;
using FuX.Model.@enum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    public interface IWrite
    {
        OperateResult Write(ConcurrentDictionary<string, object> values);

        Task<OperateResult> WriteAsync(ConcurrentDictionary<string, object> values, CancellationToken token = default(CancellationToken));

        OperateResult Write(ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values);

        Task<OperateResult> WriteAsync(ConcurrentDictionary<string, (object value, EncodingType? encodingType)> values, CancellationToken token = default(CancellationToken));

        OperateResult Write(ConcurrentDictionary<string, WriteModel> values);

        Task<OperateResult> WriteAsync(ConcurrentDictionary<string, WriteModel> values, CancellationToken token = default(CancellationToken));
    }

}
