using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//========================================================================
// Copyright(C): ***********************
//
// CLR Version : 4.0.30319.42000
// NameSpace : HZZG.Common.Tolls.EncryptDecrypt
// FileName : MD5Util
//
// Created by : XHL at 2020/8/7 0:29:36
//
// Function : 
//
//========================================================================
namespace HZZG.Common.Tolls
{
    public static class MD5Util
    {
        /// <summary>
        /// MD5 加密字符串
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <returns>加密后字符串</returns>
        private static string MD5Encoding(this string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder stb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                stb.Append(b.ToString("x2"));
            }
            return stb.ToString();
        }

        /// <summary>
        /// MD5盐值加密
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <param name="salt">盐值</param>
        /// <returns>加密后字符串</returns>
        public static string MD5Encoding(this string rawPass, object salt)
        {
            if (salt == null) return MD5Encoding(rawPass);
            return MD5Encoding(rawPass + "{" + salt.ToString() + "}");
        }
    }
}
