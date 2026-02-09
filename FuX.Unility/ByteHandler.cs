using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FuX.Unility;

namespace FuX.Unility
{
    public static class ByteHandler
    {
        private static readonly ushort[] CrcTable = new ushort[256]
        {
        0, 49345, 49537, 320, 49921, 960, 640, 49729, 50689, 1728,
        1920, 51009, 1280, 50625, 50305, 1088, 52225, 3264, 3456, 52545,
        3840, 53185, 52865, 3648, 2560, 51905, 52097, 2880, 51457, 2496,
        2176, 51265, 55297, 6336, 6528, 55617, 6912, 56257, 55937, 6720,
        7680, 57025, 57217, 8000, 56577, 7616, 7296, 56385, 5120, 54465,
        54657, 5440, 55041, 6080, 5760, 54849, 53761, 4800, 4992, 54081,
        4352, 53697, 53377, 4160, 61441, 12480, 12672, 61761, 13056, 62401,
        62081, 12864, 13824, 63169, 63361, 14144, 62721, 13760, 13440, 62529,
        15360, 64705, 64897, 15680, 65281, 16320, 16000, 65089, 64001, 15040,
        15232, 64321, 14592, 63937, 63617, 14400, 10240, 59585, 59777, 10560,
        60161, 11200, 10880, 59969, 60929, 11968, 12160, 61249, 11520, 60865,
        60545, 11328, 58369, 9408, 9600, 58689, 9984, 59329, 59009, 9792,
        8704, 58049, 58241, 9024, 57601, 8640, 8320, 57409, 40961, 24768,
        24960, 41281, 25344, 41921, 41601, 25152, 26112, 42689, 42881, 26432,
        42241, 26048, 25728, 42049, 27648, 44225, 44417, 27968, 44801, 28608,
        28288, 44609, 43521, 27328, 27520, 43841, 26880, 43457, 43137, 26688,
        30720, 47297, 47489, 31040, 47873, 31680, 31360, 47681, 48641, 32448,
        32640, 48961, 32000, 48577, 48257, 31808, 46081, 29888, 30080, 46401,
        30464, 47041, 46721, 30272, 29184, 45761, 45953, 29504, 45313, 29120,
        28800, 45121, 20480, 37057, 37249, 20800, 37633, 21440, 21120, 37441,
        38401, 22208, 22400, 38721, 21760, 38337, 38017, 21568, 39937, 23744,
        23936, 40257, 24320, 40897, 40577, 24128, 23040, 39617, 39809, 23360,
        39169, 22976, 22656, 38977, 34817, 18624, 18816, 35137, 19200, 35777,
        35457, 19008, 19968, 36545, 36737, 20288, 36097, 19904, 19584, 35905,
        17408, 33985, 34177, 17728, 34561, 18368, 18048, 34369, 33281, 17088,
        17280, 33601, 16640, 33217, 32897, 16448
        };

        private static int[] CRC_Key = new int[256]
        {
        0, 4129, 8258, 12387, 16516, 20645, 24774, 28903, 33032, 37161,
        41290, 45419, 49548, 53677, 57806, 61935, 4657, 528, 12915, 8786,
        21173, 17044, 29431, 25302, 37689, 33560, 45947, 41818, 54205, 50076,
        62463, 58334, 9314, 13379, 1056, 5121, 25830, 29895, 17572, 21637,
        42346, 46411, 34088, 38153, 58862, 62927, 50604, 54669, 13907, 9842,
        5649, 1584, 30423, 26358, 22165, 18100, 46939, 42874, 38681, 34616,
        63455, 59390, 55197, 51132, 18628, 22757, 26758, 30887, 2112, 6241,
        10242, 14371, 51660, 55789, 59790, 63919, 35144, 39273, 43274, 47403,
        23285, 19156, 31415, 27286, 6769, 2640, 14899, 10770, 56317, 52188,
        64447, 60318, 39801, 35672, 47931, 43802, 27814, 31879, 19684, 23749,
        11298, 15363, 3168, 7233, 60846, 64911, 52716, 56781, 44330, 48395,
        36200, 40265, 32407, 28342, 24277, 20212, 15891, 11826, 7761, 3696,
        65439, 61374, 57309, 53244, 48923, 44858, 40793, 36728, 37256, 33193,
        45514, 41451, 53516, 49453, 61774, 57711, 4224, 161, 12482, 8419,
        20484, 16421, 28742, 24679, 33721, 37784, 41979, 46042, 49981, 54044,
        58239, 62302, 689, 4752, 8947, 13010, 16949, 21012, 25207, 29270,
        46570, 42443, 38312, 34185, 62830, 58703, 54572, 50445, 13538, 9411,
        5280, 1153, 29798, 25671, 21540, 17413, 42971, 47098, 34713, 38840,
        59231, 63358, 50973, 55100, 9939, 14066, 1681, 5808, 26199, 30326,
        17941, 22068, 55628, 51565, 63758, 59695, 39368, 35305, 47498, 43435,
        22596, 18533, 30726, 26663, 6336, 2273, 14466, 10403, 52093, 56156,
        60223, 64286, 35833, 39896, 43963, 48026, 19061, 23124, 27191, 31254,
        2801, 6864, 10931, 14994, 64814, 60687, 56684, 52557, 48554, 44427,
        40424, 36297, 31782, 27655, 23652, 19525, 15522, 11395, 7392, 3265,
        61215, 65342, 53085, 57212, 44955, 49082, 36825, 40952, 28183, 32310,
        20053, 24180, 11923, 16050, 3793, 7920
        };

        private static readonly byte[] cRCHighArray = new byte[256]
        {
        0, 193, 129, 64, 1, 192, 128, 65, 1, 192,
        128, 65, 0, 193, 129, 64, 1, 192, 128, 65,
        0, 193, 129, 64, 0, 193, 129, 64, 1, 192,
        128, 65, 1, 192, 128, 65, 0, 193, 129, 64,
        0, 193, 129, 64, 1, 192, 128, 65, 0, 193,
        129, 64, 1, 192, 128, 65, 1, 192, 128, 65,
        0, 193, 129, 64, 1, 192, 128, 65, 0, 193,
        129, 64, 0, 193, 129, 64, 1, 192, 128, 65,
        0, 193, 129, 64, 1, 192, 128, 65, 1, 192,
        128, 65, 0, 193, 129, 64, 0, 193, 129, 64,
        1, 192, 128, 65, 1, 192, 128, 65, 0, 193,
        129, 64, 1, 192, 128, 65, 0, 193, 129, 64,
        0, 193, 129, 64, 1, 192, 128, 65, 1, 192,
        128, 65, 0, 193, 129, 64, 0, 193, 129, 64,
        1, 192, 128, 65, 0, 193, 129, 64, 1, 192,
        128, 65, 1, 192, 128, 65, 0, 193, 129, 64,
        0, 193, 129, 64, 1, 192, 128, 65, 1, 192,
        128, 65, 0, 193, 129, 64, 1, 192, 128, 65,
        0, 193, 129, 64, 0, 193, 129, 64, 1, 192,
        128, 65, 0, 193, 129, 64, 1, 192, 128, 65,
        1, 192, 128, 65, 0, 193, 129, 64, 1, 192,
        128, 65, 0, 193, 129, 64, 0, 193, 129, 64,
        1, 192, 128, 65, 1, 192, 128, 65, 0, 193,
        129, 64, 0, 193, 129, 64, 1, 192, 128, 65,
        0, 193, 129, 64, 1, 192, 128, 65, 1, 192,
        128, 65, 0, 193, 129, 64
        };

        private static readonly byte[] cRCLowArray = new byte[256]
        {
        0, 192, 193, 1, 195, 3, 2, 194, 198, 6,
        7, 199, 5, 197, 196, 4, 204, 12, 13, 205,
        15, 207, 206, 14, 10, 202, 203, 11, 201, 9,
        8, 200, 216, 24, 25, 217, 27, 219, 218, 26,
        30, 222, 223, 31, 221, 29, 28, 220, 20, 212,
        213, 21, 215, 23, 22, 214, 210, 18, 19, 211,
        17, 209, 208, 16, 240, 48, 49, 241, 51, 243,
        242, 50, 54, 246, 247, 55, 245, 53, 52, 244,
        60, 252, 253, 61, 255, 63, 62, 254, 250, 58,
        59, 251, 57, 249, 248, 56, 40, 232, 233, 41,
        235, 43, 42, 234, 238, 46, 47, 239, 45, 237,
        236, 44, 228, 36, 37, 229, 39, 231, 230, 38,
        34, 226, 227, 35, 225, 33, 32, 224, 160, 96,
        97, 161, 99, 163, 162, 98, 102, 166, 167, 103,
        165, 101, 100, 164, 108, 172, 173, 109, 175, 111,
        110, 174, 170, 106, 107, 171, 105, 169, 168, 104,
        120, 184, 185, 121, 187, 123, 122, 186, 190, 126,
        127, 191, 125, 189, 188, 124, 180, 116, 117, 181,
        119, 183, 182, 118, 114, 178, 179, 115, 177, 113,
        112, 176, 80, 144, 145, 81, 147, 83, 82, 146,
        150, 86, 87, 151, 85, 149, 148, 84, 156, 92,
        93, 157, 95, 159, 158, 94, 90, 154, 155, 91,
        153, 89, 88, 152, 136, 72, 73, 137, 75, 139,
        138, 74, 78, 142, 143, 79, 141, 77, 76, 140,
        68, 132, 133, 69, 135, 71, 70, 134, 130, 66,
        67, 131, 65, 129, 128, 64
        };

        public static double GetDouble(ushort b3, ushort b2, ushort b1, ushort b0)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(b0).Concat(BitConverter.GetBytes(b1)).Concat(BitConverter.GetBytes(b2))
                .Concat(BitConverter.GetBytes(b3))
                .ToArray(), 0);
        }

        public static float GetSingle(ushort highOrderValue, ushort lowOrderValue)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(lowOrderValue).Concat(BitConverter.GetBytes(highOrderValue)).ToArray(), 0);
        }

        public static uint GetUInt32(ushort highOrderValue, ushort lowOrderValue)
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(lowOrderValue).Concat(BitConverter.GetBytes(highOrderValue)).ToArray(), 0);
        }

        public static byte[] GetAsciiBytes(params byte[] numbers)
        {
            return Encoding.UTF8.GetBytes(numbers.SelectMany((byte n) => n.ToString("X2")).ToArray());
        }

        public static byte[] GetAsciiBytes(params ushort[] numbers)
        {
            return Encoding.UTF8.GetBytes(numbers.SelectMany((ushort n) => n.ToString("X4")).ToArray());
        }

        public static ushort[] NetworkBytesToHostUInt16(byte[] networkBytes)
        {
            if (networkBytes == null)
            {
                throw new ArgumentNullException("networkBytes");
            }
            if (networkBytes.Length % 2 != 0)
            {
                throw new FormatException("Array networkBytes must contain an even number of bytes");
            }
            ushort[] array = new ushort[networkBytes.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(networkBytes, i * 2));
            }
            return array;
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException("hex");
            }
            if (hex.Length % 2 != 0)
            {
                throw new FormatException("Hex string must have even number of characters.");
            }
            byte[] array = new byte[hex.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return array;
        }

        public static byte CalculateLrc(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            byte b = 0;
            foreach (byte b2 in data)
            {
                b = (byte)(b + b2);
            }
            return (byte)((b ^ 0xFF) + 1);
        }

        public static byte[] CalculateCrc(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            ushort num = ushort.MaxValue;
            foreach (byte b in data)
            {
                byte b2 = (byte)(num ^ b);
                num = (ushort)(num >> 8);
                num = (ushort)(num ^ CrcTable[b2]);
            }
            return BitConverter.GetBytes(num);
        }

        public static byte[] CombineBytes(byte[] firstBytes, int firstIndex, int firstLength, byte[] secondBytes, int secondIndex, int secondLength)
        {
            byte[] array = null;
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(firstBytes, firstIndex, firstLength);
            memoryStream.Write(secondBytes, secondIndex, secondLength);
            array = memoryStream.ToArray();
            memoryStream.Close();
            return array;
        }

        public static byte[] calc_modbus_crc(byte[] buffer)
        {
            ushort num = ushort.MaxValue;
            for (int i = 0; i < buffer.Length; i++)
            {
                num = (ushort)(num ^ buffer[i]);
                for (int j = 0; j < 8; j++)
                {
                    if (((uint)num & (true ? 1u : 0u)) != 0)
                    {
                        num = (ushort)(num >> 1);
                        num = (ushort)(num ^ 0xA001u);
                    }
                    else
                    {
                        num = (ushort)(num >> 1);
                    }
                }
            }
            return BitConverter.GetBytes(num);
        }

        public static byte[] calc_crc_16(ushort polynomials, byte[] buffer)
        {
            ushort num = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                num = (ushort)(num ^ (ushort)(buffer[i] << 8));
                for (int j = 0; j < 8; j++)
                {
                    if ((num & 0x8000u) != 0)
                    {
                        num = (ushort)(num << 1);
                        num = (ushort)(num ^ polynomials);
                    }
                    else
                    {
                        num = (ushort)(num << 1);
                    }
                }
            }
            return BitConverter.GetBytes(num);
        }

        public static byte[] calc_sum_16(byte[] buffer)
        {
            ushort num = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                num = (ushort)(num + buffer[i]);
            }
            return BitConverter.GetBytes(num);
        }

        public static byte[] calc_xor(byte[] buffer)
        {
            byte b = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                b = (byte)(b ^ buffer[i]);
            }
            return new byte[1] { b };
        }

        public static byte[] hex_to_byte(string str_hex)
        {
            List<byte> list = new List<byte>();
            str_hex = str_hex.Trim();
            while (str_hex.Length > 0)
            {
                int num = str_hex.Length;
                if (num > 2)
                {
                    num = 2;
                }
                list.Add(byte.Parse(str_hex.Substring(0, num), NumberStyles.HexNumber));
                str_hex = str_hex.Substring(num).TrimStart();
            }
            return list.ToArray();
        }

        public static int To16Convert10(string str)
        {
            int num = 0;
            try
            {
                str = str.Trim().Replace(" ", "");
                return int.Parse(str, NumberStyles.AllowHexSpecifier);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static byte[] TrimEnd(this byte[] bytes)
        {
            List<byte> list = bytes.ToList();
            int num = bytes.Length - 1;
            while (num >= 0 && bytes[num] == 0)
            {
                list.RemoveAt(num);
                num--;
            }
            return list.ToArray();
        }

        public static string HexToStr(this byte[] bytes, bool ClearEmpty = false)
        {
            if (ClearEmpty)
            {
                bytes = bytes.TrimEnd();
            }
            return ByteArrayToHexString(bytes);
        }

        public static List<byte[]> ByteListSegmentation(this byte[] Data, int Segmentation)
        {
            List<byte[]> list = new List<byte[]>();
            for (int i = 0; i < Data.Length; i += Segmentation)
            {
                byte[] array = new byte[Segmentation];
                for (int j = 0; j < Segmentation; j++)
                {
                    array[j] = Data[i + j];
                }
                list.Add(array);
                if (list.Count.Equals(Data.Length / Segmentation))
                {
                    break;
                }
            }
            return list;
        }

        public static byte[]? HexStringToByteArray(string s)
        {
            return s.StrToHex();
        }

        public static string HexToStr(this object bytes, bool ClearEmpty = false)
        {
            if (bytes is byte[])
            {
                return (bytes as byte[]).HexToStr(ClearEmpty);
            }
            return ((byte)bytes).HexToStr();
        }

        public static string HexToStr(this byte bytes)
        {
            return bytes.ToString("x2");
        }

        public static byte[] StrToHex(this string hexstring)
        {
            string[] array = hexstring.Trim().Split(' ');
            byte[] array2 = new byte[array.Length];
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = Convert.ToByte(array[i], 16);
            }
            return array2;
        }

        public static string ByteArrayToHexString(byte[] data)
        {
            return string.Join(" ", data.Select((byte t) => t.ToString("X2")));
        }

        public static byte[] StringToByteArray(this string str, bool strict = true)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Trim().Replace(" ", "").Length % 2 != 0)
            {
                throw new ArgumentException("请传入有效的参数");
            }
            if (strict)
            {
                return (from t in str.Split(' ')
                        where t != null && t.Length == 2
                        select Convert.ToByte(t, 16)).ToArray();
            }
            str = str.Trim().Replace(" ", "");
            List<byte> list = new List<byte>();
            int num;
            for (num = 0; num < str.Length; num++)
            {
                char reference = str[num];
                ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(ref reference);
                char reference2 = str[++num];
                string value = string.Concat(readOnlySpan, new ReadOnlySpan<char>(ref reference2));
                list.Add(Convert.ToByte(value, 16));
            }
            return list.ToArray();
        }

        public static byte[] AsciiArrayToByteArray(this byte[] str)
        {
            if (str == null || !str.Any())
            {
                throw new ArgumentException("请传入有效的参数");
            }
            List<string> list = new List<string>();
            foreach (byte b in str)
            {
                char c = (char)b;
                list.Add(c.ToString());
            }
            return string.Join("", list).StringToByteArray(strict: false);
        }

        public static byte[] ByteArrayToAsciiArray(this byte[] str)
        {
            return Encoding.ASCII.GetBytes(string.Join("", str.Select((byte t) => t.ToString("X2"))));
        }

        public static string IntToBinaryArray(this int value, int minLength = 0)
        {
            return Convert.ToString(value, 2).PadLeft(minLength, '0');
        }

        public static int BinaryArrayToInt(this string value)
        {
            return Convert.ToInt32(value, 2);
        }

        public static List<byte[]> ByteDataSplit(this byte[] FileStreamData, int ReserveLocation, int SplitCount)
        {
            List<byte[]> list = new List<byte[]>();
            double num = Convert.ToDouble(FileStreamData.Length) / Convert.ToDouble(SplitCount - ReserveLocation);
            int num2 = (num.ToString("0.00").Contains(".00") ? int.Parse(num.ToString()) : int.Parse(((int)num + 1).ToString()));
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                byte[] array = new byte[SplitCount];
                for (int j = ReserveLocation; j < SplitCount; j++)
                {
                    array[j] = FileStreamData[num3];
                    if (num3.Equals(FileStreamData.Length - 1))
                    {
                        break;
                    }
                    num3++;
                }
                list.Add(array);
            }
            return list;
        }

        public static (byte High, byte Low) CRC(byte[] Data)
        {
            ushort num = 0;
            for (uint num2 = 7u; num2 < Data.Length; num2++)
            {
                num = (ushort)((num << 8) ^ CRC_Key[(num >> 8) ^ Data[num2]]);
            }
            byte item = (byte)((num >> 8) & 0xFF);
            byte item2 = (byte)(num & 0xFFu);
            return (item, item2);
        }

        public static byte[] StrByteToBytes(string str)
        {
            string[] array = str.Split('|');
            byte[] array2 = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    array2[i] = byte.Parse(array[i]);
                }
                catch
                {
                    array2[i] = Convert.ToByte(array[i], 16);
                }
            }
            return array2;
        }

        public static byte[] BytesDispose(byte[] Data, int StartIndex, int DeleteAfterIndex)
        {
            byte[] array = new byte[Data.Length - (StartIndex + DeleteAfterIndex)];
            try
            {
                int num = StartIndex;
                while (true)
                {
                    if (num <= Data.Length)
                    {
                        array[num - StartIndex] = Data[num];
                        if (!num.Equals(Data.Length - (DeleteAfterIndex + 1)))
                        {
                            num++;
                            continue;
                        }
                        break;
                    }
                    return array;
                }
                return array;
            }
            catch (Exception ex)
            {
                throw new Exception("Byte[] 掐头去尾异常：" + ex.Message);
            }
        }

        public static byte GetEnumValue<T>(string value)
        {
            Type? typeFromHandle = typeof(T);
            return (byte)Convert.ToInt32(Enum.Format(typeFromHandle, Enum.Parse(typeFromHandle, value.ToUpper()), "d"));
        }

        public static int ByteToInt(byte[] data, bool IsLittleEndian = false)
        {
            if (IsLittleEndian)
            {
                Array.Reverse(data);
            }
            if (data.Length != 4)
            {
                byte[] array = new byte[4];
                for (int i = 0; i < array.Length; i++)
                {
                    if (i < data.Length)
                    {
                        array[i] = data[i];
                    }
                    else
                    {
                        array[i] = 0;
                    }
                }
                data = array;
            }
            return BitConverter.ToInt32(data, 0);
        }

        public static int ByteToInt(object data, bool IsLittleEndian = false)
        {
            if (data is byte[])
            {
                return ByteToInt(data as byte[], IsLittleEndian);
            }
            return ByteToInt((byte)data, IsLittleEndian);
        }

        public static int ByteToInt(byte data, bool IsLittleEndian = false)
        {
            byte[] array = new byte[1] { data };
            if (IsLittleEndian)
            {
                Array.Reverse(array);
            }
            if (array.Length != 4)
            {
                byte[] array2 = new byte[4];
                for (int i = 0; i < array2.Length; i++)
                {
                    if (i < array.Length)
                    {
                        array2[i] = array[i];
                    }
                    else
                    {
                        array2[i] = 0;
                    }
                }
                array = array2;
            }
            return BitConverter.ToInt32(array, 0);
        }

        public static byte[] IntToBytes(int value)
        {
            byte[] array = new byte[4];
            array[3] = (byte)((uint)(value >> 24) & 0xFFu);
            array[2] = (byte)((uint)(value >> 16) & 0xFFu);
            array[1] = (byte)((uint)(value >> 8) & 0xFFu);
            array[0] = (byte)((uint)value & 0xFFu);
            return array.TrimEnd();
        }

        public static byte[] intToBytes0(int value)
        {
            byte[] array = new byte[4];
            array[3] = (byte)((uint)(value >> 24) & 0xFFu);
            array[2] = (byte)((uint)(value >> 16) & 0xFFu);
            array[1] = (byte)((uint)(value >> 8) & 0xFFu);
            array[0] = (byte)((uint)value & 0xFFu);
            return array;
        }

        public static byte[] intToBytes1(int value)
        {
            return new byte[4]
            {
            (byte)((uint)(value >> 24) & 0xFFu),
            (byte)((uint)(value >> 16) & 0xFFu),
            (byte)((uint)(value >> 8) & 0xFFu),
            (byte)((uint)value & 0xFFu)
            };
        }

        public static string HexStringToASCII(string hexstring)
        {
            byte[] array = hexstring.StrToHex();
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text = text + array[i] + " ";
            }
            string[] array2 = text.Trim().Split(new char[1] { ' ' });
            char[] array3 = new char[array2.Length];
            for (int j = 0; j < array3.Length; j++)
            {
                int value = Convert.ToInt32(array2[j]);
                array3[j] = Convert.ToChar(value);
            }
            return new string(array3);
        }

        public static int StringToHexOrDec(string strData)
        {
            int result = -1;
            try
            {
                if (strData.Length > 2)
                {
                    if (strData.Substring(0, 2).Equals("0x") || strData.Substring(0, 2).Equals("0X"))
                    {
                        result = int.Parse(strData.Substring(2, strData.Length - 2), NumberStyles.HexNumber);
                        return result;
                    }
                    result = int.Parse(strData, NumberStyles.Integer);
                    return result;
                }
                result = int.Parse(strData, NumberStyles.Integer);
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public static string disPackage(byte[] recbytes)
        {
            string text = "";
            foreach (byte b in recbytes)
            {
                text = text + b.ToString("X2") + " ";
            }
            return text;
        }

        public static byte[] int2ByteArray(int i)
        {
            return new byte[4]
            {
            (byte)((uint)(i >> 24) & 0xFFu),
            (byte)((uint)(i >> 16) & 0xFFu),
            (byte)((uint)(i >> 8) & 0xFFu),
            (byte)((uint)i & 0xFFu)
            };
        }

        public static int bytes2Int(byte[] bytes)
        {
            return (bytes[3] & 0xFF) | ((bytes[2] << 8) & 0xFF00) | ((bytes[1] << 16) & 0xFF0000) | ((bytes[0] << 24) & 0xFF0000);
        }

        public static string Int2String(int str)
        {
            return Convert.ToString(str);
        }

        public static int String2Int(string str)
        {
            int.TryParse(str, out var _);
            return Convert.ToInt32(str);
        }

        public static byte[] IntToByteArray2(int value)
        {
            return new byte[4]
            {
            (byte)((uint)(value >> 24) & 0xFFu),
            (byte)((uint)(value >> 16) & 0xFFu),
            (byte)((uint)(value >> 8) & 0xFFu),
            (byte)((uint)value & 0xFFu)
            };
        }

        public static int ByteArrayToInt2(byte[] bArr)
        {
            if (bArr.Length != 4)
            {
                return -1;
            }
            return ((bArr[0] & 0xFF) << 24) | ((bArr[1] & 0xFF) << 16) | ((bArr[2] & 0xFF) << 8) | (bArr[3] & 0xFF);
        }

        public static string StringToHexArray(string input)
        {
            char[] array = input.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder(input.Length * 3);
            char[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                int num = Convert.ToInt32(array2[i]);
                _ = $"{num:X}";
                stringBuilder.Append(Convert.ToString(num, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return stringBuilder.ToString().ToUpper();
        }

        public static string ArrayFormat<T>(T[] array)
        {
            return ArrayFormat(array, string.Empty);
        }

        public static string ArrayFormat<T>(T[] array, string format)
        {
            if (array == null)
            {
                return "NULL";
            }
            StringBuilder stringBuilder = new StringBuilder("[");
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(string.IsNullOrEmpty(format) ? array[i].ToString() : string.Format(format, array[i]));
                if (i != array.Length - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public static string ArrayFormat<T>(T array)
        {
            return ArrayFormat(array, string.Empty);
        }

        public static string ArrayFormat<T>(T array, string format)
        {
            StringBuilder stringBuilder = new StringBuilder("[");
            if ((object)array is Array array2)
            {
                foreach (object item in array2)
                {
                    stringBuilder.Append(string.IsNullOrEmpty(format) ? item.ToString() : string.Format(format, item));
                    stringBuilder.Append(",");
                }
                if (array2.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ',')
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
            }
            else
            {
                stringBuilder.Append(string.IsNullOrEmpty(format) ? array.ToString() : string.Format(format, array));
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public static void AddArrayData<T>(ref T[] array, T[] data, int max)
        {
            if (data == null || data.Length == 0)
            {
                return;
            }
            if (array.Length == max)
            {
                Array.Copy(array, data.Length, array, 0, array.Length - data.Length);
                Array.Copy(data, 0, array, array.Length - data.Length, data.Length);
            }
            else if (array.Length + data.Length > max)
            {
                T[] array2 = new T[max];
                for (int i = 0; i < max - data.Length; i++)
                {
                    array2[i] = array[i + (array.Length - max + data.Length)];
                }
                for (int j = 0; j < data.Length; j++)
                {
                    array2[array2.Length - data.Length + j] = data[j];
                }
                array = array2;
            }
            else
            {
                T[] array3 = new T[array.Length + data.Length];
                for (int k = 0; k < array.Length; k++)
                {
                    array3[k] = array[k];
                }
                for (int l = 0; l < data.Length; l++)
                {
                    array3[array3.Length - data.Length + l] = data[l];
                }
                array = array3;
            }
        }

        public static T[] ArrayExpandToLength<T>(T[] data, int length)
        {
            if (data == null)
            {
                return new T[length];
            }
            if (data.Length == length)
            {
                return data;
            }
            T[] array = new T[length];
            Array.Copy(data, array, Math.Min(data.Length, array.Length));
            return array;
        }

        public static T[] ArrayExpandToLengthEven<T>(T[] data)
        {
            if (data == null)
            {
                return new T[0];
            }
            if (data.Length % 2 == 1)
            {
                return ArrayExpandToLength(data, data.Length + 1);
            }
            return data;
        }

        public static List<T[]> ArraySplitByLength<T>(T[] array, int length)
        {
            if (array == null)
            {
                return new List<T[]>();
            }
            List<T[]> list = new List<T[]>();
            int num = 0;
            while (num < array.Length)
            {
                if (num + length < array.Length)
                {
                    T[] array2 = new T[length];
                    Array.Copy(array, num, array2, 0, length);
                    num += length;
                    list.Add(array2);
                }
                else
                {
                    T[] array3 = new T[array.Length - num];
                    Array.Copy(array, num, array3, 0, array3.Length);
                    num += length;
                    list.Add(array3);
                }
            }
            return list;
        }

        public static int[] SplitIntegerToArray(int integer, int everyLength)
        {
            int[] array = new int[integer / everyLength + ((integer % everyLength != 0) ? 1 : 0)];
            for (int i = 0; i < array.Length; i++)
            {
                if (i == array.Length - 1)
                {
                    array[i] = ((integer % everyLength == 0) ? everyLength : (integer % everyLength));
                }
                else
                {
                    array[i] = everyLength;
                }
            }
            return array;
        }

        public static bool IsTwoBytesEquel(byte[] b1, int start1, byte[] b2, int start2, int length)
        {
            if (b1 == null || b2 == null)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (b1[i + start1] != b2[i + start2])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsTwoBytesEquel(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null)
            {
                return false;
            }
            if (b1.Length != b2.Length)
            {
                return false;
            }
            return IsTwoBytesEquel(b1, 0, b2, 0, b1.Length);
        }

        public static bool IsByteTokenEquel(byte[] head, Guid token)
        {
            return IsTwoBytesEquel(head, 12, token.ToByteArray(), 0, 16);
        }

        public static bool IsTwoTokenEquel(Guid token1, Guid token2)
        {
            return IsTwoBytesEquel(token1.ToByteArray(), 0, token2.ToByteArray(), 0, 16);
        }

        public static TEnum[] GetEnumValues<TEnum>() where TEnum : struct
        {
            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }

        public static TEnum GetEnumFromString<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static T GetXmlValue<T>(XElement element, string name, T defaultValue)
        {
            if (element.Attribute(name) == null)
            {
                return defaultValue;
            }
            Type typeFromHandle = typeof(T);
            if (typeFromHandle == typeof(bool))
            {
                return (T)(object)bool.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(bool[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<bool>();
            }
            if (typeFromHandle == typeof(byte))
            {
                return (T)(object)byte.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(byte[]))
            {
                return (T)(object)element.Attribute(name).Value.ToHexBytes();
            }
            if (typeFromHandle == typeof(sbyte))
            {
                return (T)(object)sbyte.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(sbyte[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<sbyte>();
            }
            if (typeFromHandle == typeof(short))
            {
                return (T)(object)short.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(short[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<short>();
            }
            if (typeFromHandle == typeof(ushort))
            {
                return (T)(object)ushort.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(ushort[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<ushort>();
            }
            if (typeFromHandle == typeof(int))
            {
                return (T)(object)int.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(int[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<int>();
            }
            if (typeFromHandle == typeof(uint))
            {
                return (T)(object)uint.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(uint[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<uint>();
            }
            if (typeFromHandle == typeof(long))
            {
                return (T)(object)long.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(long[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<long>();
            }
            if (typeFromHandle == typeof(ulong))
            {
                return (T)(object)ulong.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(ulong[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<ulong>();
            }
            if (typeFromHandle == typeof(float))
            {
                return (T)(object)float.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(float[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<float>();
            }
            if (typeFromHandle == typeof(double))
            {
                return (T)(object)double.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(double[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<double>();
            }
            if (typeFromHandle == typeof(DateTime))
            {
                return (T)(object)DateTime.Parse(element.Attribute(name).Value);
            }
            if (typeFromHandle == typeof(DateTime[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<DateTime>();
            }
            if (typeFromHandle == typeof(string))
            {
                return (T)(object)element.Attribute(name).Value;
            }
            if (typeFromHandle == typeof(string[]))
            {
                return (T)(object)element.Attribute(name).Value.ToStringArray<string>();
            }
            throw new Exception("not supported type:" + typeFromHandle.Name);
        }

        public static string ByteToHexString(byte[] InBytes)
        {
            return ByteToHexString(InBytes, '\0');
        }

        public static string ByteToHexString(byte[] InBytes, char segment)
        {
            return ByteToHexString(InBytes, segment, 0);
        }

        public static string ByteToHexString(byte[] InBytes, char segment, int newLineCount, string format = "{0:X2}")
        {
            if (InBytes == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            long num = 0L;
            foreach (byte b in InBytes)
            {
                if (segment == '\0')
                {
                    stringBuilder.Append(string.Format(format, b));
                }
                else
                {
                    stringBuilder.Append(string.Format(format + "{1}", b, segment));
                }
                num++;
                if (newLineCount > 0 && num >= newLineCount)
                {
                    stringBuilder.Append(Environment.NewLine);
                    num = 0L;
                }
            }
            if (segment != 0 && stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 1] == segment)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        public static string ByteToHexString(string InString)
        {
            return ByteToHexString(Encoding.Unicode.GetBytes(InString));
        }

        private static int GetHexCharIndex(char ch)
        {
            switch (ch)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                case 'a':
                    return 10;
                case 'B':
                case 'b':
                    return 11;
                case 'C':
                case 'c':
                    return 12;
                case 'D':
                case 'd':
                    return 13;
                case 'E':
                case 'e':
                    return 14;
                case 'F':
                case 'f':
                    return 15;
                default:
                    return -1;
            }
        }

        public static byte[] HexStringToBytes(string hex)
        {
            MemoryStream memoryStream = new MemoryStream();
            for (int i = 0; i < hex.Length; i++)
            {
                if (i + 1 < hex.Length && GetHexCharIndex(hex[i]) >= 0 && GetHexCharIndex(hex[i + 1]) >= 0)
                {
                    memoryStream.WriteByte((byte)(GetHexCharIndex(hex[i]) * 16 + GetHexCharIndex(hex[i + 1])));
                    i++;
                }
            }
            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();
            return result;
        }

        public static byte[] BytesReverseByWord(byte[] inBytes)
        {
            if (inBytes == null)
            {
                return null;
            }
            if (inBytes.Length == 0)
            {
                return new byte[0];
            }
            byte[] array = ArrayExpandToLengthEven(inBytes.CopyArray());
            for (int i = 0; i < array.Length / 2; i++)
            {
                byte b = array[i * 2];
                array[i * 2] = array[i * 2 + 1];
                array[i * 2 + 1] = b;
            }
            return array;
        }

        public static string GetAsciiStringRender(byte[] content)
        {
            if (content == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] < 32 || content[i] > 126)
                {
                    StringBuilder stringBuilder2 = stringBuilder;
                    StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
                    handler.AppendLiteral("\\");
                    handler.AppendFormatted(content[i], "X2");
                    stringBuilder2.Append(ref handler);
                }
                else
                {
                    stringBuilder.Append((char)content[i]);
                }
            }
            return stringBuilder.ToString();
        }

        public static byte[] GetFromAsciiStringRender(string render)
        {
            if (string.IsNullOrEmpty(render))
            {
                return new byte[0];
            }
            MatchEvaluator evaluator = (Match m) => $"{Convert.ToByte(m.Value.Substring(1), 16)}";
            return Encoding.ASCII.GetBytes(Regex.Replace(render.Replace("\\r", "\r").Replace("\\n", "\n"), "\\\\[0-9A-Fa-f]{2}", evaluator));
        }

        public static byte[] BytesToAsciiBytes(byte[] inBytes)
        {
            return Encoding.ASCII.GetBytes(ByteToHexString(inBytes));
        }

        public static byte[] AsciiBytesToBytes(byte[] inBytes)
        {
            return HexStringToBytes(Encoding.ASCII.GetString(inBytes));
        }

        public static byte[] BuildAsciiBytesFrom(byte value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X2"));
        }

        public static byte[] BuildAsciiBytesFrom(short value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X4"));
        }

        public static byte[] BuildAsciiBytesFrom(ushort value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X4"));
        }

        public static byte[] BuildAsciiBytesFrom(uint value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("X8"));
        }

        public static byte[] BuildAsciiBytesFrom(byte[] value)
        {
            byte[] array = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                BuildAsciiBytesFrom(value[i]).CopyTo(array, 2 * i);
            }
            return array;
        }

        private static byte GetDataByBitIndex(int offset)
        {
            return offset switch
            {
                0 => 1,
                1 => 2,
                2 => 4,
                3 => 8,
                4 => 16,
                5 => 32,
                6 => 64,
                7 => 128,
                _ => 0,
            };
        }

        public static bool BoolOnByteIndex(byte value, int offset)
        {
            byte dataByBitIndex = GetDataByBitIndex(offset);
            return (value & dataByBitIndex) == dataByBitIndex;
        }

        public static byte SetBoolOnByteIndex(byte byt, int offset, bool value)
        {
            byte dataByBitIndex = GetDataByBitIndex(offset);
            if (value)
            {
                return (byte)(byt | dataByBitIndex);
            }
            return (byte)(byt & ~dataByBitIndex);
        }

        public static byte[] BoolArrayToByte(bool[] array)
        {
            if (array == null)
            {
                return null;
            }
            byte[] array2 = new byte[(array.Length % 8 == 0) ? (array.Length / 8) : (array.Length / 8 + 1)];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    array2[i / 8] += GetDataByBitIndex(i % 8);
                }
            }
            return array2;
        }

        public static string BoolArrayToString(bool[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i] ? "1" : "0");
            }
            return stringBuilder.ToString();
        }

        public static bool[] ByteToBoolArray(byte[] inBytes, int length)
        {
            if (inBytes == null)
            {
                return null;
            }
            if (length > inBytes.Length * 8)
            {
                length = inBytes.Length * 8;
            }
            bool[] array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = BoolOnByteIndex(inBytes[i / 8], i % 8);
            }
            return array;
        }

        public static bool[] ByteToBoolArray(byte[] inBytes, int length, byte trueValue)
        {
            if (inBytes == null)
            {
                return null;
            }
            if (length > inBytes.Length)
            {
                length = inBytes.Length;
            }
            bool[] array = new bool[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = inBytes[i] == trueValue;
            }
            return array;
        }

        public static bool[] ByteToBoolArray(byte[] InBytes)
        {
            if (InBytes != null)
            {
                return ByteToBoolArray(InBytes, InBytes.Length * 8);
            }
            return null;
        }

        public static T[] ArrayRemoveDouble<T>(T[] value, int leftLength, int rightLength)
        {
            if (value == null)
            {
                return null;
            }
            if (value.Length <= leftLength + rightLength)
            {
                return new T[0];
            }
            T[] array = new T[value.Length - leftLength - rightLength];
            Array.Copy(value, leftLength, array, 0, array.Length);
            return array;
        }

        public static T[] ArrayRemoveBegin<T>(T[] value, int length)
        {
            return ArrayRemoveDouble(value, length, 0);
        }

        public static T[] ArrayRemoveLast<T>(T[] value, int length)
        {
            return ArrayRemoveDouble(value, 0, length);
        }

        public static T[] ArraySelectMiddle<T>(T[] value, int index, int length)
        {
            if (value == null)
            {
                return null;
            }
            if (length == 0)
            {
                return new T[0];
            }
            T[] array = new T[Math.Min(value.Length, length)];
            Array.Copy(value, index, array, 0, array.Length);
            return array;
        }

        public static T[] ArraySelectBegin<T>(T[] value, int length)
        {
            if (length == 0)
            {
                return new T[0];
            }
            T[] array = new T[Math.Min(value.Length, length)];
            if (array.Length != 0)
            {
                Array.Copy(value, 0, array, 0, array.Length);
            }
            return array;
        }

        public static T[] ArraySelectLast<T>(T[] value, int length)
        {
            if (length == 0)
            {
                return new T[0];
            }
            T[] array = new T[Math.Min(value.Length, length)];
            Array.Copy(value, value.Length - length, array, 0, array.Length);
            return array;
        }

        public static T[] SpliceArray<T>(params T[][] arrays)
        {
            int num = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                T[] array = arrays[i];
                if (array != null && array.Length != 0)
                {
                    num += arrays[i].Length;
                }
            }
            int num2 = 0;
            T[] array2 = new T[num];
            for (int j = 0; j < arrays.Length; j++)
            {
                T[] array3 = arrays[j];
                if (array3 != null && array3.Length != 0)
                {
                    arrays[j].CopyTo(array2, num2);
                    num2 += arrays[j].Length;
                }
            }
            return array2;
        }

        public static string[] SpliceStringArray(string first, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.AddRange(array);
            return list.ToArray();
        }

        public static string[] SpliceStringArray(string first, string second, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.Add(second);
            list.AddRange(array);
            return list.ToArray();
        }

        public static string[] SpliceStringArray(string first, string second, string third, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.Add(second);
            list.Add(third);
            list.AddRange(array);
            return list.ToArray();
        }

        private static int HexToInt(char h)
        {
            if (h < '0' || h > '9')
            {
                if (h < 'a' || h > 'f')
                {
                    if (h < 'A' || h > 'F')
                    {
                        return -1;
                    }
                    return h - 65 + 10;
                }
                return h - 97 + 10;
            }
            return h - 48;
        }

        private static string ValidateString(string input, bool skipUtf16Validation)
        {
            if (skipUtf16Validation || string.IsNullOrEmpty(input))
            {
                return input;
            }
            int num = -1;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsSurrogate(input[i]))
                {
                    num = i;
                    break;
                }
            }
            if (num < 0)
            {
                return input;
            }
            char[] array = input.ToCharArray();
            for (int j = num; j < array.Length; j++)
            {
                char c = array[j];
                if (char.IsLowSurrogate(c))
                {
                    array[j] = '\ufffd';
                }
                else if (char.IsHighSurrogate(c))
                {
                    if (j + 1 < array.Length && char.IsLowSurrogate(array[j + 1]))
                    {
                        j++;
                    }
                    else
                    {
                        array[j] = '\ufffd';
                    }
                }
            }
            return new string(array);
        }

        private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
            {
                return false;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return true;
        }

        private static bool IsUrlSafeChar(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
            {
                return true;
            }
            switch (ch)
            {
                case '!':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                    return true;
                default:
                    return false;
            }
        }

        private static string UrlEncodeSpaces(string str)
        {
            if (str != null && str.IndexOf(' ') >= 0)
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }

        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)(n - 10 + 65);
        }

        private static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            int num = 0;
            int num2 = bytes.Length;
            if (!ValidateUrlEncodingParameters(bytes, num, num2))
            {
                return null;
            }
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < num2; i++)
            {
                char c = (char)bytes[num + i];
                if (c == ' ')
                {
                    num3++;
                }
                else if (!IsUrlSafeChar(c))
                {
                    num4++;
                }
            }
            if (num3 == 0 && num4 == 0)
            {
                if (num == 0 && bytes.Length == num2)
                {
                    return bytes;
                }
                byte[] array = new byte[num2];
                Buffer.BlockCopy(bytes, num, array, 0, num2);
                return array;
            }
            byte[] array2 = new byte[num2 + num4 * 2];
            int num5 = 0;
            for (int j = 0; j < num2; j++)
            {
                byte b = bytes[num + j];
                char c2 = (char)b;
                if (IsUrlSafeChar(c2))
                {
                    array2[num5++] = b;
                    continue;
                }
                if (c2 == ' ')
                {
                    array2[num5++] = 43;
                    continue;
                }
                array2[num5++] = 37;
                array2[num5++] = (byte)IntToHex((b >> 4) & 0xF);
                array2[num5++] = (byte)IntToHex(b & 0xF);
            }
            return array2;
        }

        private static byte[] UrlEncode(byte[] bytes, bool alwaysCreateNewReturnValue)
        {
            byte[] array = UrlEncodeToBytes(bytes);
            if (!alwaysCreateNewReturnValue || array == null || array != bytes)
            {
                return array;
            }
            return (byte[])array.Clone();
        }

        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return Encoding.ASCII.GetString(UrlEncode(bytes, alwaysCreateNewReturnValue: true));
        }

        public static T[,] CreateTwoArrayFromOneArray<T>(T[] array, int row, int col)
        {
            T[,] array2 = new T[row, col];
            int num = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    array2[i, j] = array[num];
                    num++;
                }
            }
            return array2;
        }

        public static byte[] GetCRC16(byte[] cmd, bool isHighBefore = false)
        {
            int num = 255;
            int num2 = 255;
            for (int i = 0; i < cmd.Length; i++)
            {
                int num3 = num2 ^ cmd[i];
                num2 = num ^ cRCHighArray[num3];
                num = cRCLowArray[num3];
            }
            if (!isHighBefore)
            {
                return new byte[2]
                {
                (byte)num,
                (byte)num2
                };
            }
            return new byte[2]
            {
            (byte)num2,
            (byte)num
            };
        }
    }
}
