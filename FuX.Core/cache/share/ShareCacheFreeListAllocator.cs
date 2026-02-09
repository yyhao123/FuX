using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.cache.share
{
    public class ShareCacheFreeListAllocator
    {
        private readonly SortedDictionary<long, ShareCacheFree> freeBlocks = new SortedDictionary<long, ShareCacheFree>();

        private long currentPosition;

        public long Allocate(int size)
        {
            ShareCacheFree shareCacheFree = null;
            long key = -1L;
            foreach (KeyValuePair<long, ShareCacheFree> freeBlock in freeBlocks)
            {
                ShareCacheFree value = freeBlock.Value;
                if (value.Length >= size && (shareCacheFree == null || value.Length < shareCacheFree.Length))
                {
                    shareCacheFree = value;
                    key = freeBlock.Key;
                }
            }
            if (shareCacheFree != null)
            {
                long position = shareCacheFree.Position;
                if (shareCacheFree.Length == size)
                {
                    freeBlocks.Remove(key);
                    return position;
                }
                shareCacheFree.Position += size;
                shareCacheFree.Length -= size;
                return position;
            }
            long result = currentPosition;
            currentPosition += size;
            return result;
        }

        public void Free(long position, int size)
        {
            ShareCacheFree shareCacheFree = new ShareCacheFree
            {
                Position = position,
                Length = size
            };
            freeBlocks[position] = shareCacheFree;
            Merge(shareCacheFree);
        }

        private void Merge(ShareCacheFree block)
        {
            if (freeBlocks.TryGetValue(block.Position - 1, out ShareCacheFree value) && value.Position + value.Length == block.Position)
            {
                block.Position = value.Position;
                block.Length += value.Length;
                freeBlocks.Remove(value.Position);
            }
            if (freeBlocks.TryGetValue(block.Position + block.Length, out ShareCacheFree value2))
            {
                block.Length += value2.Length;
                freeBlocks.Remove(value2.Position);
            }
            freeBlocks[block.Position] = block;
        }

        public void Reset()
        {
            freeBlocks.Clear();
            currentPosition = 0L;
        }
    }
}
