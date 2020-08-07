using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HZZG.Common.Tolls
{
    /// <summary>
    /// XML助理类
    /// </summary>
    public static class XMLHelper
    {  
        private static int conDefaultInt=-1;
        private static float conDefaultFloat = 0f;
        /// <summary>
        /// 使用XML序列化与反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xmlData) where T : new()
        {
            if (string.IsNullOrEmpty(xmlData))
                return default(T);

            TextReader tr = new StringReader(xmlData);
            T DocItms = new T();
            XmlSerializer xms = new XmlSerializer(DocItms.GetType());
            DocItms = (T)xms.Deserialize(tr);

            return DocItms == null ? default(T) : DocItms;
        }
        /// <summary>
        /// 获取int属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static int GetIntAttribute(XElement element, string attribute)
        {
            return GetIntAttribute(element, attribute,conDefaultInt);
        }

        /// <summary>
        /// 获取float属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static float GetFloatAttribute(XElement element, string attribute)
        {
            return GetFloatAttribute(element, attribute, conDefaultFloat);
        }

        /// <summary>
        /// 是否包含属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static bool HasAttribute(XElement element, string attribute)
        {
            return element.Attribute(attribute) != null;
        }

        /// <summary>
        /// 获取int属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntAttribute(XElement element, string attribute, int defaultValue)
        {
            if (element == null || element.Attribute(attribute) == null)
            {
                return defaultValue;
            }

            return element.Attribute(attribute).Value.CastToNum<int>();
        }


        /// <summary>
        /// 获取float属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float GetFloatAttribute(XElement element, string attribute, float defaultValue)
        {
            if (element == null || element.Attribute(attribute) == null)
            {
                return defaultValue;
            }

            return element.Attribute(attribute).Value.CastToNum<float>();
        }

        /// <summary>
        /// 获取string属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetStringAttribute(XElement element, string attribute)
        {
            if (element == null || element.Attribute(attribute) == null)
            {
                return null;
            }

            return element.Attribute(attribute).Value;
        }

        /// <summary>
        /// 获取枚举属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static T GetEnumAttribute<T>(XElement element, string attribute)
        {
            try
            {
                if (element == null || element.Attribute(attribute) == null)
                {
                    return default(T);
                }

                return element.Attribute(attribute).Value.CastTo<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetAttibute(XElement element, string attribute, object value)
        {
            if (element.Attribute(attribute) != null)
            {
                if (value != null)
                {
                    element.Attribute(attribute).Value = value.ToString();
                }
            }
        }

        public static bool GetBoolAttribute(XElement element, string attribute)
        {
            if (element != null && element.Attribute(attribute) != null)
            {
                bool ret = false;
                bool.TryParse(element.Attribute(attribute).Value.ToLower(), out ret);
                return ret;
            }
            else
                return false;
        }

        public static bool GetBoolAttribute(XElement element, string attribute, bool defaultValue)
        {
            if (element != null && element.Attribute(attribute) != null)
            {
                bool ret = false;
                bool.TryParse(element.Attribute(attribute).Value.ToLower(), out ret);
                return ret;
            }
            else
                return defaultValue;
        }

        //add by zhaoqishi 
        public static int GetIntXElementValue(XElement element)
        {
            return element.Value.CastToNum<int>();
        }


        public static float GetFloatXElementValue(XElement element)
        {
            return element.Value.CastToNum<float>();
        }

        public static string GetStringXElementValue(XElement element)
        {
            return element.Value;
        }
    }
}
