using Demo.Model.data;
using Demo.Model.@enum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.handler
{
    public class RevPackParserHandler
    {
        public static object Parse(PackageModel data)
        {
            if (data == null) return null;

            return Get16XToStr(data);
        }

        /// <summary>
        /// 返回数据位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ParseByte(PackageModel data)
        {
            return data.lDatas;
        }

        /// <summary>
        /// 解析BOOL型数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ParseBoolean(PackageModel data)
        {
            if (data.lDatas.Length <= 0) return false;
            if (data.lDatas[0] == 0) return true;

            return false;
        }

        /// <summary>
        /// 转为INT型(低位)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ParseInt(PackageModel data)
        {
            var list = data.lDatas.ToArray();
            var myint = 0;
            var bs = 8;
            for (int i = 0; i < 4; i++)
            {
                if (4 - i <= list.Length)
                    myint = myint + (list[3 - i] << (bs * Math.Abs((4 - i - list.Length))));
            }

            return myint;
        }

        /// <summary>
        /// 转为电机移动结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<bool, string> ParseMotor(PackageModel data)
        {
            switch (data.lDatas[0])
            {
                case (byte)RetByDj.Success:
                    return Tuple.Create(true, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Success));
                case (byte)RetByDj.Busy:
                    return Tuple.Create(false, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Busy));
                case (byte)RetByDj.Data_Err:
                    return Tuple.Create(false, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Data_Err));
                case (byte)RetByDj.Limit_Err:
                    return Tuple.Create(false, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Limit_Err));
                case (byte)RetByDj.Limit_Max:
                    return Tuple.Create(false, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Limit_Max));
                case (byte)RetByDj.Limit_Min:
                    return Tuple.Create(false, CommonFuncHandler.GetEnumDiscriptionExtension(RetByDj.Limit_Min));
            }

            return Tuple.Create(true, "SUCCESS！");
        }

        /// <summary>
        /// 转为ATP系列CCD数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Tuple<bool, IList<int>> ParseCCDData(PackageModel data)
        {
            switch (data.lDatas[0])
            {
                case 0x00:
                    var list = ParseToList(data, 1, 2);
                    return Tuple.Create(true, list);
                case 0xff:
                    throw new Exception("CCD data acquisition exception");
            }
            throw new Exception("CCD data acquisition exception");
        }

        /// <summary>
        /// 返回浮点型集合
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<float> ParseFloatList(PackageModel data)
        {
            var index = 0;

            var res = new List<float>();

            while (true)
            {
                var start = index * 4;

                var fbyte = new byte[] { data.lDatas[start], data.lDatas[start + 1], data.lDatas[start + 2], data.lDatas[start + 3] };

                var myFloat = BitConverter.ToSingle(fbyte, 0);

                if (float.IsNaN(myFloat))
                    myFloat = 0;

                res.Add(myFloat);

                index++;

                if (4 * index + 3 > data.lDatas.Length - 1) break;
            }

            return res;
        }

        /// <summary>
        /// 返回int型集合
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="snum">数据起始位</param>
        /// <param name="numcount">转换int字节长度</param>
        /// <returns></returns>
        public static IList<int> ParseToList(PackageModel data, int snum = 0, int numcount = 4)
        {
            var index = 0;

            var res = new List<int>();
            if (data.lDatas.Length <= 0 || data.lDatas[0] == 0XFF)
            {
                return res;
            }

            while (true)
            {
                var start = index * numcount;

                var myint = 0;
                var bs = 8;
                for (int i = 0; i < numcount; i++)
                {
                    myint = myint + (data.lDatas[start + i + snum] << (bs * (numcount - 1 - i)));
                }

                res.Add(myint);

                index++;

                if (res.Count * numcount >= data.lDatas.Length - snum) break;
            }

            return res;
        }

        /// <summary>
        /// 转换浮点类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float ParseFloat(PackageModel data)
        {
            
            var start = 0;
            var fbyte = new byte[] { data.lDatas[start], data.lDatas[start + 1], data.lDatas[start + 2], data.lDatas[start + 3] };

            var myFloat = BitConverter.ToSingle(fbyte, 0);

            if (float.IsNaN(myFloat))
                myFloat = 0;

            return myFloat;
        }

        /// <summary>
        /// 转换为字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Get16XToStr(PackageModel data)
        {
            var str = ByteToChar(data);
            string[] temp = str.Split(' ');
            string ret = string.Empty;
            foreach (var obj in temp)
            {
                ret += GetChsFromHex(obj);
            }

            return ret;
        }

        /// <summary>
        /// 字节流转换
        /// </summary>
        /// <returns></returns>
        public static string ByteToChar(PackageModel data)
        {
            var bytes = data.lDatas;
            if (bytes == null) return "";
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                //if (bytes[i] != 0)
                //{
                strBuilder.Append(bytes[i].ToString("X2"));
                strBuilder.Append(" ");
                //}
            }
            return strBuilder.ToString().TrimEnd();
        }

        /// <summary>
        /// 从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string GetChsFromHex(string hex)
        {
            if (hex == null)
                throw new Exception("hex");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                        System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message.
                    throw new Exception("hex is not a valid hex number!");
                }
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // 获得 GB2312，Chinese Simplified。
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");

            return chs.GetString(bytes);
        }
    }
}
