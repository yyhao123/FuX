using FuX.Core.extend;
using FuX.Model.data;
using NetTaste;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FuX.Core.cache.share
{
    public class ShareCacheOperate : CoreUnify<ShareCacheOperate, ShareCacheData>, IDisposable
    {
        private MemoryMappedFile mappedFile;

        private MemoryMappedViewAccessor mappedViewAccessor;

        private readonly ConcurrentDictionary<string, ShareCacheEntry> indexs = new ConcurrentDictionary<string, ShareCacheEntry>();

        private readonly ShareCacheFreeListAllocator allocator = new ShareCacheFreeListAllocator();

        private const int HeaderSize = 1048576;

        private static readonly Mutex GlobalMutex = new Mutex(initiallyOwned: false, "Global_ShareCache_Mutex");

        public ShareCacheOperate(ShareCacheData basics)
            : base(basics)
        {
            Init();
            RefreshIndex();
        }

        private void RefreshIndex()
        {
            byte[] array = new byte[1048576];
            mappedViewAccessor.ReadArray(0L, array, 0, 1048576);
            string text = Encoding.UTF8.GetString(array).Trim('\0');
            indexs.Clear();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            Dictionary<string, ShareCacheEntry> dictionary = JsonSerializer.Deserialize<Dictionary<string, ShareCacheEntry>>(text);
            if (dictionary == null)
            {
                return;
            }
            foreach (KeyValuePair<string, ShareCacheEntry> item in dictionary)
            {
                indexs[item.Key] = item.Value;
            }
        }

        private void SaveIndex()
        {
            string s = JsonSerializer.Serialize(indexs);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            if (bytes.Length > 1048576)
            {
                throw new InvalidOperationException("索引数据超过 Header 区域大小");
            }
            mappedViewAccessor.WriteArray(0L, bytes, 0, bytes.Length);
            if (bytes.Length < 1048576)
            {
                mappedViewAccessor.WriteArray(bytes.Length, new byte[1048576 - bytes.Length], 0, 1048576 - bytes.Length);
            }
        }

        private void Init()
        {
            if (!Directory.Exists(base.basics.Path))
            {
                Directory.CreateDirectory(base.basics.Path);
            }
            mappedFile = MemoryMappedFile.CreateFromFile(Path.Combine(base.basics.Path, base.basics.FileName), FileMode.OpenOrCreate, base.basics.MapName, base.basics.Capacity, base.basics.Access);
            mappedViewAccessor = mappedFile.CreateViewAccessor();
        }

        private ShareCacheEntry AllocateSpace(int size)
        {
            long position = allocator.Allocate(size) + 1048576;
            return new ShareCacheEntry
            {
                Position = position,
                Length = size
            };
        }

        public OperateResult SetCache(string key, byte[] value)
        {
            BegOperate("SetCache");
            try
            {
                GlobalMutex.WaitOne();
                RefreshIndex();
                if (!indexs.TryGetValue(key, out ShareCacheEntry value2))
                {
                    value2 = AllocateSpace(value.Length);
                    indexs[key] = value2;
                }
                else if (value.Length > value2.Length)
                {
                    mappedViewAccessor.WriteArray(value2.Position, new byte[value2.Length], 0, value2.Length);
                    allocator.Free(value2.Position, value2.Length);
                    value2 = AllocateSpace(value.Length);
                    indexs[key] = value2;
                }
                mappedViewAccessor.WriteArray(value2.Position, value, 0, value.Length);
                SaveIndex();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "SetCache", 179);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "SetCache", 183);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public OperateResult SetCache<T>(string key, T t)
        {
            BegOperate("SetCache");
            try
            {
                if (t == null)
                {
                    throw new ArgumentNullException("obj", "输入对象不能为空");
                }
                string jsonString = JsonSerializer.Serialize(t);
                byte[] value = Encoding.UTF8.GetBytes(jsonString);

              
                GlobalMutex.WaitOne();
                RefreshIndex();
                if (!indexs.TryGetValue(key, out ShareCacheEntry value2))
                {
                    value2 = AllocateSpace(value.Length);
                    indexs[key] = value2;
                }
                else if (value.Length > value2.Length)
                {
                    mappedViewAccessor.WriteArray(value2.Position, new byte[value2.Length], 0, value2.Length);
                    allocator.Free(value2.Position, value2.Length);
                    value2 = AllocateSpace(value.Length);
                    indexs[key] = value2;
                }
                mappedViewAccessor.WriteArray(value2.Position, value, 0, value.Length);
                SaveIndex();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "SetCache", 179);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "SetCache", 183);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public OperateResult GetCache<T>(string key)
        {
            BegOperate("GetCache");
            try
            {
                GlobalMutex.WaitOne();
                RefreshIndex();
                if (!indexs.TryGetValue(key, out ShareCacheEntry value))
                {
                    return EndOperate(status: false, "未找到缓存: " + key, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 212);
                }
                byte[] array = new byte[value.Length];
                mappedViewAccessor.ReadArray(value.Position, array, 0, value.Length);
                if (array == null)
                {
                    throw new ArgumentNullException("byteArray", "输入字节数组不能为空");
                }

                string jsonString = Encoding.UTF8.GetString(array);

                T obj = JsonSerializer.Deserialize<T>(jsonString);
                return EndOperate(status: true, null, obj, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 217);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 221);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public Task<OperateResult> SetCacheAsync(string key, byte[] value, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            byte[] value2 = value;
            return Task.Run(() => SetCache(key2, value2), token);
        }

        public OperateResult GetCache(string key)
        {
            BegOperate("GetCache");
            try
            {
                GlobalMutex.WaitOne();
                RefreshIndex();
                if (!indexs.TryGetValue(key, out ShareCacheEntry value))
                {
                    return EndOperate(status: false, "未找到缓存: " + key, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 212);
                }
                byte[] array = new byte[value.Length];
                mappedViewAccessor.ReadArray(value.Position, array, 0, value.Length);
                return EndOperate(status: true, null, array, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 217);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "GetCache", 221);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public Task<OperateResult> GetCacheAsync(string key, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            return Task.Run(() => GetCache(key2), token);
        }

        public OperateResult RemoveCache(string key)
        {
            BegOperate("RemoveCache");
            try
            {
                GlobalMutex.WaitOne();
                RefreshIndex();
                if (!indexs.TryRemove(key, out ShareCacheEntry value))
                {
                    return EndOperate(status: false, "未找到缓存: " + key, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "RemoveCache", 250);
                }
                mappedViewAccessor.WriteArray(value.Position, new byte[value.Length], 0, value.Length);
                allocator.Free(value.Position, value.Length);
                SaveIndex();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "RemoveCache", 259);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "RemoveCache", 263);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public Task<OperateResult> RemoveCacheAsync(string key, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            return Task.Run(() => RemoveCache(key2), token);
        }

        public OperateResult ClearCache()
        {
            BegOperate("ClearCache");
            try
            {
                GlobalMutex.WaitOne();
                RefreshIndex();
                indexs.Clear();
                allocator.Reset();
                mappedViewAccessor?.Dispose();
                mappedFile?.Dispose();
                string path = Path.Combine(base.basics.Path, base.basics.FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                Init();
                SaveIndex();
                return EndOperate(status: true, null, null, null, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "ClearCache", 308);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Demo\\Shunnet\\Demo\\Demo.Core\\cache\\share\\ShareCacheOperate.cs", "ClearCache", 312);
            }
            finally
            {
                GlobalMutex.ReleaseMutex();
            }
        }

        public Task<OperateResult> ClearCacheAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() => ClearCache(), token);
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
