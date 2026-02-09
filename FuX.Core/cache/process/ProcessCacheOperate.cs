using FuX.Core.extend;
using FuX.Model.data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.cache.process
{
    public class ProcessCacheOperate : CoreUnify<ProcessCacheOperate, ProcessCacheData>, IDisposable
    {
        private readonly MemoryCache cacheObject = new MemoryCache(new MemoryCacheOptions());

        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();

        public ProcessCacheOperate(ProcessCacheData basics)
            : base(basics)
        {
            cacheOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(basics.AbsoluteExpiration));
            cacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(basics.SlidingExpiration));
            cacheOptions.SetPriority(basics.Priority);
        }

        public OperateResult SetCache<T>(string key, T value)
        {
            BegOperate("SetCache");
            try
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60L)
                };
                cacheObject.Set(key, value, options);
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "SetCache", 51);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "SetCache", 55);
            }
        }

        public async Task<OperateResult> SetCacheAsync<T>(string key, T value, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            T value2 = value;
            return await Task.Run(() => SetCache(key2, value2), token);
        }

        public OperateResult GetCache<T>(string key)
        {
            BegOperate("GetCache");
            try
            {
                if (cacheObject.TryGetValue<T>(key, out T value))
                {
                    return EndOperate(status: true, null, value, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "GetCache", 83);
                }
                return EndOperate(status: false, "未获取到对象键值对象", null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "GetCache", 87);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "GetCache", 92);
            }
        }

        public async Task<OperateResult> GetCacheAsync<T>(string key, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            return await Task.Run(() => GetCache<T>(key2), token);
        }

        public OperateResult RemoveCache(string key)
        {
            BegOperate("RemoveCache");
            try
            {
                cacheObject.Remove(key);
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "RemoveCache", 117);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "RemoveCache", 121);
            }
        }

        public async Task<OperateResult> RemoveCacheAsync(string key, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            return await Task.Run(() => RemoveCache(key2), token);
        }

        public OperateResult ClearCache()
        {
            BegOperate("ClearCache");
            try
            {
                cacheObject.Clear();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "ClearCache", 145);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\process\\ProcessCacheOperate.cs", "ClearCache", 149);
            }
        }

        public async Task<OperateResult> ClearCacheAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => ClearCache(), token);
        }

        public override void Dispose()
        {
            ClearCache();
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            await ClearCacheAsync();
            await base.DisposeAsync();
        }
    }
}
