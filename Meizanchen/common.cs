using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Meizanchen
{
    /// <summary>
    /// 数据验证类库
    /// </summary>
    public class DataValidator
    {

        /// <summary>
        /// 整数验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 实数验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimal(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, "^[0-9]+[.]?[0-9]+$");
        }

        /// <summary>
        /// 带符号的实数验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDecimalSign(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, "^[+-]?[0-9]+[.]?[0-9]+$");
        }

        /// <summary>
        /// 正整数验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, "^[0-9]+$");
        }

        /// <summary>
        /// 整数验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumberSign(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, "^[+-]?[0-9]+$");
        }

        /// <summary>
        /// 邮编验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPostCode(string input)
        {
            return (IsNumber(input) && (input.Length == 6));
        }

        /// <summary>
        /// 邮箱地址格式验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^/w+([-+.']/w+)*@/w+([-.]/w+)*/./w+([-.]/w+)*$");
        }

        /// <summary>
        /// IP地址验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIP(string input)
        {
            return (!string.IsNullOrEmpty(input) && Regex.IsMatch(input.Trim(), @"^(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])$"));
        }

        /// <summary>
        /// URL验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            return !string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)? ");
        }

        /// <summary>
        /// 区号验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAreaCode(string input)
        {
            return ((IsNumber(input) && (input.Length >= 3)) && (input.Length <= 5));
        }

        /// <summary>
        /// 用户名格式验证,长度[0,20],不能含有"[]:|<>+=;,?*@
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            bool res = !string.IsNullOrEmpty(userName) && userName.Length < 20 && userName.Trim().Length != 0 && userName.Trim(new char[] { '.' }).Length == 0;
            if (res)
            {
                string str = "\"[]:|<>+=;,?*@";
                for (int i = 0; i < userName.Length; i++)
                {
                    if (str.IndexOf(userName[i]) >= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    /// <summary>
    /// 数据安全类
    /// </summary>
    public sealed class DataSecurity
    {
        /// <summary>
        /// 特殊字符过滤
        /// </summary>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string FilterBadChar(string strchar)
        {
            string input = "";
            if (string.IsNullOrEmpty(strchar))
            {
                return "";
            }
            string str = strchar;
            string[] strArray = new string[] {
                "+", "'", "%", "^", "&", "?", "(", ")", "<", ">", "[", "]", "{", "}", "/", "/\"",
                ";", ":", "Chr(34)", "Chr(0)", "--"
             };
            StringBuilder builder = new StringBuilder(str);
            for (int i = 0; i < strArray.Length; i++)
            {
                input = builder.Replace(strArray[i], "").ToString();
            }
            return Regex.Replace(input, "@+", "@");
        }

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string FilterSqlKeyword(string strchar)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(strchar))
            {
                return string.Empty;
            }
            strchar = strchar.ToLower();
            string[] strArray = new string[] {
                "select", "update", "insert", "delete", "declare", "@", "exec", "dbcc", "alter", "drop", "create", "backup", "if", "else", "end", "and",
                "or", "add", "set", "open", "close", "use", "begin", "retun", "as", "go", "exists", "kill"
             };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strchar.Contains(strArray[i]))
                {
                    strchar = strchar.Replace(strArray[i], "");
                    flag = true;
                }
            }
            if (flag)
            {
                return FilterSqlKeyword(strchar);
            }
            return strchar;
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlDecode(object value)
        {
            if (value == null)
            {
                return null;
            }
            return HtmlDecode(value.ToString());
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlDecode(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("<br>", "/n");
                value = value.Replace("& gt;", ">");
                value = value.Replace("& lt;", "<");
                value = value.Replace("& nbsp;", " ");
                value = value.Replace("& #39;", "'");
                value = value.Replace("& quot;", "/\"");
            }
            return value;
        }

        /// <summary>
        /// HTML解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlEncode(object value)
        {
            return value == null ? string.Empty : HtmlEncode(value.ToString());
        }

        /// <summary>
        /// HTML解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str.Replace("<", "& lt;").Replace(">", "& gt;").Replace(" ", "& nbsp;").Replace("'", "& #39;").Replace("/\"", " & quot; ").Replace("/r/n", "<br>").Replace("/n", "<br>");
        }
    }

    /// <summary>
    /// 数据转换类
    /// </summary>
    public sealed class DataConverter
    {
        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime CDate(object input)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDate(input.ToString());
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime CDate(string input)
        {
            if (!DateTime.TryParse(input, out DateTime now))
            {
                now = DateTime.Now;
            }
            return now;
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outTime"></param>
        /// <returns></returns>
        public static DateTime? CDate(string input, DateTime? outTime)
        {
            if (!DateTime.TryParse(input, out DateTime time))
            {
                return outTime;
            }
            return new DateTime?(time);
        }

        /// <summary>
        /// 实数转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal CDecimal(object input)
        {
            return CDecimal(input, 0M);
        }

        /// <summary>
        /// 实数转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal CDecimal(string input)
        {
            return CDecimal(input, 0M);
        }

        /// <summary>
        /// 实数转换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal CDecimal(object input, decimal defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDecimal(input.ToString(), defaultValue);
            }
            return 0M;
        }

        /// <summary>
        /// 实数转换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal CDecimal(string input, decimal defaultValue)
        {
            if (!decimal.TryParse(input, out decimal num))
            {
                num = defaultValue;
            }
            return num;
        }

        /// <summary>
        /// Object to 双精
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double CDouble(object input)
        {
            return CDouble(input, 0.0);
        }

        /// <summary>
        /// String to 双精
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double CDouble(string input)
        {
            return CDouble(input, 0.0);
        }

        /// <summary>
        /// 双精
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double CDouble(object input, double defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CDouble(input.ToString(), defaultValue);
            }
            return 0.0;
        }

        /// <summary>
        /// 双精
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double CDouble(string input, double defaultValue)
        {
            if (!double.TryParse(input, out double num))
            {
                return defaultValue;
            }
            return num;
        }

        /// <summary>
        /// 整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int CInt(object input)
        {
            return CInt(input, 0);
        }

        /// <summary>
        /// 整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int CInt(string input)
        {
            return CInt(input, 0);
        }

        /// <summary>
        /// 整型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int CInt(object input, int defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CInt(input.ToString(), defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// 整型
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int CInt(string input, int defaultValue)
        {
            if (!int.TryParse(input, out int num))
            {
                num = defaultValue;
            }
            return num;
        }

        /// <summary>
        /// 单精
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float CFloat(object input)
        {
            return CFloat(input, 0f);
        }

        /// <summary>
        /// 单精
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float CFloat(string input)
        {
            return CFloat(input, 0f);
        }

        /// <summary>
        /// 单精
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float CFloat(object input, float defaultValue)
        {
            if (!Convert.IsDBNull(input) && !object.Equals(input, null))
            {
                return CFloat(input.ToString(), defaultValue);
            }
            return 0f;
        }

        /// <summary>
        /// 单精
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float CFloat(string input, float defaultValue)
        {
            if (!float.TryParse(input, out float num))
            {
                num = defaultValue;
            }
            return num;
        }
    }
}