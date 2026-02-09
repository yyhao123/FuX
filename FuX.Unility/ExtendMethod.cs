using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Caching.Memory;
using ProtoBuf;

namespace FuX.Unility
{
    //
    // 摘要:
    //     扩展方法
    public static class ExtendMethod
    {
        //
        // 摘要:
        //     枚举记录实体信息
        public class EnumberEntity
        {
            //
            // 摘要:
            //     枚举名称
            public string Name { get; set; }

            //
            // 摘要:
            //     枚举对象的值
            public int Value { get; set; }

            //
            // 摘要:
            //     枚举的描述
            public string Describe { get; set; }
        }

        //
        // 摘要:
        //     对象比对器
        private static readonly CompareLogic compareLogic = new CompareLogic(new ComparisonConfig
        {
            UseHashCodeIdentifier = true,
            IgnoreObjectTypes = true,
            CompareChildren = true
        });

        //
        // 摘要:
        //     默认结果
        private static readonly (bool result, List<Difference>? difference) defaultResult = (result: true, difference: null);

        //
        // 摘要:
        //     内存缓存
        private static MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        //
        // 摘要:
        //     验证字符串是否匹配正则表达式描述的规则
        //
        // 参数:
        //   inputStr:
        //     待验证的字符串
        //
        //   patternStr:
        //     正则表达式字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsMatch(this string inputStr, string patternStr)
        {
            return inputStr.IsMatch(patternStr, ifIgnoreCase: false, ifValidateWhiteSpace: false);
        }

        //
        // 摘要:
        //     验证字符串是否匹配正则表达式描述的规则
        //
        // 参数:
        //   inputStr:
        //     待验证的字符串
        //
        //   patternStr:
        //     正则表达式字符串
        //
        //   ifIgnoreCase:
        //     匹配时是否不区分大小写
        //
        // 返回结果:
        //     是否匹配
        public static bool IsMatch(this string inputStr, string patternStr, bool ifIgnoreCase)
        {
            return inputStr.IsMatch(patternStr, ifIgnoreCase, ifValidateWhiteSpace: false);
        }

        //
        // 摘要:
        //     验证字符串是否匹配正则表达式描述的规则
        //
        // 参数:
        //   inputStr:
        //     待验证的字符串
        //
        //   patternStr:
        //     正则表达式字符串
        //
        //   ifValidateWhiteSpace:
        //     是否验证空白字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsMatchs(this string inputStr, string patternStr, bool ifValidateWhiteSpace)
        {
            return inputStr.IsMatch(patternStr, ifIgnoreCase: false, ifValidateWhiteSpace);
        }

        //
        // 摘要:
        //     验证字符串是否匹配正则表达式描述的规则
        //
        // 参数:
        //   inputStr:
        //     待验证的字符串
        //
        //   patternStr:
        //     正则表达式字符串
        //
        //   ifIgnoreCase:
        //     匹配时是否不区分大小写
        //
        //   ifValidateWhiteSpace:
        //     是否验证空白字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsMatch(this string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))
            {
                return false;
            }

            Regex regex = null;
            regex = ((!ifIgnoreCase) ? new Regex(patternStr) : new Regex(patternStr, RegexOptions.IgnoreCase));
            return regex.IsMatch(inputStr);
        }

        //
        // 摘要:
        //     验证数字(double类型) [可以包含负号和小数点]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsNumber(this string input)
        {
            double result = 0.0;
            if (double.TryParse(input, out result))
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证整数
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsInteger(this string input)
        {
            int result = 0;
            if (int.TryParse(input, out result))
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证非负整数
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIntegerNotNagtive(this string input)
        {
            int result = -1;
            if (int.TryParse(input, out result) && result >= 0)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证正整数
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIntegerPositive(this string input)
        {
            int result = 0;
            if (int.TryParse(input, out result) && result >= 1)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证只包含英文字母
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsEnglishCharacter(this string input)
        {
            string patternStr = "^[A-Za-z]+$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证只包含数字和英文字母
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIntegerAndEnglishCharacter(this string input)
        {
            string patternStr = "^[0-9A-Za-z]+$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证只包含汉字
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsChineseCharacter(this string input)
        {
            string patternStr = "^[\\u4e00-\\u9fa5]+$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证数字长度范围（数字前端的0计长度） [若要验证固定长度，可传入相同的两个长度数值]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   lengthBegin:
        //     长度范围起始值（含）
        //
        //   lengthEnd:
        //     长度范围结束值（含）
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIntegerLength(this string input, int lengthBegin, int lengthEnd)
        {
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
            {
                if (int.TryParse(input, out var _))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        //
        // 摘要:
        //     验证字符串包含内容
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   withEnglishCharacter:
        //     是否包含英文字母
        //
        //   withNumber:
        //     是否包含数字
        //
        //   withChineseCharacter:
        //     是否包含汉字
        //
        // 返回结果:
        //     是否匹配
        public static bool IsStringInclude(this string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
            {
                return false;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("^[");
            if (withEnglishCharacter)
            {
                stringBuilder.Append("a-zA-Z");
            }

            if (withNumber)
            {
                stringBuilder.Append("0-9");
            }

            if (withChineseCharacter)
            {
                stringBuilder.Append("\\u4E00-\\u9FA5");
            }

            stringBuilder.Append("]+$");
            return input.IsMatch(stringBuilder.ToString());
        }

        //
        // 摘要:
        //     验证字符串长度范围 [若要验证固定长度，可传入相同的两个长度数值]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   lengthBegin:
        //     长度范围起始值（含）
        //
        //   lengthEnd:
        //     长度范围结束值（含）
        //
        // 返回结果:
        //     是否匹配
        public static bool IsStringLength(this string input, int lengthBegin, int lengthEnd)
        {
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证字符串长度范围（字符串内只包含数字和/或英文字母） [若要验证固定长度，可传入相同的两个长度数值]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   lengthBegin:
        //     长度范围起始值（含）
        //
        //   lengthEnd:
        //     长度范围结束值（含）
        //
        // 返回结果:
        //     是否匹配
        public static bool IsStringLengthOnlyNumberAndEnglishCharacter(this string input, int lengthBegin, int lengthEnd)
        {
            string patternStr = "^[0-9a-zA-z]{" + lengthBegin + "," + lengthEnd + "}$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证字符串长度范围 [若要验证固定长度，可传入相同的两个长度数值]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   withEnglishCharacter:
        //     是否包含英文字母
        //
        //   withNumber:
        //     是否包含数字
        //
        //   withChineseCharacter:
        //     是否包含汉字
        //
        //   lengthBegin:
        //     长度范围起始值（含）
        //
        //   lengthEnd:
        //     长度范围结束值（含）
        //
        // 返回结果:
        //     是否匹配
        public static bool IsStringLengthByInclude(this string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter, int lengthBegin, int lengthEnd)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
            {
                return false;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("^[");
            if (withEnglishCharacter)
            {
                stringBuilder.Append("a-zA-Z");
            }

            if (withNumber)
            {
                stringBuilder.Append("0-9");
            }

            if (withChineseCharacter)
            {
                stringBuilder.Append("\\u4E00-\\u9FA5");
            }

            stringBuilder.Append("]{" + lengthBegin + "," + lengthEnd + "}$");
            return input.IsMatch(stringBuilder.ToString());
        }

        //
        // 摘要:
        //     验证字符串字节数长度范围 [若要验证固定长度，可传入相同的两个长度数值；每个汉字为两个字节长度]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        //   lengthBegin:
        //     长度范围起始值（含）
        //
        //   lengthEnd:
        //     长度范围结束值（含）
        public static bool IsStringByteLength(this string input, int lengthBegin, int lengthEnd)
        {
            int byteCount = Encoding.Default.GetByteCount(input);
            if (byteCount >= lengthBegin && byteCount <= lengthEnd)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证固定电话号码 [3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsTelePhoneNumber(this string input)
        {
            string patternStr = "^(((0\\d2|0\\d{2})[- ]?)?\\d{8}|((0\\d3|0\\d{3})[- ]?)?\\d{7})(-\\d{3})?$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证手机号码 [可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，11位手机号前的0可以省略；11位手机号第二位数可以是3、4、5、8中的任意一个]
        //
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsMobilePhoneNumber(this string input)
        {
            string patternStr = "^((\\+)?86|((\\+)?86)?)0?1[3458]\\d{9}$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证电话号码（可以是固定电话号码或手机号码） [固定电话：[3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]]
        //     [手机号码：[可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，手机号前的0可以省略；手机号第二位数可以是3、4、5、8中的任意一个]]
        //
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsPhoneNumber(this string input)
        {
            string patternStr = "^((\\+)?86|((\\+)?86)?)0?1[3458]\\d{9}$|^(((0\\d2|0\\d{2})[- ]?)?\\d{8}|((0\\d3|0\\d{3})[- ]?)?\\d{7})(-\\d{3})?$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证邮政编码
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsZipCode(this string input)
        {
            if (input.Length != 6)
            {
                return false;
            }

            if (int.TryParse(input, out var _))
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证电子邮箱 [@字符前可以包含字母、数字、下划线和点号；@字符后可以包含字母、数字、下划线和点号；@字符后至少包含一个点号且点号不能是最后一个字符；最后一个点号后只能是字母或数字]
        //
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsEmail(this string input)
        {
            string patternStr = "^([\\w-\\.]+)@([\\w-\\.]+)(\\.[a-zA-Z0-9]+)$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证网址（可以匹配IPv4地址但没对IPv4地址进行格式验证；IPv6暂时没做匹配） [允许省略"://"；可以添加端口号；允许层级；允许传参；域名中至少一个点号且此点号前要有内容]
        //
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsURL(this string input)
        {
            string patternStr = "^([a-zA-Z]+://)?([\\w-\\.]+)(\\.[a-zA-Z0-9]+)(:\\d{0,5})?/?([\\w-/]*)\\.?([a-zA-Z]*)\\??(([\\w-]*=[\\w%]*&?)*)$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证IPv4地址 [第一位和最后一位数字不能是0或255；允许用0补位]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIPv4(this string input)
        {
            string[] array = input.Split('.');
            if (array.Length != 4)
            {
                return false;
            }

            int result = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0 || i == 3)
                {
                    if (!int.TryParse(array[i], out result) || result <= 0 || result >= 255)
                    {
                        return false;
                    }
                }
                else if (!int.TryParse(array[i], out result) || result < 0 || result > 255)
                {
                    return false;
                }
            }

            return true;
        }

        //
        // 摘要:
        //     验证IPv6地址 [可用于匹配任何一个合法的IPv6地址]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIPv6(this string input)
        {
            string patternStr = "^\\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))(%.+)?\\s*$";
            return input.IsMatch(patternStr);
        }

        //
        // 摘要:
        //     验证一代身份证号（15位数） [长度为15位的数字；匹配对应省份地址；生日能正确匹配]
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIDCard15(this string input)
        {
            long result = 0L;
            if (!long.TryParse(input, out result) || result.ToString().Length != 15)
            {
                return false;
            }

            if (!"11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,".Contains(input.Remove(2) + ","))
            {
                return false;
            }

            if (!DateTime.TryParse(input.Substring(6, 6).Insert(4, "/").Insert(2, "/"), out var _))
            {
                return false;
            }

            return true;
        }

        //
        // 摘要:
        //     验证二代身份证号（18位数，GB11643-1999标准） [长度为18位；前17位为数字，最后一位(校验码)可以为大小写x；匹配对应省份地址；生日能正确匹配；校验码能正确匹配]
        //
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIDCard18(this string input)
        {
            long result = 0L;
            if (!long.TryParse(input.Remove(17), out result) || result.ToString().Length != 17 || !long.TryParse(input.Replace('x', '0').Replace('X', '0'), out result))
            {
                return false;
            }

            if (!"11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,".Contains(input.Remove(2) + ","))
            {
                return false;
            }

            if (!DateTime.TryParse(input.Substring(6, 8).Insert(6, "/").Insert(4, "/"), out var _))
            {
                return false;
            }

            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(',');
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(',');
            char[] array3 = input.Remove(17).ToCharArray();
            int num = 0;
            for (int i = 0; i < 17; i++)
            {
                num += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }

            int result3 = -1;
            Math.DivRem(num, 11, out result3);
            if (array[result3] != input.Substring(17, 1).ToLower())
            {
                return false;
            }

            return true;
        }

        //
        // 摘要:
        //     验证身份证号（不区分一二代身份证号）
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsIDCard(this string input)
        {
            if (input.Length == 18)
            {
                return input.IsIDCard18();
            }

            if (input.Length == 15)
            {
                return input.IsIDCard15();
            }

            return false;
        }

        //
        // 摘要:
        //     验证经度
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsLongitude(this string input)
        {
            if (float.TryParse(input, out var result) && result >= -180f && result <= 180f)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     验证纬度
        //
        // 参数:
        //   input:
        //     待验证的字符串
        //
        // 返回结果:
        //     是否匹配
        public static bool IsLatitude(this string input)
        {
            if (float.TryParse(input, out var result) && result >= -90f && result <= 90f)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     判断是否为合法的16进制字符串
        //
        // 参数:
        //   hexString:
        public static bool IsHexadecimal(this string hexString)
        {
            try
            {
                hexString = hexString.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                if (hexString.Length % 2 != 0)
                {
                    hexString += " ";
                }

                byte[] array = new byte[hexString.Length / 2];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        //
        // 摘要:
        //     判断为空GUID
        //
        // 参数:
        //   guid:
        public static bool IsEmptyGuid(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        //
        // 摘要:
        //     判断为空GUID
        //
        // 参数:
        //   guid:
        public static bool IsEmptyGuid(this Guid? guid)
        {
            if (guid.HasValue)
            {
                return guid.Value == Guid.Empty;
            }

            return true;
        }

        //
        // 摘要:
        //     判断不为空GUID
        //
        // 参数:
        //   guid:
        public static bool IsNotEmptyGuid(this Guid guid)
        {
            return guid != Guid.Empty;
        }

        //
        // 摘要:
        //     判断不为NULL和空GUID
        //
        // 参数:
        //   guid:
        public static bool IsNotEmptyGuid(this Guid? guid)
        {
            if (guid.HasValue)
            {
                return guid.Value != Guid.Empty;
            }

            return false;
        }

        //
        // 摘要:
        //     判断字符串是否为null或""
        //
        // 参数:
        //   str:
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        //
        // 摘要:
        //     判断字符串是否为null或""或" "(包含空字符的字符串)
        //
        // 参数:
        //   str:
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        //
        // 摘要:
        //     判断一个字符串是否是GUID
        //
        // 参数:
        //   str:
        public static bool IsGuid(this string str)
        {
            Guid result;
            return Guid.TryParse(str.Trims(), out result);
        }

        //
        // 摘要:
        //     判断一个字符串是否是GUID
        //
        // 参数:
        //   str:
        public static bool IsGuid(this string str, out Guid guid)
        {
            return Guid.TryParse(str.Trims(), out guid);
        }

        //
        // 摘要:
        //     判断一个字符串是否是字体图标(以fa开头)
        //
        // 参数:
        //   str:
        public static bool IsFontIco(this string str)
        {
            return str.Trims().StartsWith("fa");
        }

        //
        // 摘要:
        //     判断字符串是否为整数
        //
        // 参数:
        //   str:
        public static bool IsInt(this string str)
        {
            int result;
            return int.TryParse(str, out result);
        }

        //
        // 摘要:
        //     判断字符串是否为整数
        //
        // 参数:
        //   str:
        public static bool IsInt(this string str, out int i)
        {
            return int.TryParse(str, out i);
        }

        //
        // 摘要:
        //     判断字符串是否是长整型
        //
        // 参数:
        //   str:
        public static bool IsLong(this string str)
        {
            long result;
            return long.TryParse(str, out result);
        }

        //
        // 摘要:
        //     判断字符串是否是长整型
        //
        // 参数:
        //   str:
        public static bool IsLong(this string str, out long l)
        {
            return long.TryParse(str, out l);
        }

        //
        // 摘要:
        //     判断字符串是否为数字
        //
        // 参数:
        //   str:
        public static bool IsDecimal(this string str)
        {
            decimal result;
            return decimal.TryParse(str, out result);
        }

        //
        // 摘要:
        //     判断字符串是否为数字
        //
        // 参数:
        //   str:
        public static bool IsDecimal(this string str, out decimal d)
        {
            return decimal.TryParse(str, out d);
        }

        //
        // 摘要:
        //     判断字符串是否为日期时间
        //
        // 参数:
        //   str:
        public static bool IsDateTime(this string str)
        {
            DateTime result;
            return DateTime.TryParse(str, out result);
        }

        //
        // 摘要:
        //     判断字符串是否为日期时间
        //
        // 参数:
        //   str:
        public static bool IsDateTime(this string str, out DateTime dt)
        {
            return DateTime.TryParse(str, out dt);
        }

        //
        // 摘要:
        //     验证字符串是否为数字
        //
        // 参数:
        //   str:
        public static bool IsDigital(this string str)
        {
            char[] array = str.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (!char.IsDigit(array[i]))
                {
                    return false;
                }
            }

            return true;
        }

        //
        // 摘要:
        //     验证是否为固话号码
        //
        // 参数:
        //   str:
        public static bool IsTelNumber(this string str)
        {
            if (!str.IsNullOrWhiteSpace() && !str.StartsWith("-"))
            {
                return str.Replace("-", "").IsDigital();
            }

            return false;
        }

        //
        // 摘要:
        //     是不是为空
        //
        // 参数:
        //   value:
        //     字符串
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNull(this string value)
        {
            return value == null;
        }

        public static bool IsEmpty(this Guid? value)
        {
            if (!value.HasValue)
            {
                return true;
            }

            return value.Value.IsEmpty();
        }

        public static bool IsEmpty(this Guid value)
        {
            if (value == Guid.Empty)
            {
                return true;
            }

            return false;
        }

        public static bool IsEmpty(this DateTime? value)
        {
            if (!value.HasValue)
            {
                return true;
            }

            return value.Value.IsEmpty();
        }

        public static bool IsEmpty(this DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return true;
            }

            return false;
        }

        public static bool IsEmpty(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            return true;
        }

        public static bool IsNum(this string value)
        {
            return Regex.IsMatch(value, "^\\d+(\\.\\d*)?$");
        }

        //
        // 摘要:
        //     判断是否为有效的 JSON 数据
        //
        // 参数:
        //   input:
        //     Json 字符串
        public static bool IsJson(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            ReadOnlySpan<char> readOnlySpan = input.AsSpan().Trim();
            if (!IsPotentialJsonStructure(readOnlySpan))
            {
                return false;
            }

            int byteCount = Encoding.UTF8.GetByteCount(readOnlySpan);
            byte[] array = null;
            Span<byte> span = ((byteCount > 1024) ? ((Span<byte>)(array = ArrayPool<byte>.Shared.Rent(byteCount))) : stackalloc byte[byteCount]);
            Span<byte> bytes = span;
            try
            {
                return IsJson(bytes[..Encoding.UTF8.GetBytes(readOnlySpan, bytes)]);
            }
            finally
            {
                if (array != null)
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }

        //
        // 摘要:
        //     判断是否为有效的 JSON 数据
        public static bool IsJson(this ReadOnlySpan<byte> utf8Bytes)
        {
            try
            {
                Utf8JsonReader reader = new Utf8JsonReader(utf8Bytes, isFinalBlock: true, default(JsonReaderState));
                using (JsonDocument.ParseValue(ref reader))
                {
                    return reader.BytesConsumed == utf8Bytes.Length;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }

        //
        // 摘要:
        //     判断是否为有效的 JSON 数据
        public static bool IsJson(this ReadOnlySequence<byte> sequence)
        {
            if (sequence.IsSingleSegment)
            {
                return sequence.FirstSpan.IsJson();
            }

            try
            {
                Utf8JsonReader reader = new Utf8JsonReader(sequence, isFinalBlock: true, default(JsonReaderState));
                using (JsonDocument.ParseValue(ref reader))
                {
                    return reader.BytesConsumed == sequence.Length;
                }
            }
            catch (JsonException)
            {
                return false;
            }
        }

        //
        // 摘要:
        //     核心结构检测
        private static bool IsPotentialJsonStructure(ReadOnlySpan<char> span)
        {
            if (span.Length < 2)
            {
                return false;
            }

            char c = span[0];
            char c2 = span[span.Length - 1];
            if (c != '"')
            {
                if (c != '[')
                {
                    if (c == '{' && c2 == '}')
                    {
                        goto IL_004b;
                    }
                }
                else if (c2 == ']')
                {
                    goto IL_004b;
                }
            }
            else if (c2 == '"')
            {
                return true;
            }

            return false;
            IL_004b:
            return span.Length > 2;
        }

        //
        // 摘要:
        //     将枚举转换为List,便于界面绑定
        //
        // 类型参数:
        //   T:
        //     枚举类型
        //
        // 返回结果:
        //     枚举转换后的List列表信息
        public static List<EnumberEntity> EnumToList<T>(this T @enum)
        {
            List<EnumberEntity> list = new List<EnumberEntity>();
            foreach (object value in Enum.GetValues(typeof(T)))
            {
                EnumberEntity enumberEntity = new EnumberEntity();
                object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    DescriptionAttribute descriptionAttribute = customAttributes[0] as DescriptionAttribute;
                    enumberEntity.Describe = descriptionAttribute.Description;
                }

                enumberEntity.Value = Convert.ToInt32(value);
                enumberEntity.Name = value.ToString();
                list.Add(enumberEntity);
            }

            return list;
        }

        //
        // 摘要:
        //     get all information of enum,include value,name and description
        //
        // 参数:
        //   enumName:
        //     the type of enumName
        public static List<dynamic> GetAllItems(this Type enumName)
        {
            List<object> list = new List<object>();
            FieldInfo[] fields = enumName.GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.FieldType.IsEnum)
                {
                    int num = (int)enumName.InvokeMember(fieldInfo.Name, BindingFlags.GetField, null, null, null);
                    string name = fieldInfo.Name;
                    string empty = string.Empty;
                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                    empty = ((customAttributes.Length == 0) ? "" : ((DescriptionAttribute)customAttributes[0]).Description);
                    dynamic val = new ExpandoObject();
                    val.Value = num;
                    val.Name = name;
                    val.Describe = empty;
                    list.Add(val);
                }
            }

            return list;
        }

        //
        // 摘要:
        //     将枚举转换为List,便于界面绑定
        //
        // 返回结果:
        //     枚举转换后的List列表信息
        public static List<EnumberEntity> EnumToList(this Type @enum)
        {
            List<EnumberEntity> list = new List<EnumberEntity>();
            foreach (object value in Enum.GetValues(@enum))
            {
                EnumberEntity enumberEntity = new EnumberEntity();
                object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: true);
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    DescriptionAttribute descriptionAttribute = customAttributes[0] as DescriptionAttribute;
                    enumberEntity.Describe = descriptionAttribute.Description;
                }

                enumberEntity.Value = Convert.ToInt32(value);
                enumberEntity.Name = value.ToString();
                list.Add(enumberEntity);
            }

            return list;
        }

        //
        // 摘要:
        //     获取成员元数据的Description特性描述信息
        //
        // 参数:
        //   obj:
        //
        //   propertyName:
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //
        //   T:System.ArgumentException:
        public static string GetDescription(this object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            PropertyInfo? property = obj.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("Property '" + propertyName + "' not found.");
            }

            return property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? propertyName;
        }

        //
        // 摘要:
        //     获取成员元数据的Description特性描述信息
        //
        // 参数:
        //   member:
        //     成员元数据对象
        //
        //   inherit:
        //     是否搜索成员的继承链以查找描述特性
        //
        // 返回结果:
        //     返回Description特性描述信息，如不存在则返回成员的名称
        public static string GetDescription(this MemberInfo member, bool inherit = false)
        {
            DescriptionAttribute attribute = member.GetAttribute<DescriptionAttribute>(inherit);
            if (attribute != null)
            {
                return attribute.Description;
            }

            DisplayNameAttribute attribute2 = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (attribute2 != null)
            {
                return attribute2.DisplayName;
            }

            return member.Name;
        }

        //
        // 摘要:
        //     获取枚举项上的System.ComponentModel.DescriptionAttribute特性的文字描述
        //
        // 参数:
        //   value:
        //     Enum
        //
        // 返回结果:
        //     string
        public static string ToDescription(this object value)
        {
            MemberInfo memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            if (!(memberInfo != null))
            {
                return value.ToString();
            }

            return memberInfo.GetDescription();
        }

        //
        // 摘要:
        //     从类型成员获取指定Attribute特性
        //
        // 参数:
        //   memberInfo:
        //     类型类型成员
        //
        //   inherit:
        //     是否从继承中查找
        //
        // 类型参数:
        //   T:
        //     Attribute特性类型
        //
        // 返回结果:
        //     存在返回第一个，不存在返回null
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
        }

        //
        // 摘要:
        //     通过枚举值，返回枚举对象
        //
        // 参数:
        //   value:
        //     值
        //
        // 类型参数:
        //   T:
        //     枚举
        public static T EnumValueToEnumObj<T>(this int value)
        {
            return Enum.GetName(typeof(T), value).EnumKeyToEnumObj<T>();
        }

        //
        // 摘要:
        //     通过枚举值，返回枚举对象
        //
        // 参数:
        //   value:
        //     值
        //
        // 类型参数:
        //   T:
        //     枚举
        public static T EnumValueToEnumObj<T>(this byte value)
        {
            return Enum.GetName(typeof(T), value).EnumKeyToEnumObj<T>();
        }

        //
        // 摘要:
        //     通过枚举名称返回枚举对象
        //
        // 参数:
        //   Key:
        //
        // 类型参数:
        //   T:
        public static T EnumKeyToEnumObj<T>(this string Key)
        {
            return (T)Enum.Parse(typeof(T), Key);
        }

        //
        // 摘要:
        //     微秒延时
        //
        // 参数:
        //   time:
        //     延时时间,单位:ms
        public static void DelayUs(this double time)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.Elapsed.TotalMilliseconds < time)
            {
            }

            stopwatch.Stop();
        }

        //
        // 摘要:
        //     微秒延时
        //
        // 参数:
        //   time:
        //     延时时间,单位:ms
        public static void DelayUs(this int time)
        {
            time.DelayUs();
        }

        //
        // 摘要:
        //     对象比对器
        //
        // 参数:
        //   a:
        //     对象A
        //
        //   b:
        //     对象B
        //
        //   propertyNames:
        //     按名称忽略特定成员
        //     填属性名称
        //
        // 类型参数:
        //   T:
        //     泛型对象
        //
        // 返回结果:
        //     (是否一致，差异结果)
        public static (bool result, List<Difference>? difference) Comparer<T>(this T a, T b, string[]? propertyNames = null)
        {
            (bool, List<Difference>) result = defaultResult;
            if (propertyNames != null && propertyNames.Length != 0)
            {
                compareLogic.Config.MembersToIgnore.AddRange(propertyNames.ToList());
            }

            ComparisonResult comparisonResult = compareLogic.Compare(a, b);
            result.Item1 = comparisonResult.AreEqual;
            if (!comparisonResult.AreEqual)
            {
                result.Item2 = comparisonResult.Differences;
            }

            compareLogic.Config.MembersToIgnore.Clear();
            return result;
        }

        //
        // 摘要:
        //     复制自身对象[ 深拷贝 ]
        //
        // 参数:
        //   obj:
        //     对象
        //
        // 类型参数:
        //   T:
        //     泛型对象
        //
        // 返回结果:
        //     返回深拷贝对象
        public static T? DeepCopy<T>(this T obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            string text = obj.ToJson();
            if (text != null)
            {
                return text.ToJsonEntity<T>();
            }

            return default(T);
        }

        //
        // 摘要:
        //     拷贝当前的实例数组，是基于引用层的浅拷贝，如果类型为值类型，那就是深度拷贝，如果类型为引用类型，就是浅拷贝
        //
        // 参数:
        //   value:
        //     数组对象
        //
        // 类型参数:
        //   T:
        //     类型对象
        //
        // 返回结果:
        //     拷贝的结果内容
        public static T[] CopyArray<T>(this T[] value)
        {
            if (value == null)
            {
                return null;
            }

            T[] array = new T[value.Length];
            Array.Copy(value, array, value.Length);
            return array;
        }

        //
        // 摘要:
        //     字节数组转值类型结构体；
        //     用于操作C/C++的结构体字节数据；
        //
        // 参数:
        //   data:
        //     字节数组
        //
        //   offset:
        //     起始位偏移量，默认0
        //
        // 类型参数:
        //   T:
        //     值类型结构体
        //
        // 返回结果:
        //     值类型结构对象
        public static T ByteArrayToStructure<T>(this byte[] data, int offset = 0) where T : struct
        {
            int num = Marshal.SizeOf<T>();
            nint num2 = Marshal.AllocHGlobal(num);
            try
            {
                Marshal.Copy(data, offset, num2, num);
                return Marshal.PtrToStructure<T>(num2);
            }
            catch
            {
                return default(T);
            }
            finally
            {
                Marshal.FreeHGlobal(num2);
            }
        }

        //
        // 摘要:
        //     值类型结构体转字节数组；
        //     用于生成C/C++兼容的结构体二进制数据；
        //
        // 参数:
        //   structure:
        //     结构体对象
        //
        // 类型参数:
        //   T:
        //     值类型结构体
        //
        // 返回结果:
        //     结构体对应的字节数组
        public static byte[] StructureToByteArray<T>(this T structure) where T : struct
        {
            int num = Marshal.SizeOf<T>();
            nint num2 = Marshal.AllocHGlobal(num);
            try
            {
                Marshal.StructureToPtr(structure, num2, fDeleteOld: false);
                byte[] array = new byte[num];
                Marshal.Copy(num2, array, 0, num);
                return array;
            }
            catch
            {
                return Array.Empty<byte>();
            }
            finally
            {
                Marshal.FreeHGlobal(num2);
            }
        }

        //
        // 摘要:
        //     属性是否在当前对象中存在
        //
        // 参数:
        //   data:
        //     泛型对象数据
        //
        //   propertyName:
        //     属性名称
        //
        // 类型参数:
        //   T:
        //     泛型对象
        //
        // 返回结果:
        //     状态
        public static bool PropertyExists<T>(this T data, string propertyName, out string? message, out PropertyInfo? propertyInfo)
        {
            if (data != null)
            {
                PropertyInfo property = data.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    message = $"[ {data.GetType().Name} ] 中存在 [ {propertyName} ] 属性";
                    propertyInfo = property;
                    return true;
                }

                message = $"[ {data.GetType().Name} ] 中不存在 [ {propertyName} ] 属性";
                propertyInfo = null;
                return false;
            }

            message = "入参不能为null";
            propertyInfo = null;
            return false;
        }

        //
        // 摘要:
        //     获取对应数据源
        //
        // 参数:
        //   obj:
        //     通用类型
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     指定类型的数据
        public static T? GetSource<T>(this object obj)
        {
            if (obj != null)
            {
                try
                {
                    return (T)obj;
                }
                catch
                {
                    return default(T);
                }
            }

            return default(T);
        }

        //
        // 摘要:
        //     将GUID转换为整数
        //
        // 参数:
        //   guid:
        public static int ToInt(this Guid guid)
        {
            return Math.Abs(guid.GetHashCode());
        }

        //
        // 摘要:
        //     转换GUID为大写字符串
        //
        // 参数:
        //   guid:
        public static string ToUpperString(this Guid guid)
        {
            return guid.ToString().ToUpper();
        }

        //
        // 摘要:
        //     转换guid为小写字符串
        //
        // 参数:
        //   guid:
        public static string ToLowerString(this Guid guid)
        {
            return guid.ToString().ToLower();
        }

        //
        // 摘要:
        //     没有分隔线的字符串
        //
        // 参数:
        //   guid:
        public static string ToNString(this Guid guid)
        {
            return guid.ToString("N");
        }

        //
        // 摘要:
        //     没有分隔线的小写字符串
        //
        // 参数:
        //   guid:
        public static string ToLowerNString(this Guid guid)
        {
            return guid.ToString("N").ToLower();
        }

        //
        // 摘要:
        //     没有分隔线的大写字符串
        //
        // 参数:
        //   guid:
        public static string ToUpperNString(this Guid guid)
        {
            return guid.ToString("N").ToUpper();
        }

        //
        // 摘要:
        //     将int转换为GUID
        //
        // 参数:
        //   i:
        //     111
        //
        // 返回结果:
        //     00000000-0000-0000-0000-000000000111
        public static Guid ToGuid(this int i)
        {
            return i.ToString().PadLeft(32, '0').ToGuid();
        }

        //
        // 摘要:
        //     将long转换为GUID
        //
        // 参数:
        //   i:
        //     111
        //
        // 返回结果:
        //     00000000-0000-0000-0000-000000000111
        public static Guid ToGuid(this long i)
        {
            return i.ToString().PadLeft(32, '0').ToGuid();
        }

        //
        // 摘要:
        //     将GUID转换为int
        //
        // 参数:
        //   guid:
        //     00000000-0000-0000-0000-000000000111 的GUID
        //
        // 返回结果:
        //     111
        public static int ToInteger(this Guid guid)
        {
            return guid.ToNString().TrimStart('0').ToInt();
        }

        //
        // 摘要:
        //     将GUID转换为long
        //
        // 参数:
        //   guid:
        //     00000000-0000-0000-0000-000000000111 的GUID
        //
        // 返回结果:
        //     111
        public static long ToLong(this Guid guid)
        {
            return guid.ToNString().TrimStart('0').ToLong();
        }

        //
        // 摘要:
        //     格式化为长日期格式(yyyy年M月d日)
        //
        // 参数:
        //   date:
        public static string ToLongDate(this DateTime date)
        {
            return date.ToString("yyyy年M月d日");
        }

        //
        // 摘要:
        //     格式化为长日期格式(yyyy年MM月dd日)
        //
        // 参数:
        //   date:
        public static string ToLongDate1(this DateTime date)
        {
            return date.ToString("yyyy年MM月dd日");
        }

        //
        // 摘要:
        //     格式化为日期格式(yyyy-MM-dd)
        //
        // 参数:
        //   date:
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        //
        // 摘要:
        //     格式化为日期时间格式(yyyy-MM-dd HH:mm:ss.fff)
        //
        // 参数:
        //   date:
        public static string ToDateTimeString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        //
        // 摘要:
        //     格式化为日期时间格式(yyyy-MM-dd HH:mm)
        //
        // 参数:
        //   date:
        public static string ToShortDateTimeString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        //
        // 摘要:
        //     格式化为日期时间格式(yyyy-MM-dd HH:mm:ss)
        //
        // 参数:
        //   date:
        public static string ToDateTimeString(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }

            return date.Value.ToDateTimeString();
        }

        //
        // 摘要:
        //     取日期时间的日期部分 yyyy-MM-dd
        //
        // 参数:
        //   date:
        public static DateTime GetDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd").ToDateTime();
        }

        //
        // 摘要:
        //     取日期时间的 yyyy-MM-dd HH:mm:ss.fff
        //
        // 参数:
        //   date:
        public static DateTime GetDateDetails(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff").ToDateTime();
        }

        //
        // 摘要:
        //     比较字符串区分大小写
        //
        // 参数:
        //   str1:
        //     字符串1
        //
        //   str2:
        //     字符串2
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return str1?.Equals(str2, StringComparison.CurrentCultureIgnoreCase) ?? (str2 == null);
        }

        //
        // 摘要:
        //     判断字符串是否包含（不区分大不写）
        //
        // 参数:
        //   str1:
        //
        //   str2:
        public static bool ContainsIgnoreCase(this string str1, string str2)
        {
            if (str1 != null && str2 != null)
            {
                return str1.IndexOf(str2, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }

            return false;
        }

        //
        // 摘要:
        //     去除空格
        //
        // 参数:
        //   str:
        public static string Trims(this string str)
        {
            if (!str.IsNullOrEmpty())
            {
                return str.Trim();
            }

            return "";
        }

        //
        // 摘要:
        //     将字符串转换为GUID
        //
        // 参数:
        //   str:
        public static Guid ToGuid(this string str)
        {
            if (!Guid.TryParse(str.Trims(), out var result))
            {
                return Guid.Empty;
            }

            return result;
        }

        //
        // 摘要:
        //     将字符串转换为整数
        //
        // 参数:
        //   str:
        //
        //   defaultValue:
        //     转换失败时的默认值
        public static int ToInt(this string str, int defaultValue = int.MinValue)
        {
            if (!int.TryParse(str, out var result))
            {
                return defaultValue;
            }

            return result;
        }

        //
        // 摘要:
        //     将字符串转换为长整型
        //
        // 参数:
        //   str:
        //
        //   defauleValue:
        //     转换失败后的默认值
        public static long ToLong(this string str, long defauleValue = long.MinValue)
        {
            if (!long.TryParse(str, out var result))
            {
                return defauleValue;
            }

            return result;
        }

        //
        // 摘要:
        //     将字符串转换为数字
        //
        // 参数:
        //   str:
        //
        //   defaultValue:
        //     转换失败时的默认值
        public static decimal ToDecimal(this string str, decimal defaultValue = decimal.MinValue)
        {
            if (!decimal.TryParse(str, out var result))
            {
                return defaultValue;
            }

            return result;
        }

        //
        // 摘要:
        //     将字符串转换为日期时间
        //
        // 参数:
        //   str:
        public static DateTime ToDateTime(this string str)
        {
            if (!DateTime.TryParse(str, out var result))
            {
                return DateTime.MinValue;
            }

            return result;
        }

        //
        // 摘要:
        //     移出所有空格
        //
        // 参数:
        //   str:
        public static string TrimAll(this string str)
        {
            return Regex.Replace(str, "\\s", "");
        }

        //
        // 摘要:
        //     转换为SQL的in字符串
        //
        // 参数:
        //   str:
        //     逗号分开的字符串
        //
        //   isSignle:
        //     是否加单引号
        public static string ToSqlIn(this string str, bool isSignle = true)
        {
            if (str.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in array)
            {
                if (isSignle)
                {
                    stringBuilder.Append("'");
                }

                stringBuilder.Append(value);
                if (isSignle)
                {
                    stringBuilder.Append("'");
                }

                stringBuilder.Append(",");
            }

            return stringBuilder.ToString().TrimEnd(',');
        }

        //
        // 摘要:
        //     URL编码
        //
        // 参数:
        //   url:
        public static string UrlEncode(this string url)
        {
            return WebUtility.UrlEncode(url);
        }

        //
        // 摘要:
        //     URL解码
        //
        // 参数:
        //   url:
        public static string UrlDecode(this string url)
        {
            return WebUtility.UrlDecode(url);
        }

        //
        // 摘要:
        //     HTML编码
        //
        // 参数:
        //   str:
        public static string HtmlEncode(this string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        //
        // 摘要:
        //     HTML解码
        //
        // 参数:
        //   str:
        public static string HtmlDecode(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        //
        // 摘要:
        //     将List拼接为字符串
        //
        // 参数:
        //   ts:
        //
        //   split:
        //     分隔符
        //
        //   prefix:
        //     前缀
        //
        //   suffix:
        //     后缀
        public static string JoinList<T>(this IEnumerable<T> ts, string split = ",", string prefix = "", string suffix = "")
        {
            if (ts == null || !ts.Any())
            {
                return "";
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (T t in ts)
            {
                stringBuilder.Append(prefix);
                stringBuilder.Append(t);
                stringBuilder.Append(suffix);
                stringBuilder.Append(split);
            }

            return stringBuilder.ToString().TrimEnd(split.ToCharArray());
        }

        //
        // 摘要:
        //     将List转换为SQL in语句
        //
        // 参数:
        //   ts:
        //
        //   single:
        //     是包含单引号
        //
        // 类型参数:
        //   T:
        public static string JoinSqlIn<T>(this List<T> ts, bool single = true)
        {
            return ts.JoinList(",", single ? "'" : "", single ? "'" : "");
        }

        //
        // 摘要:
        //     得到实符串实际长度
        //
        // 参数:
        //   str:
        public static int Size(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        //
        // 摘要:
        //     将日期时间转换为INT
        //
        // 参数:
        //   dateTime:
        //     日期时间
        public static int ToInt(this DateTime dateTime)
        {
            DateTime dateTime2 = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((Convert.ToDateTime(dateTime) - dateTime2).TotalSeconds);
        }

        //
        // 摘要:
        //     将INT转换为日期时间
        //
        // 参数:
        //   ticks:
        public static DateTime ToDateTime(this int ticks)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(ticks).ToLocalTime();
        }

        public static string UTF8ToBase64(this string data)
        {
            if (data.IsEmpty())
            {
                return string.Empty;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        public static string Base64ToUTF8(this string data)
        {
            if (data.IsEmpty())
            {
                return string.Empty;
            }

            return Convert.FromBase64String(data).ToUTF8String();
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] Base64ToBytes(this string binary)
        {
            return Convert.FromBase64String(binary);
        }

        public static string ComputeSHA256(this byte[] bytes)
        {
            SHA256 sHA = SHA256.Create();
            byte[] array = sHA.ComputeHash(bytes);
            sHA.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string ComputeSHA256(this string word)
        {
            return Encoding.UTF8.GetBytes(word).ComputeSHA256();
        }

        public static string ComputeSHA256(this FileStream stream)
        {
            SHA256 sHA = SHA256.Create();
            byte[] array = sHA.ComputeHash(stream);
            sHA.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string ComputeSHA1(this byte[] bytes)
        {
            SHA1 sHA = SHA1.Create();
            byte[] array = sHA.ComputeHash(bytes);
            sHA.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string ComputeSHA1(this string word)
        {
            return Encoding.UTF8.GetBytes(word).ComputeSHA1();
        }

        public static string ComputeSHA1(this FileStream stream)
        {
            SHA1 sHA = SHA1.Create();
            byte[] array = sHA.ComputeHash(stream);
            sHA.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string ToMD5(this string plainText)
        {
            StringBuilder stringBuilder = new StringBuilder(32);
            using (MD5 mD = new MD5CryptoServiceProvider())
            {
                byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
                }
            }

            return stringBuilder.ToString();
        }

        public static string ToEncodeString(this byte[] dealBytes, Encoding encode)
        {
            return encode.GetString(dealBytes);
        }

        public static string ToUTF8String(this byte[] dealBytes)
        {
            return Encoding.UTF8.GetString(dealBytes);
        }

        public static byte[] ToBytes(this string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static byte[] ToBytesDefault(this string data)
        {
            return Encoding.Default.GetBytes(data);
        }

        public static string ToStringExt(this object data)
        {
            return data?.ToString() ?? string.Empty;
        }

        public static string ToStringExt(this DateTime? data, string format)
        {
            return data?.ToString(format) ?? string.Empty;
        }

        public static string ToStringExt(this DateTimeOffset? data, string format)
        {
            return data?.ToString(format) ?? string.Empty;
        }

        public static ulong ToULong(this decimal data)
        {
            return decimal.ToUInt64(data);
        }

        public static uint ToUInt(this decimal data)
        {
            return decimal.ToUInt32(data);
        }

        public static long ToLong(this decimal data)
        {
            return decimal.ToInt64(data);
        }

        public static int ToInt(this decimal data)
        {
            return decimal.ToInt32(data);
        }

        public static long ToLong(this object data)
        {
            if (data == null)
            {
                return 0L;
            }

            if (data is bool)
            {
                return ((bool)data) ? 1 : 0;
            }

            if (long.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            try
            {
                return Convert.ToInt64(data.ToDouble(0));
            }
            catch (Exception)
            {
                return 0L;
            }
        }

        public static ulong ToULong(this object data)
        {
            if (data == null)
            {
                return 0uL;
            }

            if (data is bool)
            {
                if (!(bool)data)
                {
                    return 0uL;
                }

                return 1uL;
            }

            if (ulong.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            try
            {
                return Convert.ToUInt64(data.ToDouble(0));
            }
            catch (Exception)
            {
                return 0uL;
            }
        }

        public static int ToInt(this object data)
        {
            if (data == null)
            {
                return 0;
            }

            if (data is bool)
            {
                if (!(bool)data)
                {
                    return 0;
                }

                return 1;
            }

            if (int.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            try
            {
                return Convert.ToInt32(data.ToDouble(0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int? ToIntOrNull(this object data)
        {
            if (data == null)
            {
                return null;
            }

            if (int.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            return null;
        }

        public static double ToDouble(this object data)
        {
            if (data == null)
            {
                return 0.0;
            }

            if (!double.TryParse(data.ToString(), out var result))
            {
                return 0.0;
            }

            return result;
        }

        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(data.ToDouble(), digits, MidpointRounding.AwayFromZero);
        }

        public static double? ToDoubleOrNull(this object data)
        {
            if (data == null)
            {
                return null;
            }

            if (double.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            return null;
        }

        public static decimal ToDecimal(this object data)
        {
            if (data == null)
            {
                return 0m;
            }

            if (!decimal.TryParse(data.ToString(), out var result))
            {
                return 0m;
            }

            return result;
        }

        public static decimal ToDecimal(this object data, int digits)
        {
            return Math.Round(data.ToDecimal(), digits, MidpointRounding.AwayFromZero);
        }

        public static decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
            {
                return null;
            }

            if (decimal.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            return null;
        }

        public static decimal? ToDecimalOrNull(this object data, int digits)
        {
            decimal? num = data.ToDecimalOrNull();
            if (!num.HasValue)
            {
                return null;
            }

            return Math.Round(num.Value, digits, MidpointRounding.AwayFromZero);
        }

        public static DateTime ToDate(this object data)
        {
            try
            {
                if (data == null)
                {
                    return DateTime.MinValue;
                }

                if (Regex.IsMatch(data.ToStringExt(), "^\\d{8}$"))
                {
                    string text = data.ToStringExt();
                    return new DateTime(text.Substring(0, 4).ToInt(), text.Substring(4, 2).ToInt(), text.Substring(6, 2).ToInt());
                }

                DateTime result;
                return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ToDateOrNull(this object data)
        {
            try
            {
                if (data == null)
                {
                    return null;
                }

                if (Regex.IsMatch(data.ToStringExt(), "^\\d{8}$"))
                {
                    string text = data.ToStringExt();
                    return new DateTime(text.Substring(0, 4).ToInt(), text.Substring(4, 2).ToInt(), text.Substring(6, 2).ToInt());
                }

                if (DateTime.TryParse(data.ToString(), out var result))
                {
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public static bool ToBool(this object data)
        {
            if (data == null)
            {
                return false;
            }

            bool? @bool = data.GetBool();
            if (@bool.HasValue)
            {
                return @bool.Value;
            }

            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        private static bool? GetBool(this object data)
        {
            return data.ToString().Trim().ToLower() switch
            {
                "0" => false,
                "1" => true,
                "是" => true,
                "否" => false,
                "yes" => true,
                "no" => false,
                _ => null,
            };
        }

        public static bool? ToBoolOrNull(this object data)
        {
            if (data == null)
            {
                return null;
            }

            bool? @bool = data.GetBool();
            if (@bool.HasValue)
            {
                return @bool.Value;
            }

            if (bool.TryParse(data.ToString(), out var result))
            {
                return result;
            }

            return null;
        }

        //
        // 摘要:
        //     把对象转换成 protobuf 字节组
        //     注意：对象类必须严格遵循 protobuf-net 的定义，在每个属性与类名上方添加对应特性
        //
        // 参数:
        //   obj:
        //     对象
        //
        // 类型参数:
        //   T:
        //     protobuf的对象类
        public static byte[] ToProtobuf<T>(this T obj)
        {
            using MemoryStream memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        //
        // 摘要:
        //     protobuf 字节组 转换成对象
        //     注意：对象类必须严格遵循 protobuf-net 的定义，在每个属性与类名上方添加对应特性
        //
        // 参数:
        //   protoBuf:
        //     字节组
        public static T ToProtobufEntity<T>(this byte[] protoBuf)
        {
            using MemoryStream source = new MemoryStream(protoBuf);
            return Serializer.Deserialize<T>(source);
        }

        //
        // 摘要:
        //     转换JSON
        //
        // 参数:
        //   obj:
        //     对象
        //
        //   formatting:
        //     是否需要格式化
        public static string? ToJson<T>(this T obj, bool formatting = false)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = formatting,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(obj, options);
        }

        //
        // 摘要:
        //     JSON格式化
        //
        // 参数:
        //   json:
        //     Json 字符串
        public static string JsonFormatting(this string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(json, options), options);
        }

        //
        // 摘要:
        //     JSON转实体
        //
        // 参数:
        //   json:
        //     Json字符串
        //
        // 类型参数:
        //   T:
        //     对象
        public static T? ToJsonEntity<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        //
        // 摘要:
        //     实体对象序列化成xml字符串
        //
        // 参数:
        //   obj:
        //     对象
        //
        // 类型参数:
        //   T:
        //     对象
        //
        // 返回结果:
        //     XM字符串
        public static string? ToXml<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            using MemoryStream memoryStream = new MemoryStream();
            new XmlSerializer(obj.GetType()).Serialize(memoryStream, obj);
            memoryStream.Position = 0L;
            return new StreamReader(memoryStream).ReadToEnd();
        }

        //
        // 摘要:
        //     反序列化xml字符为对象，默认为Utf-8编码
        //
        // 参数:
        //   xml:
        //     xml 字符串
        //
        // 类型参数:
        //   T:
        //     对象
        public static T? ToXmlEntity<T>(this string xml)
        {
            return xml.ToXmlEntity<T>(Encoding.UTF8);
        }

        //
        // 摘要:
        //     反序列化xml字符为对象
        //
        // 参数:
        //   xml:
        //     xml字符串
        //
        //   encoding:
        //     编码格式
        //
        // 类型参数:
        //   T:
        //     对象
        public static T? ToXmlEntity<T>(this string xml, Encoding encoding)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using MemoryStream stream = new MemoryStream(encoding.GetBytes(xml));
                using StreamReader textReader = new StreamReader(stream, encoding);
                return (T)xmlSerializer.Deserialize(textReader);
            }
            catch
            {
                return default(T);
            }
        }

        //
        // 摘要:
        //     安全获取可空值类型的实际值或默认值（若为 null）
        //
        // 参数:
        //   value:
        //     可空值类型实例（例如 int?）
        //
        // 类型参数:
        //   T:
        //     值类型（如 int、DateTime 等）
        //
        // 返回结果:
        //     当 value 不为 null 时返回其实际值； 当 value 为 null 时返回 T 的默认值
        //
        // 言论：
        //     注意： - 默认值可能不符合业务逻辑（如 0 可能不是有效的年龄值） - 仅适用于值类型（不可用于 string 等引用类型）
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value.GetValueOrDefault();
        }

        //
        // 摘要:
        //     读取Json文件内容为集合
        //
        // 参数:
        //   filePath:
        //     文件地址
        //
        // 类型参数:
        //   T:
        public static async Task<List<T>> GetListAsync<T>(this string filePath)
        {
            return JsonSerializer.Deserialize<List<T>>(await filePath.ReadAsync());
        }

        //
        // 摘要:
        //     读取Json文件内容为实体
        //
        // 参数:
        //   filePath:
        //     文件地址
        //
        // 类型参数:
        //   T:
        public static async Task<T> GetModelAsync<T>(this string filePath)
        {
            return JsonSerializer.Deserialize<T>(await filePath.ReadAsync());
        }

        //
        // 摘要:
        //     将数据写入到文件
        //
        // 参数:
        //   filePath:
        //     文件地址
        //
        //   data:
        //     数据
        //
        // 类型参数:
        //   T:
        //     实体信息
        public static async Task WriteDataAsync<T>(this string filePath, List<T> data)
        {
            if (data != null)
            {
                string content = JsonSerializer.Serialize(data);
                await filePath.WriteAsync(content);
            }
        }

        //
        // 摘要:
        //     将数据写入到文件
        //
        // 参数:
        //   filePath:
        //     文件地址
        //
        //   data:
        //     数据
        //
        // 类型参数:
        //   T:
        //     实体信息
        public static async Task WriteDataAsync<T>(this string filePath, T data)
        {
            if (data != null)
            {
                string content = JsonSerializer.Serialize(data);
                await filePath.WriteAsync(content);
            }
        }

        //
        // 摘要:
        //     写入文件
        //
        // 参数:
        //   filePath:
        //     文件地址
        //
        //   content:
        //     写入内容
        private static async Task WriteAsync(this string filePath, string content)
        {
            filePath = Directory.GetCurrentDirectory() + filePath;
            using StreamWriter stream = new StreamWriter(filePath, append: false, Encoding.UTF8);
            await stream.WriteAsync(content);
            await stream.FlushAsync();
        }

        //
        // 摘要:
        //     读取文件
        //
        // 参数:
        //   filePath:
        //     文件地址
        private static async Task<string> ReadAsync(this string filePath)
        {
            if (!filePath.Contains(":\\"))
            {
                filePath = Directory.GetCurrentDirectory() + filePath;
            }

            using StreamReader stream = new StreamReader(filePath, Encoding.UTF8);
            return await stream.ReadToEndAsync();
        }

        //
        // 摘要:
        //     string转ushort数组
        //
        // 参数:
        //   inString:
        public static ushort[] ToUshort(this string inString)
        {
            if (inString.Length % 2 == 1)
            {
                inString += " ";
            }

            char[] array = inString.ToCharArray();
            _ = new byte[array.Length];
            byte[] array2 = new byte[2];
            ushort[] array3 = new ushort[array.Length / 2];
            int num = 0;
            int num2 = 0;
            while (num < array.Length)
            {
                array2[0] = BitConverter.GetBytes(array[num])[0];
                array2[1] = BitConverter.GetBytes(array[num + 1])[0];
                array3[num2] = BitConverter.ToUInt16(array2, 0);
                num += 2;
                num2++;
            }

            return array3;
        }

        //
        // 摘要:
        //     int转ushort
        //
        // 参数:
        //   inInt:
        //     int 数据
        public static ushort ToUshort(this int inInt)
        {
            return (ushort)inInt;
        }

        //
        // 摘要:
        //     double转ushort
        //
        // 参数:
        //   inDouble:
        //     double 数据
        public static ushort ToUshort(this double inDouble)
        {
            return (ushort)inDouble;
        }

        //
        // 摘要:
        //     float转ushort
        //
        // 参数:
        //   inFloat:
        //     float 数据
        public static ushort ToUshort(this float inFloat)
        {
            return (ushort)inFloat;
        }

        //
        // 摘要:
        //     ushort数组转string
        //
        // 参数:
        //   inUshort:
        //     ushort 数据
        public static string ToString(this ushort[] inUshort)
        {
            byte[] array = new byte[inUshort.Length * 2];
            for (int i = 0; i < inUshort.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(inUshort[i]);
                array[i * 2] = bytes[0];
                array[i * 2 + 1] = bytes[1];
            }

            return Encoding.ASCII.GetString(array).Trim();
        }

        //
        // 摘要:
        //     获取西门子PLC字符串数组--String
        //
        // 参数:
        //   str:
        //     字符串
        public static byte[] GetString(this string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return new byte[2]
            {
            Convert.ToByte(254),
            Convert.ToByte(str.Length)
            }.Concat(bytes).ToArray();
        }

        //
        // 摘要:
        //     字节数组转16进制字符
        //
        // 参数:
        //   bytes:
        //     字节数组
        public static string ToHexString(this byte[] bytes)
        {
            return string.Join(" ", bytes.Select((byte t) => t.ToString("X2")));
        }

        public static string ToHexString(this byte[] InBytes, char segment)
        {
            return ByteHandler.ByteToHexString(InBytes, segment);
        }

        public static string ToHexString(this byte[] InBytes, char segment, int newLineCount, string format = "{0:X2}")
        {
            return ByteHandler.ByteToHexString(InBytes, segment, newLineCount, format);
        }

        public static byte[] ToHexBytes(this string value)
        {
            return ByteHandler.HexStringToBytes(value);
        }

        public static byte[] ToByteArray(this bool[] array)
        {
            return ByteHandler.BoolArrayToByte(array);
        }

        public static bool[] ToBoolArray(this byte[] InBytes, int length)
        {
            return ByteHandler.ByteToBoolArray(InBytes, length);
        }

        //
        // 摘要:
        //     获取当前数组的倒序数组，这是一个新的实例，不改变原来的数组值
        //
        // 参数:
        //   value:
        //     输入的原始数组
        //
        // 返回结果:
        //     反转之后的数组信息
        public static T[] ReverseNew<T>(this T[] value)
        {
            T[] array = value.CopyArray();
            Array.Reverse(array);
            return array;
        }

        public static bool[] ToBoolArray(this byte[] InBytes)
        {
            return ByteHandler.ByteToBoolArray(InBytes);
        }

        //
        // 摘要:
        //     获取Byte数组的第 bytIndex 个位置的，boolIndex偏移的bool值
        //
        // 参数:
        //   bytes:
        //     字节数组信息
        //
        //   bytIndex:
        //     字节的偏移位置
        //
        //   boolIndex:
        //     指定字节的位偏移
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolValue(this byte[] bytes, int bytIndex, int boolIndex)
        {
            return ByteHandler.BoolOnByteIndex(bytes[bytIndex], boolIndex);
        }

        //
        // 摘要:
        //     获取Byte数组的第 boolIndex 偏移的bool值，这个偏移值可以为 10，就是第 1 个字节的 第3位
        //
        // 参数:
        //   bytes:
        //     字节数组信息
        //
        //   boolIndex:
        //     指定字节的位偏移
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this byte[] bytes, int boolIndex)
        {
            return ByteHandler.BoolOnByteIndex(bytes[boolIndex / 8], boolIndex % 8);
        }

        //
        // 摘要:
        //     获取Byte的第 boolIndex 偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   byt:
        //     字节信息
        //
        //   boolIndex:
        //     指定字节的位偏移
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this byte byt, int boolIndex)
        {
            return ByteHandler.BoolOnByteIndex(byt, boolIndex % 8);
        }

        //
        // 摘要:
        //     获取short类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     short数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this short value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     获取ushort类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     ushort数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this ushort value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     获取int类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     int数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this int value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     获取uint类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     uint数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this uint value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     获取long类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     long数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this long value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     获取ulong类型数据的第 boolIndex (从0起始)偏移的bool值，比如3，就是第4位
        //
        // 参数:
        //   value:
        //     ulong数据值
        //
        //   boolIndex:
        //     位偏移索引，从0开始
        //
        // 返回结果:
        //     bool值
        public static bool GetBoolByIndex(this ulong value, int boolIndex)
        {
            return BitConverter.GetBytes(value).GetBoolByIndex(boolIndex);
        }

        //
        // 摘要:
        //     从字节数组里提取字符串数据，如果碰到0x00字节，就直接结束
        //
        // 参数:
        //   buffer:
        //     原始字节信息
        //
        //   index:
        //     起始的偏移地址
        //
        //   length:
        //     字节长度信息
        //
        //   encoding:
        //     编码
        //
        // 返回结果:
        //     字符串信息
        public static string GetStringOrEndChar(this byte[] buffer, int index, int length, Encoding encoding)
        {
            for (int i = index; i < index + length; i++)
            {
                if (buffer[i] == 0)
                {
                    length = i - index;
                    break;
                }
            }

            return Encoding.UTF8.GetString(buffer, index, length);
        }

        //
        // 摘要:
        //     设置Byte的第 boolIndex 位的bool值，可以强制为 true 或是 false, 不影响其他的位
        //
        // 参数:
        //   byt:
        //     字节信息
        //
        //   boolIndex:
        //     指定字节的位偏移
        //
        //   value:
        //     bool的值
        //
        // 返回结果:
        //     修改之后的byte值
        public static byte SetBoolByIndex(this byte byt, int boolIndex, bool value)
        {
            return ByteHandler.SetBoolOnByteIndex(byt, boolIndex, value);
        }

        //
        // 摘要:
        //     设置Byte[]的第 boolIndex 位的bool值，可以强制为 true 或是 false, 不影响其他的位，如果是第 10 位，则表示第 1 个字节的第
        //     2 位（都是从 0 地址开始算的）
        //
        // 参数:
        //   buffer:
        //     字节数组信息
        //
        //   boolIndex:
        //     位偏移的索引
        //
        //   value:
        //     bool的值
        public static void SetBoolByIndex(this byte[] buffer, int boolIndex, bool value)
        {
            buffer[boolIndex / 8] = buffer[boolIndex / 8].SetBoolByIndex(boolIndex % 8, value);
        }

        //
        // 摘要:
        //     修改short数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~15，之外的值会引发异常
        //
        // 参数:
        //   shortValue:
        //     等待修改的short值
        //
        //   boolIndex:
        //     位索引，位索引为 0~15，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的short值
        public static short SetBoolByIndex(this short shortValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(shortValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToInt16(bytes, 0);
        }

        //
        // 摘要:
        //     修改ushort数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~15，之外的值会引发异常
        //
        // 参数:
        //   ushortValue:
        //     等待修改的ushort值
        //
        //   boolIndex:
        //     位索引，位索引为 0~15，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的ushort值
        public static ushort SetBoolByIndex(this ushort ushortValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(ushortValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToUInt16(bytes, 0);
        }

        //
        // 摘要:
        //     修改int数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~31，之外的值会引发异常
        //
        // 参数:
        //   intValue:
        //     等待修改的int值
        //
        //   boolIndex:
        //     位索引，位索引为 0~31，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的int值
        public static int SetBoolByIndex(this int intValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(intValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToInt32(bytes, 0);
        }

        //
        // 摘要:
        //     修改uint数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~31，之外的值会引发异常
        //
        // 参数:
        //   uintValue:
        //     等待修改的uint值
        //
        //   boolIndex:
        //     位索引，位索引为 0~31，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的uint值
        public static uint SetBoolByIndex(this uint uintValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(uintValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToUInt32(bytes, 0);
        }

        //
        // 摘要:
        //     修改long数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~63，之外的值会引发异常
        //
        // 参数:
        //   longValue:
        //     等待修改的long值
        //
        //   boolIndex:
        //     位索引，位索引为 0~63，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的long值
        public static long SetBoolByIndex(this long longValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(longValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToInt64(bytes, 0);
        }

        //
        // 摘要:
        //     修改ulong数据的某个位，并且返回修改后的值，不影响原来的值。位索引为 0~63，之外的值会引发异常
        //
        // 参数:
        //   ulongValue:
        //     等待修改的ulong值
        //
        //   boolIndex:
        //     位索引，位索引为 0~63，之外的值会引发异常
        //
        //   value:
        //     bool值
        //
        // 返回结果:
        //     修改之后的ulong值
        public static ulong SetBoolByIndex(this ulong ulongValue, int boolIndex, bool value)
        {
            byte[] bytes = BitConverter.GetBytes(ulongValue);
            bytes.SetBoolByIndex(boolIndex, value);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static T[] RemoveDouble<T>(this T[] value, int leftLength, int rightLength)
        {
            return ByteHandler.ArrayRemoveDouble(value, leftLength, rightLength);
        }

        public static T[] RemoveBegin<T>(this T[] value, int length)
        {
            return ByteHandler.ArrayRemoveBegin(value, length);
        }

        public static T[] RemoveLast<T>(this T[] value, int length)
        {
            return ByteHandler.ArrayRemoveLast(value, length);
        }

        public static T[] SelectMiddle<T>(this T[] value, int index, int length)
        {
            return ByteHandler.ArraySelectMiddle(value, index, length);
        }

        public static T[] SelectBegin<T>(this T[] value, int length)
        {
            return ByteHandler.ArraySelectBegin(value, length);
        }

        public static T[] SelectLast<T>(this T[] value, int length)
        {
            return ByteHandler.ArraySelectLast(value, length);
        }

        public static T[] SpliceArray<T>(this T[] value, params T[][] arrays)
        {
            List<T[]> list = new List<T[]>(arrays.Length + 1);
            list.Add(value);
            list.AddRange(arrays);
            return ByteHandler.SpliceArray(list.ToArray());
        }

        //
        // 摘要:
        //     移除指定字符串数据的最后 length 个字符。如果字符串本身的长度不足 length，则返回为空字符串。 an empty string is returned.
        //
        //
        // 参数:
        //   value:
        //     等待操作的字符串数据
        //
        //   length:
        //     准备移除的长度信息
        //
        // 返回结果:
        //     移除之后的数据信息
        public static string RemoveLast(this string value, int length)
        {
            if (value == null)
            {
                return null;
            }

            if (value.Length < length)
            {
                return string.Empty;
            }

            return value.Remove(value.Length - length);
        }

        //
        // 摘要:
        //     将指定的数据添加到数组的每个元素上去，会改变每个元素的值
        //
        // 参数:
        //   array:
        //     原始数组
        //
        //   value:
        //     值
        //
        // 返回结果:
        //     修改后的数组信息
        public static byte[] EveryByteAdd(this byte[] array, int value)
        {
            if (array == null)
            {
                return null;
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(array[i] + value);
            }

            return array;
        }

        public static string ToArrayString<T>(this T[] value)
        {
            return ByteHandler.ArrayFormat(value);
        }

        public static string ToArrayString<T>(this T[] value, string format)
        {
            return ByteHandler.ArrayFormat(value, format);
        }

        //
        // 摘要:
        //     将字符串数组转换为实际的数据数组。例如字符串格式[1,2,3,4,5]，可以转成实际的数组对象
        //
        // 参数:
        //   value:
        //     字符串数据
        //
        //   selector:
        //     转换方法
        //
        // 类型参数:
        //   T:
        //     类型对象
        //
        // 返回结果:
        //     实际的数组
        public static T[] ToStringArray<T>(this string value, Func<string, T> selector)
        {
            if (value.IndexOf('[') >= 0)
            {
                value = value.Replace("[", "");
            }

            if (value.IndexOf(']') >= 0)
            {
                value = value.Replace("]", "");
            }

            return value.Split(new char[2] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(selector).ToArray();
        }

        //
        // 摘要:
        //     将字符串数组转换为实际的数据数组。支持byte,sbyte,bool,short,ushort,int,uint,long,ulong,float,double，使用默认的十进制，例如字符串格式[1,2,3,4,5]，可以转成实际的数组对象
        //
        //
        // 参数:
        //   value:
        //     字符串数据
        //
        // 类型参数:
        //   T:
        //     类型对象
        //
        // 返回结果:
        //     实际的数组
        public static T[] ToStringArray<T>(this string value)
        {
            Type typeFromHandle = typeof(T);
            if (typeFromHandle == typeof(byte))
            {
                return (T[])(object)value.ToStringArray(byte.Parse);
            }

            if (typeFromHandle == typeof(sbyte))
            {
                return (T[])(object)value.ToStringArray(sbyte.Parse);
            }

            if (typeFromHandle == typeof(bool))
            {
                return (T[])(object)value.ToStringArray(bool.Parse);
            }

            if (typeFromHandle == typeof(short))
            {
                return (T[])(object)value.ToStringArray(short.Parse);
            }

            if (typeFromHandle == typeof(ushort))
            {
                return (T[])(object)value.ToStringArray(ushort.Parse);
            }

            if (typeFromHandle == typeof(int))
            {
                return (T[])(object)value.ToStringArray(int.Parse);
            }

            if (typeFromHandle == typeof(uint))
            {
                return (T[])(object)value.ToStringArray(uint.Parse);
            }

            if (typeFromHandle == typeof(long))
            {
                return (T[])(object)value.ToStringArray(long.Parse);
            }

            if (typeFromHandle == typeof(ulong))
            {
                return (T[])(object)value.ToStringArray(ulong.Parse);
            }

            if (typeFromHandle == typeof(float))
            {
                return (T[])(object)value.ToStringArray(float.Parse);
            }

            if (typeFromHandle == typeof(double))
            {
                return (T[])(object)value.ToStringArray(double.Parse);
            }

            if (typeFromHandle == typeof(DateTime))
            {
                return (T[])(object)value.ToStringArray(DateTime.Parse);
            }

            if (typeFromHandle == typeof(Guid))
            {
                return (T[])(object)value.ToStringArray(Guid.Parse);
            }

            if (typeFromHandle == typeof(string))
            {
                return (T[])(object)value.ToStringArray((string m) => m);
            }

            throw new Exception("use ToArray<T>(Func<string,T>) method instead");
        }

        //
        // 摘要:
        //     根据英文小数点进行切割字符串，去除空白的字符
        //
        // 参数:
        //   str:
        //     字符串本身
        //
        // 返回结果:
        //     切割好的字符串数组，例如输入 "100.5"，返回 "100", "5"
        public static string[] SplitDot(this string str)
        {
            return str.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        }

        //
        // 摘要:
        //     写入
        //
        // 参数:
        //   ms:
        //     流对象
        //
        //   buffer:
        //     字节
        public static void Write(this MemoryStream ms, byte[] buffer)
        {
            if (buffer != null)
            {
                ms.Write(buffer, 0, buffer.Length);
            }
        }

        //
        // 摘要:
        //     将System.UInt16数据写入到字节流，字节顺序为相反
        //
        // 参数:
        //   ms:
        //     字节流
        //
        //   value:
        //     等待写入的值
        public static void WriteReverse(this MemoryStream ms, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte b = bytes[0];
            bytes[0] = bytes[1];
            bytes[1] = b;
            ms.Write(bytes);
        }

        //
        // 摘要:
        //     根据指定的字节长度信息，获取到随机的字节信息
        //
        // 参数:
        //   random:
        //     随机数对象
        //
        //   length:
        //     字节的长度信息
        //
        // 返回结果:
        //     原始字节数组
        public static byte[] GetBytes(this Random random, int length)
        {
            byte[] array = new byte[length];
            random.NextBytes(array);
            return array;
        }

        //
        // 摘要:
        //     将byte数组按照双字节进行反转，如果为单数的情况，则自动补齐
        //
        // 参数:
        //   inBytes:
        //     输入的字节信息
        //
        // 返回结果:
        //     反转后的数据
        //
        // 言论：
        //     例如传入的字节数据是 01 02 03 04, 那么反转后就是 02 01 04 03
        public static byte[] ReverseByWord(this byte[] inBytes)
        {
            return ByteHandler.BytesReverseByWord(inBytes);
        }

        //
        // 摘要:
        //     16进制字符串转字节数组
        //
        // 参数:
        //   str:
        //     字符串
        //
        //   strict:
        //     严格模式（严格按两个字母间隔一个空格）
        public static byte[] ToHex(this string str, bool strict = true)
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

        //
        // 摘要:
        //     字节转字符串
        //
        // 参数:
        //   bytes:
        //     字节数据
        //
        //   encoding:
        //     编码格式
        //
        // 返回结果:
        //     字符串
        public static string HexToStr(this byte[] bytes, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }

            return encoding.GetString(bytes);
        }

        //
        // 摘要:
        //     字符串转字节
        //
        // 参数:
        //   strings:
        //     字符串
        //
        //   encoding:
        //     编码格式
        //
        // 返回结果:
        //     字节
        public static byte[] StrToHex(this string strings, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.ASCII;
            }

            return encoding.GetBytes(strings);
        }

        //
        // 摘要:
        //     动态排序扩展
        //
        // 参数:
        //   source:
        //     数据源
        //
        //   propertyName:
        //     属性名
        //
        //   ascending:
        //     true升序/false倒序
        //
        // 类型参数:
        //   T:
        //
        // 异常:
        //   T:System.ArgumentException:
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            LambdaExpression lambdaExpression = Expression.Lambda(Expression.MakeMemberAccess(parameterExpression, property), parameterExpression);
            string methodName = (ascending ? "OrderBy" : "OrderByDescending");
            MethodCallExpression expression = Expression.Call(typeof(Queryable), methodName, new Type[2]
            {
            typeof(T),
            property.PropertyType
            }, source.Expression, lambdaExpression);
            return source.Provider.CreateQuery<T>(expression);
        }

        //
        // 摘要:
        //     获取枚举字段上指定类型的特性。
        //
        // 参数:
        //   enumValue:
        //     枚举值。
        //
        // 类型参数:
        //   T:
        //     特性的类型，必须继承自 Attribute。
        //
        // 返回结果:
        //     特性对象，如果未找到则返回 null。
        public static T? GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            try
            {
                FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());
                if (field == null)
                {
                    return null;
                }

                return field.GetCustomAttribute<T>(inherit: false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 摘要:
        //     获取类型上指定类型的特性。
        //
        // 参数:
        //   type:
        //     类型对象。
        //
        // 类型参数:
        //   T:
        //     特性类型，必须继承自 Attribute。
        //
        // 返回结果:
        //     特性对象，如果未找到则返回 null。
        public static T? GetAttribute<T>(this Type type) where T : Attribute
        {
            try
            {
                return type.GetCustomAttribute<T>(inherit: false);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //
        // 摘要:
        //     AES加密
        //
        // 参数:
        //   plainText:
        //     加密内容
        //
        //   key:
        //     密钥
        //
        //   iv:
        //     向量,必须16个字节
        public static string AESEncrypt(this string plainText, string? key = null, string? iv = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = "sDhibwYX7EkqG6b6GfcO3/ft1FBeYb+Gdml4lhKXJ/o=";
            }

            if (string.IsNullOrWhiteSpace(iv))
            {
                iv = "zRDPEQHreJ2d7tANOdGoIQ==";
            }

            using Aes aes = Aes.Create();
            aes.Key = GetAssignKey(key);
            aes.IV = GetAssignBytes(iv, 16);
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            foreach (byte b in array)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }

            return stringBuilder.ToString();
        }

        //
        // 摘要:
        //     AES 解密
        //
        // 参数:
        //   cipherText:
        //     加密后的内容
        //
        //   key:
        //     密钥
        //
        //   iv:
        //     向量,必须16个字节
        public static string AESDecrypt(this string cipherText, string? key = null, string? iv = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                key = "sDhibwYX7EkqG6b6GfcO3/ft1FBeYb+Gdml4lhKXJ/o=";
            }

            if (string.IsNullOrWhiteSpace(iv))
            {
                iv = "zRDPEQHreJ2d7tANOdGoIQ==";
            }

            using Aes aes = Aes.Create();
            aes.Key = GetAssignKey(key);
            aes.IV = GetAssignBytes(iv, 16);
            byte[] array = new byte[cipherText.Length / 2];
            for (int i = 0; i < cipherText.Length / 2; i++)
            {
                int num = Convert.ToInt32(cipherText.Substring(i * 2, 2), 16);
                array[i] = (byte)num;
            }

            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        //
        // 摘要:
        //     不足自动补齐, 超过自动截取
        //
        // 参数:
        //   key:
        //     密钥
        //
        //   count:
        //     长度
        private static byte[] GetAssignBytes(string key, int count)
        {
            byte[] array = Encoding.UTF8.GetBytes(key);
            if (array.Length < count)
            {
                Array.Resize(ref array, count);
            }
            else if (array.Length > count)
            {
                Array.Resize(ref array, count);
            }

            return array;
        }

        //
        // 摘要:
        //     使用 SHA256 生成 32 字节的密钥
        //
        // 参数:
        //   key:
        //     密钥
        private static byte[] GetAssignKey(string key)
        {
            using SHA256 sHA = SHA256.Create();
            return sHA.ComputeHash(Encoding.UTF8.GetBytes(key));
        }

        //
        // 摘要:
        //     通用模糊查询扩展方法
        //
        // 参数:
        //   query:
        //     当前查询对象
        //
        //   content:
        //     模糊查询的内容
        //
        //   propertyNames:
        //     需要模糊查询的属性名数组
        //
        // 类型参数:
        //   T:
        //     实体类型
        //
        // 返回结果:
        //     经过模糊查询过滤后的查询对象
        public static IQueryable<T> ApplyFuzzySearch<T>(this IQueryable<T> query, string content, params string[] propertyNames)
        {
            if (string.IsNullOrWhiteSpace(content) || propertyNames == null || !propertyNames.Any())
            {
                return query;
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            Expression expression = null;
            foreach (string propertyOrFieldName in propertyNames)
            {
                MethodCallExpression instance = Expression.Call(Expression.PropertyOrField(parameterExpression, propertyOrFieldName), "ToString", null);
                MethodInfo method = typeof(string).GetMethod("Contains", new Type[1] { typeof(string) });
                MethodCallExpression methodCallExpression = Expression.Call(instance, method, Expression.Constant(content));
                expression = ((expression == null) ? ((Expression)methodCallExpression) : ((Expression)Expression.OrElse(expression, methodCallExpression)));
            }

            Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[1] { parameterExpression });
            return query.Where(predicate);
        }

        //
        // 摘要:
        //     多条件查询
        //
        // 参数:
        //   source:
        //     源
        //
        //   filters:
        //     键值对应的字典
        //
        // 类型参数:
        //   T:
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> source, Dictionary<string, object> filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }

            IQueryable<T> queryable = source;
            foreach (KeyValuePair<string, object> filter in filters)
            {
                string key = filter.Key;
                object obj = filter.Value;
                if (obj != null)
                {
                    if (obj is JsonElement jsonElement)
                    {
                        obj = ConvertJsonElement(jsonElement);
                    }

                    if (obj is string value)
                    {
                        queryable = queryable.Where(CreateContainsExpression<T>(key, value));
                    }
                    else if (obj is DateTime || obj is DateTime?)
                    {
                        queryable = queryable.Where(CreateDateEqualExpression<T>(key, (DateTime?)obj));
                    }
                    else if (obj is int || obj is int?)
                    {
                        queryable = queryable.Where(CreateEqualExpression<T>(key, (int?)obj));
                    }
                    else if (obj is double || obj is double?)
                    {
                        queryable = queryable.Where(CreateEqualExpression<T>(key, (double?)obj));
                    }
                    else if (obj is bool || obj is bool?)
                    {
                        queryable = queryable.Where(CreateEqualExpression<T>(key, (bool?)obj));
                    }
                    else if (obj is IEnumerable<object> values)
                    {
                        queryable = queryable.Where(CreateInExpression<T>(key, values));
                    }
                }
            }

            return queryable;
        }

        private static object ConvertJsonElement(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    {
                        if (DateTime.TryParse(jsonElement.GetString(), out var result))
                        {
                            return result;
                        }

                        return jsonElement.GetString();
                    }
                case JsonValueKind.Number:
                    {
                        if (jsonElement.TryGetInt32(out var value))
                        {
                            return value;
                        }

                        if (jsonElement.TryGetDouble(out var value2))
                        {
                            return value2;
                        }

                        break;
                    }
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return jsonElement.GetBoolean();
            }

            throw new NotSupportedException($"Unsupported JsonElement ValueKind: {jsonElement.ValueKind}");
        }

        private static Expression<Func<T, bool>> CreateContainsExpression<T>(string propertyName, string value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression instance = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new Type[1] { typeof(string) });
            return Expression.Lambda<Func<T, bool>>(Expression.Call(instance, method, Expression.Constant(value)), new ParameterExpression[1] { parameterExpression });
        }

        private static Expression<Func<T, bool>> CreateEqualExpression<T>(string propertyName, object value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyName);
            Type type = memberExpression.Type;
            if (type.IsNullable())
            {
                Type underlyingType = Nullable.GetUnderlyingType(type);
                ConstantExpression right = Expression.Constant(Convert.ChangeType(value, underlyingType), type);
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(memberExpression, right), new ParameterExpression[1] { parameterExpression });
            }

            ConstantExpression right2 = Expression.Constant(value, type);
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(memberExpression, right2), new ParameterExpression[1] { parameterExpression });
        }

        private static Expression<Func<T, bool>> CreateDateEqualExpression<T>(string propertyName, DateTime? value)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression left = Expression.Property(parameterExpression, propertyName);
            if (value.HasValue)
            {
                ConstantExpression right = Expression.Constant(value.Value, typeof(DateTime?));
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(left, right), new ParameterExpression[1] { parameterExpression });
            }

            return (T x) => true;
        }

        private static Expression<Func<T, bool>> CreateInExpression<T>(string propertyName, IEnumerable<object> values)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyName);
            ConstantExpression arg = Expression.Constant(values);
            return Expression.Lambda<Func<T, bool>>(Expression.Call(typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "Contains" && m.GetParameters().Length == 2).MakeGenericMethod(memberExpression.Type), arg, memberExpression), new ParameterExpression[1] { parameterExpression });
        }

        private static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        //
        // 摘要:
        //     时间范围查询
        //
        // 参数:
        //   source:
        //     源
        //
        //   propertyName:
        //     属性名
        //
        //   startDate:
        //     开始时间
        //
        //   endDate:
        //     结束时间
        //
        // 类型参数:
        //   T:
        //
        // 异常:
        //   T:System.ArgumentNullException:
        public static IQueryable<T> ApplyDateRangeFilter<T>(this IQueryable<T> source, string propertyName, DateTime? startDate, DateTime? endDate)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyName);
            Expression expression = Expression.Constant(true);
            if (startDate.HasValue)
            {
                if (memberExpression.Type != typeof(DateTime?) && memberExpression.Type != typeof(DateTime))
                {
                    throw new InvalidOperationException("Property '" + propertyName + "' is not of type DateTime or DateTime?");
                }

                BinaryExpression right = Expression.GreaterThanOrEqual(memberExpression, Expression.Convert(Expression.Constant(startDate.Value), typeof(DateTime?)));
                expression = Expression.AndAlso(expression, right);
            }

            if (endDate.HasValue)
            {
                if (memberExpression.Type != typeof(DateTime?) && memberExpression.Type != typeof(DateTime))
                {
                    throw new InvalidOperationException("Property '" + propertyName + "' is not of type DateTime or DateTime?");
                }

                BinaryExpression right2 = Expression.LessThanOrEqual(memberExpression, Expression.Convert(Expression.Constant(endDate.Value), typeof(DateTime?)));
                expression = Expression.AndAlso(expression, right2);
            }

            Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[1] { parameterExpression });
            return source.Where(predicate);
        }

        //
        // 摘要:
        //     设置缓存
        //
        // 参数:
        //   key:
        //     健
        //
        //   value:
        //     值
        //
        //   expirationTime:
        //     过期时间,为空默认60分钟
        //
        // 类型参数:
        //   T:
        //     缓存对象
        //
        // 返回结果:
        //     设置状态
        public static bool SetCache<T>(this string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = (expirationTime ?? TimeSpan.FromMinutes(60L))
                };
                memoryCache.Set(key, value, options);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //
        // 摘要:
        //     获取缓存
        //
        // 参数:
        //   key:
        //     健
        //
        // 类型参数:
        //   T:
        //     缓存对象
        //
        // 返回结果:
        //     缓存对象
        public static T? GetCache<T>(this string key)
        {
            try
            {
                if (memoryCache.TryGetValue<T>(key, out T value))
                {
                    return value;
                }

                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        //
        // 摘要:
        //     移除缓存
        //
        // 参数:
        //   key:
        //     健
        //
        // 返回结果:
        //     返回状态
        public static bool RemoveCache(this string key)
        {
            try
            {
                memoryCache.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //
        // 摘要:
        //     清空缓存
        public static bool ClearCache()
        {
            try
            {
                memoryCache.Clear();
                memoryCache.Dispose();
                memoryCache = new MemoryCache(new MemoryCacheOptions());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// -------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// -------------------------------------------------------------------------------------------------------------
        public static string GetNumber(this string s)
        {
            var sb = new StringBuilder();

            foreach (var c in s)
            {
                // 有效数字
                if (c >= '0' && c <= '9')
                    sb.Append(c);
                //  有效符号
                else if (c == '.' || c == '-' || c == '+' || c == ' ' || c == '\t' || c == '\r' || c == '\n')
                    sb.Append(c);
                //  无效数字
                else
                    break;
            }

            return sb.ToString();
        }
    }
}
