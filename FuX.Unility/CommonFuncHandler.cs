using MathNet.Numerics;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
    public static class CommonFuncHandler
    {
        /// <summary>
        /// 字符串转List
        /// </summary>
        /// <param name="json">字符</param>
        public static List<T> JsonStrToList<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) return new List<T>();
            return JsonConvert.DeserializeObject<T[]>(json).ToList();
        }

        /// <summary>
        /// 根据名称和数据转为枚举值
        /// </summary>
        /// <param name="enumname">枚举名</param>
        /// <param name="val">数值</param>
        /// <returns></returns>

        public static object GetEnumVal(string enumname, string val)
        {
            object obj = null;
            var path = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            foreach (var p in path)
            {
                try
                {
                    if (p.IndexOf("Model") < 0) continue;

                    Assembly assembly = Assembly.LoadFrom(p);
                    // 获取程序集中所有的枚举类型
                    Type[] enumTypes = assembly.GetTypes().Where(t => t.IsEnum).ToArray();

                    foreach (var enumType in enumTypes)
                    {
                        if (enumType.Name == enumname)
                        {
                            // 获取枚举成员
                            Array enumValues = Enum.GetValues(enumType);

                            foreach (var enumValue in enumValues)
                            {
                                if (enumValue.ToString() == val)
                                {
                                    obj = enumValue;
                                    return obj;
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            return obj;
        }

        /// <summary>
        /// 根据名称和数据获取Class对象
        /// </summary>
        /// <param name="classobj">类名</param>
        /// <param name="val">数值</param>
        /// <returns></returns>

        public static object GetClassObjVal(string classobj, string val)
        {
            object obj = null;
            var path = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            foreach (var p in path)
            {
                try
                {
                    if (p.IndexOf("Entities") < 0) continue;

                    Assembly assembly = Assembly.LoadFrom(p);
                    // 获取程序集中所有的枚举类型
                    Type[] objTypes = assembly.GetTypes().Where(t => t.IsClass).ToArray();

                    foreach (var objType in objTypes)
                    {
                        if (objType.Name == classobj)
                        {
                            var ret = JsonConvert.DeserializeObject(val, objType);
                            return ret;
                        }
                    }
                }
                catch { }
            }


            return obj;
        }

        /// <summary>
        /// 根据名称和数据获取Class对象集合
        /// </summary>
        /// <param name="classobj">类名</param>
        /// <param name="val">数值</param>
        /// <returns></returns>

        public static object GetClassObjList(string classobj, string val)
        {
            object obj = null;
            var path = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            foreach (var p in path)
            {
                try
                {
                    if (p.IndexOf("Model") < 0) continue;

                    Assembly assembly = Assembly.LoadFrom(p);
                    // 获取程序集中所有的枚举类型
                    Type[] objTypes = assembly.GetTypes().Where(t => t.IsClass).ToArray();

                    foreach (var objType in objTypes)
                    {
                        if (objType.Name == classobj)
                        {

                            Type listType = typeof(List<>).MakeGenericType(objType);

                            IList list = (IList)JsonConvert.DeserializeObject(val, listType);

                            return list;
                        }
                    }
                }
                catch { }
            }

            return obj;
        }

        /// <summary>
        /// List转字符
        /// </summary>
        /// <param name="list">数组</param>
        public static string ListToJsonStr<T>(List<T> list)
        {
            // return JsonConvert.SerializeObject(list, new RoundingJsonConverter(3));
            return string.Empty;
        }

        public static double[,] GenRawData(List<double[]> arraysList)
        {




            double[,] twoDimArray = new double[2048, 2048];
            //double[] oneDimArray; // 假设有2048个这样的一维数组待填充到二维数组中





            int row = 0;
            int col = 0;

            foreach (double[] arr in arraysList)
            {
                foreach (double val in arr)
                {
                    twoDimArray[row, col] = val;
                    col++;

                    if (col == 2048)
                    {
                        col = 0;
                        row++;
                    }
                }
            }

            return twoDimArray;
        }

        public static string[] GetTxtFilesInFolder(string folderPath)
        {
            try
            {
                // 获取指定文件夹下所有的txt文件
                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

                // 仅返回文件名，而不是文件的完整路径
                txtFiles = txtFiles.Select(Path.GetFileName).ToArray();

                return txtFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常: " + ex.Message);
                return new string[0];
            }
        }

        /// <summary>
        /// 读取导出的数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<(double[], double[])> GetFileByCsvtxtAsync(string fileName)
        {
            return await Task.Run<(double[], double[])>(() =>
            {

                //读取csv 或 txt文件
                var type = fileName.Substring(fileName.LastIndexOf('.'));
                var data = File.ReadAllLines(fileName);
                if (type == ".csv")
                {
                    var len = data.Length - 9;
                    double[] x = new double[len];
                    double[] y = new double[len];
                    for (int i = 9, j = 0; i < data.Length; i++, j++)
                    {
                        var temp = data[i].Split(',');
                        x[j] = double.Parse(temp[0]);
                        y[j] = double.Parse(temp[1]);
                    }
                    return (x, y);
                }
                else if (type == ".txt")
                {

                    double[] x = new double[data.Length];
                    double[] y = new double[data.Length];
                    for (int i = 0; i < data.Length; i++)
                    {
                        var temp = data[i].Split('\t');
                        x[i] = double.Parse(temp[0]);
                        y[i] = double.Parse(temp[1]);
                    }
                    return (x, y);
                }
                return (null, null);
            });
        }

        /// <summary>
        /// 枚举转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDiscriptionExtension(Enum value, bool isc = true)
        {
            var ret = string.Empty;
            if (isc)
            {
                var enumType = value.GetType();
                var filed = enumType.GetField(value.ToString());
                if (filed.IsDefined(typeof(DescriptionAttribute), false))
                {
                    var des = (DescriptionAttribute)filed.GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
                    return des.Description;
                }
            }
            else
            {
                ret = value.ToString();
            }
            return ret;
        }

        /// <summary>
        /// 字符串右截取
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">数据长度</param>
        /// <returns></returns>
        public static string RightSubstring(this string input, int length)
        {
            if (string.IsNullOrEmpty(input) || length <= 0)
            {
                return string.Empty;
            }

            if (input.Length <= length)
            {
                return input;
            }

            return input.Substring(input.Length - length, length);
        }

        /// <summary>
        /// 字符串转byte
        /// </summary>
        /// <param name="str">需要转入的字符（0x??或者是??）</param>
        /// <returns></returns>
        public static byte StringToByte(string str)
        {
            if (str.Length == 4)
            {
                return Convert.ToByte(str.ToLower().Replace("0x", ""), 16); ;
            }
            else
            {
                return Convert.ToByte(str, 16);
            }
        }

        /// <summary>
        /// 字符串转为编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToByteList(string str)
        {
            char[] val = str.ToCharArray();
            List<byte> ret = new List<byte>();
            foreach (char let in val)
            {
                int v = Convert.ToInt32(let);
                ret.Add(Convert.ToByte(string.Format("{0:X}", v), 16));

            }
            return ret.ToArray();
        }

        public static double[,] MonaLisa()
        {
            return new double[,] {
            { 1.001, 2.0015, 3.004, 4.0065, 5.009, 6.001, 7.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},

            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},
            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009},

            { 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009,0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009, 0.001, 0.0015, 0.004, 0.0065, 0.009}
        };
        }


        /// <summary>
        /// (多阶)通过Y值推算X值
        /// </summary>
        /// <param name="coefficients">系数</param>
        /// <param name="targetY">Y值</param>
        /// <param name="lowerBound">-</param>
        /// <param name="upperBound">-</param>
        /// <returns></returns>

        public static double SetYGetXVal(double[] coefficients, double targetY, double lowerBound = 0, double upperBound = 5)
        {
            Func<double, double> equation = CreatePolynomialFunction(coefficients, targetY);
            return FindRoots.OfFunction(equation, lowerBound, upperBound);
        }

        /// <summary>
        /// (多阶)通过Y值推算X值用方法
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        private static Func<double, double> CreatePolynomialFunction(double[] coefficients, double targetY)
        {
            return x =>
            {
                double y = 0.0;
                for (int i = 0; i < coefficients.Length; i++)
                {
                    y += coefficients[i] * Math.Pow(x, coefficients.Length - 1 - i);
                }
                return y - targetY;
            };
        }

        /// <summary>
        /// (多阶)通过X值推算Y值
        /// </summary>
        /// <param name="coefficients">系数</param>
        /// <param name="xValues">X值</param>
        /// <returns></returns>
        public static double SetXGetYVal(double[] coefficients, double xValues)
        {
            int degree = coefficients.Length - 1;

            double x = xValues;
            double y = 0.0;

            for (int j = 0; j <= degree; j++)
            {
                y += coefficients[j] * Math.Pow(x, degree - j);
            }

            return y;
        }



    }
}
