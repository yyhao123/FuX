namespace Demo.Unility
{
    /// <summary>
    /// 提供针对基础整数类型的位操作（设置、清除、翻转、检测）扩展方法<br/>
    /// 支持的类型包括：byte, ushort, short, int, uint, long, ulong
    /// </summary>
    public static class BitHandler
    {
        /// <summary>
        /// 获取指定索引的位值（返回 true 表示该位为 1）
        /// 等价于 IsBitSet，只是命名更语义化。
        /// </summary>
        /// <typeparam name="T">整数类型</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">要获取的位索引</param>
        /// <returns>该位是否为 1</returns>
        public static bool GetBit<T>(this T value, int bitIndex) where T : unmanaged
        {
            return IsBitSet(value, bitIndex);
        }

        /// <summary>
        /// 获取指定索引的位值（返回 1 或 0）
        /// </summary>
        /// <typeparam name="T">整数类型（byte, short, int, etc.）</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">位索引（0 表示最低位）</param>
        /// <returns>该位的值（1 或 0）</returns>
        public static int GetBitValue<T>(this T value, int bitIndex) where T : unmanaged
        {
            ulong v = ConvertToUInt64(value);
            return (int)((v >> bitIndex) & 1UL);
        }

        /// <summary>
        /// 设置指定位为传入值（true 表示置 1，false 表示置 0）。
        /// </summary>
        /// <typeparam name="T">支持的整数类型</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">位索引（0 表示最低位）</param>
        /// <param name="bitValue">true 设置为 1，false 设置为 0</param>
        /// <returns>操作后的新值</returns>
        public static T SetBit<T>(this T value, int bitIndex, bool bitValue) where T : unmanaged
            => bitValue ? SetBit(value, bitIndex) : ClearBit(value, bitIndex);

        /// <summary>
        /// 设置指定索引的位为 1。
        /// </summary>
        /// <typeparam name="T">整数类型（byte, ushort, short, int, uint, long, ulong）</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">要设置的位索引（0 表示最低位）</param>
        /// <returns>设置指定位后的新值</returns>
        public static T SetBit<T>(this T value, int bitIndex) where T : unmanaged
            => BitOperation(value, bitIndex, BitOpType.Set);

        /// <summary>
        /// 清除指定索引的位为 0。
        /// </summary>
        /// <typeparam name="T">整数类型</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">要清除的位索引</param>
        /// <returns>清除指定位后的新值</returns>
        public static T ClearBit<T>(this T value, int bitIndex) where T : unmanaged
            => BitOperation(value, bitIndex, BitOpType.Clear);

        /// <summary>
        /// 翻转指定索引的位（1变0，0变1）。
        /// </summary>
        /// <typeparam name="T">整数类型</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">要翻转的位索引</param>
        /// <returns>翻转指定位后的新值</returns>
        public static T ToggleBit<T>(this T value, int bitIndex) where T : unmanaged
            => BitOperation(value, bitIndex, BitOpType.Toggle);

        /// <summary>
        /// 判断指定索引的位是否为 1。
        /// </summary>
        /// <typeparam name="T">整数类型</typeparam>
        /// <param name="value">原始值</param>
        /// <param name="bitIndex">要判断的位索引</param>
        /// <returns>若该位为 1，返回 true；否则返回 false</returns>
        public static bool IsBitSet<T>(this T value, int bitIndex) where T : unmanaged
        {
            ulong v = ConvertToUInt64(value);
            return (v & (1UL << bitIndex)) != 0;
        }

        /// <summary>
        /// 内部统一处理位操作的方法。
        /// </summary>
        private static T BitOperation<T>(T value, int bitIndex, BitOpType opType) where T : unmanaged
        {
            ulong v = ConvertToUInt64(value);
            ulong mask = 1UL << bitIndex;

            v = opType switch
            {
                BitOpType.Set => v | mask,
                BitOpType.Clear => v & ~mask,
                BitOpType.Toggle => v ^ mask,
                _ => v
            };

            return ConvertFromUInt64<T>(v);
        }

        /// <summary>
        /// 将任意整数类型转换为统一处理的 ulong。
        /// </summary>
        private static ulong ConvertToUInt64<T>(T value) where T : unmanaged
        {
            return value switch
            {
                byte b => b,
                ushort s => s,
                uint i => i,
                int si => (ulong)(uint)si,
                short ss => (ulong)(ushort)ss,
                long l => (ulong)l,
                ulong ul => ul,
                _ => throw new NotSupportedException($"类型 {typeof(T)} 不受支持")
            };
        }

        /// <summary>
        /// 将 ulong 转换回指定类型的值。
        /// </summary>
        private static T ConvertFromUInt64<T>(ulong value) where T : unmanaged
        {
            return (T)(typeof(T) switch
            {
                Type t when t == typeof(byte) => (object)(byte)value,
                Type t when t == typeof(ushort) => (object)(ushort)value,
                Type t when t == typeof(uint) => (object)(uint)value,
                Type t when t == typeof(int) => (object)(int)value,
                Type t when t == typeof(short) => (object)(short)value,
                Type t when t == typeof(long) => (object)(long)value,
                Type t when t == typeof(ulong) => (object)value,
                _ => throw new NotSupportedException($"类型 {typeof(T)} 不受支持")
            });
        }

        /// <summary>
        /// 位操作类型枚举：设置、清除、翻转。
        /// </summary>
        private enum BitOpType
        {
            Set,
            Clear,
            Toggle
        }
    }
}
