using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{
    public class SerializeUtil
    {
        /// <summary>
        /// 序列化模型到 byte 数组 [Serializable]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T model)
        {
            if (model != null)
            {
                MemoryStream ms = null;
                try
                {
                    ms = new MemoryStream(); //内存实例
                    BinaryFormatter formatter = new BinaryFormatter(); //创建序列化的实例
                    formatter.Serialize(ms, model);//序列化对象，写入ms流中    
                    byte[] bytes = ms.GetBuffer();
                    return bytes;
                }
                catch { }
                finally
                {
                    ms?.Close();
                }
            }
            return null;
        }
        public static T Deserialize<T>(byte[] bytes)
        {
            if (bytes != null)
            {
                MemoryStream ms = null;
                try
                {
                    object obj = null;
                    ms = new MemoryStream(bytes); //利用传来的byte[]创建一个内存流
                    ms.Position = 0;
                    BinaryFormatter formatter = new BinaryFormatter();
                    obj = formatter.Deserialize(ms);//把内存流反序列成对象
                    return (T)Convert.ChangeType(obj, typeof(T)); ;
                }
                catch { }
                finally
                {
                    ms?.Close();
                }
            }
            return default(T);
        }

        /// <summary>
        /// 将类转化为2进制,并保存文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullFilePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Serialize2File<T>(string fullFilePath,T data)
        {
            string errorInfo=null;
            FileStream fileStream = null;
            try
            {
                if (!Directory.Exists(fullFilePath))
                {
                    Directory.CreateDirectory(fullFilePath);
                }
       
                fileStream = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, data);
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
            }
            finally
            {
                if (fileStream != null) { fileStream.Close(); }
            }
            return errorInfo;
        }

        /// <summary>
        /// 将二进制文件转换为相应的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullFilePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DeSerializeFromFile<T>(string fullFilePath, out T data) where T : class 
        {
            data = null;
            string errorInfo = null;
            if (!File.Exists(fullFilePath))
            {
                return "File not exist!";
            }

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                data = bf.Deserialize(fileStream) as T;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
            }
            finally
            {
                if (fileStream != null) { fileStream.Close(); }              
            }
            return errorInfo;
        }
    }
}
