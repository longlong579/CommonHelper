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
// FileName : RSAUtil
//
// Created by : XHL at 2020/8/7 0:04:32
//
// Function : 
//
//========================================================================
namespace HZZG.Common.Tolls
{
   public struct RSASecretKey
    {
        public RSASecretKey(string privateKey, string publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public override string ToString()
        {
            return string.Format(
                "PrivateKey: {0}\r\nPublicKey: {1}", PrivateKey, PublicKey);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public  class RSAUtil
    {
        /// <summary>
        /// generate RSA secret key
        /// </summary>
        /// <param name="keySize">the size of the key,must from 384 bits to 16384 bits in increments of 8 </param>
        /// <returns></returns>
        public  RSASecretKey  GenerateRSASecretKey(int keySize=384)
        {
            RSASecretKey rsaKey = new RSASecretKey();
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
            {
                rsaKey.PrivateKey = rsa.ToXmlString(true);
                rsaKey.PublicKey = rsa.ToXmlString(false);
            }
            return rsaKey;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string RSAEncrypt(string xmlPublicKey, string content)
        {
            string encryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                byte[] encryptedData = rsa.Encrypt(Encoding.Default.GetBytes(content), false);
                encryptedContent = Convert.ToBase64String(encryptedData);
            }
            return encryptedContent;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string RSADecrypt(string xmlPrivateKey, string content)
        {
            string decryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(content), false);
                decryptedContent = Encoding.GetEncoding("gb2312").GetString(decryptedData);
            }
            return decryptedContent;
        }

        #region 2种生成公私钥方法
        /// <summary>
        /// 生成一对公钥和私钥
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, string> GetKeyPair1()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key = Convert.ToBase64String(RSA.ExportCspBlob(false));
            string private_Key = Convert.ToBase64String(RSA.ExportCspBlob(true));
            return new KeyValuePair<string, string>(public_Key, private_Key);
        }
        /// <summary>
        /// 生成一对公钥和私钥
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, string> GetKeyPair2()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key = RSA.ToXmlString(false);
            string private_Key = RSA.ToXmlString(true);
            return new KeyValuePair<string, string>(public_Key, private_Key);
        }
        #endregion
    }
}
