using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//========================================================================
// Copyright(C): ***********************
//
// CLR Version : 4.0.30319.42000
// NameSpace : HZZG.Common.Tolls.Tools
// FileName : CopyUtil
//
// Created by : XHL at 2020/6/23 10:28:32
//
// Function : 
//
//========================================================================
namespace HZZG.Common.Tolls
{
  
    public class CopyUtil
    {
        /// <summary>
        /// 二进制深拷贝（适合所有情况，但必须有[Seriable]）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopyByBinary<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// 利用xml序列化和反序列化 注意：只适合基础类（无方法，无集合，只有字段或属性的类）利用Json也同理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByXml<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
