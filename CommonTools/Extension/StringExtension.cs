using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{    /// <summary>
     /// 1.字符串处理
     /// 2.环境变量，文件路径处理 
     /// 3.值转换用CastTo<>
     /// </summary>
    public static class StringExtension
    {
        #region 字符串处理
        /// <summary>
        /// 判断字符串 非null、""（Not NullOrEmpty）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
      
        /// <summary>
        /// 查看字符串包含字符（不区分大小写）
        /// </summary>
        /// <param name="s"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool Contains(this string s, string word,bool ignoreCase=true)
        {
            if (ignoreCase)
            {
                if (s.ToLower().Contains(word.ToLower()))
                    return true;
            }
            else
            {
                if (s.Contains(word))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            if (!str.isEmpty())
            {
                string[] list = Regex.Split(str, separator, RegexOptions.None);
                return list;
            }
            return null;
        }
        public static string[] Split(this string str, char[] arraySeparatorChar)
        {
            if (!str.isEmpty())
            {
                string[] list = str.Split(arraySeparatorChar);
                return list;
            }
            return null;
        }
        /// <summary>
        /// 根据通配符验证字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="pattern">通配符：%和_</param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            try
            {
                //key = key.Replace("%", @"[\s\S]*").Replace("_", @"[\s\S]");
                pattern = pattern.Replace("%", ".*").Replace("_", ".");
                return Regex.IsMatch(s, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 秒转毫秒
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int SecToMs(this string sec)
        {
            double outSec;
            double.TryParse(sec, out outSec);
            return (int)(outSec * 1000);
        }
        #endregion

        #region File/路径/环境变量相关
        public static string getEnvironmentVariable(this string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName);
        }

        public static string[] GetFileNameList(this string pathDir)
        {
            return Directory.GetFiles(pathDir);
        }
        /// <summary>
        /// 获取目录下文件的信息
        /// FileInfo 包括文件创建时间,大小,文件名,路径等信息
        /// </summary>
        /// <param name="pathDir"></param>
        /// <param name="creatDirIfNotExist"></param>
        /// <returns></returns>
        public static FileInfo[] GetFileInfoList(this string pathDir, bool creatDirIfNotExist = true)
        {
            FileInfo[] file = null;
            if (Directory.Exists(pathDir))
            {
                DirectoryInfo dir = new DirectoryInfo(pathDir);
                file = dir.GetFiles();
            }
            else
            {
                if (creatDirIfNotExist)
                    Directory.CreateDirectory(pathDir);
                else
                    return null;
            }
            return file;
        }
        #endregion

        #region 字符串转换 建议用CastToNum<>
        /// <summary>
        /// 通过泛型 扩展方法 统一转换String为指定类型 不报错，错误时转换为默认值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static DateTime ParseByDefault(this string input, DateTime defaultvalue)
        {
            return input.ParseStringToType<DateTime>(delegate (string e) { return Convert.ToDateTime(input); },
                defaultvalue);
        }

        public static decimal ParseByDefault(this string input, decimal defaultvalue)
        {
            return input.ParseStringToType<decimal>(delegate (string e) { return Convert.ToDecimal(input); }, defaultvalue);
        }

        public static double ParseByDefault(this string input, double defaultvalue)
        {
            return input.ParseStringToType<double>(delegate (string e) { return Convert.ToDouble(input); }, defaultvalue);
        }

        public static int ParseByDefault(this string input, int defaultvalue)
        {
            return input.ParseStringToType<int>(delegate (string e) { return Convert.ToInt32(input); }, defaultvalue);
        }

        public static long ParseByDefault(this string input, long defaultvalue)
        {
            return input.ParseStringToType<long>(delegate (string e) { return Convert.ToInt64(input); }, defaultvalue);
        }

        public static float ParseByDefault(this string input, float defaultvalue)
        {
            return input.ParseStringToType<float>(delegate (string e) { return Convert.ToSingle(input); }, defaultvalue);
        }

        public static float ParseByDefault(this string input, short defaultvalue)
        {
            return input.ParseStringToType<short>(delegate (string e) { return Convert.ToInt16(input); }, defaultvalue);
        }

        public static string ParseByDefault(this string input, string defaultvalue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultvalue;
            }

            return input;
        }

        private static T ParseStringToType<T>(this string input, Func<string, T> action, T defaultvalue) where T : struct
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultvalue;
            }

            try
            {
                return action(input);
            }
            catch
            {
                return defaultvalue;
            }
        }
        #endregion
    }
}
