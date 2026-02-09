using System.Text.Json.Serialization;
using System.Threading;


namespace FuX.Unility
{
    /// <summary>
    /// 事件参数异步
    /// </summary>
    public class EventArgsAsync
    {
        /// <summary>
        /// 提供用于没有事件数据的事件的值
        /// </summary>
        public static readonly EventArgsAsync Empty = new EventArgsAsync();

        [JsonIgnore]
        public CancellationToken CancellationToken { get; }

        public EventArgsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            CancellationToken = cancellationToken;
        }

        public static EventArgsAsync CreateOrDefault(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
            {
                return new EventArgsAsync(cancellationToken);
            }
            return Empty;
        }
    }

}
