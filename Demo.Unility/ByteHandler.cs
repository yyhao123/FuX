namespace Demo.Unility
{
    /// <summary>
    /// 字节扩展方法
    /// </summary>
    public static class ByteHandler
    {
        /// <summary>
        /// 从 List《byte》 中查找并移除第一个与指定子序列完全匹配的连续段。
        /// </summary>
        /// <param name="source">原始 List《byte》</param>
        /// <param name="sequence">要移除的连续字节序列</param>
        /// <returns>若成功移除，返回 true；否则返回 false</returns>
        public static bool RemoveSequence(this List<byte> source, byte[] sequence)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (sequence.Length == 0 || source.Count < sequence.Length) return false;

            for (int i = 0; i <= source.Count - sequence.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < sequence.Length; j++)
                {
                    if (source[i + j] != sequence[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    source.RemoveRange(i, sequence.Length);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 从 byte[] 中移除第一个与指定子序列完全匹配的连续段，并返回新的数组副本。
        /// </summary>
        /// <param name="source">原始字节数组</param>
        /// <param name="sequence">要移除的连续字节序列</param>
        /// <returns>移除匹配段后的新数组；若无匹配则返回原数组</returns>
        public static byte[] RemoveSequence(this byte[] source, byte[] sequence)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (sequence.Length == 0 || source.Length < sequence.Length) return source;

            for (int i = 0; i <= source.Length - sequence.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < sequence.Length; j++)
                {
                    if (source[i + j] != sequence[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    // 构造新数组，排除匹配段
                    byte[] result = new byte[source.Length - sequence.Length];
                    Buffer.BlockCopy(source, 0, result, 0, i);
                    Buffer.BlockCopy(source, i + sequence.Length, result, i, source.Length - i - sequence.Length);
                    return result;
                }
            }

            // 无匹配则原样返回
            return source;
        }
    }
}
