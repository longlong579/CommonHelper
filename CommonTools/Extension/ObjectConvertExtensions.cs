using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

//========================================================================
// Copyright(C): ***********************
//
// CLR Version : 4.0.30319.42000
// NameSpace : IECommonEntiry.Tools.Extension
// FileName : ObjectExtensions
//
// Created by : XHL at 2020/8/06 11:26:04
//
// Function : 
//
//========================================================================
namespace HZZG.Common.Tolls
{
    /// <summary>
    /// 1.转换
    /// 2.深拷贝
    /// </summary>
    public static class ObjectConvertExtensions
    {
        #region 转换相关
        /// <summary>
        /// 通用类型扩展方法类
        /// ChangeType往往用在不知道当前类型应当是什么的情况下
        /// 要使转换成功，value 必须实现 IConvertible 接口
        /// 把对象类型转化为指定类型，转化失败时返回该类型默认值，对于数值转换，不考虑精度（该方法主要用于通用，数字建议用CastToNum）
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="digit"> 若是String和Decimal之间转换，转化小数位，int.MaxValue为不处理小数 最多28位</param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        /// 
        //使用情形：枚举之间的转换，bool类型等的转换
        public static T CastTo<T>(this object value, bool throwError = false)
        {
            object result = null;
            Type type = typeof(T);
            try
            {
                if (type == value.GetType()) return (T)value;
                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                    //注意：若值不存在枚举中，转换不会报错（WCF中会导致堵塞，超时异常）
                    if (!Enum.IsDefined(type, result))
                        throw new EnumException(string.Format("Enum [{0}] not defined in {1}", value, typeof(T).Name));
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else if (!type.IsInterface && type.IsGenericType)
                {
                    Type innerType = type.GetGenericArguments()[0];
                    object innerValue = CastTo(value, innerType, throwError);
                    result = Activator.CreateInstance(type, new object[] { innerValue });
                }
                //枚举转文字/值
                else if (value.GetType().IsEnum && (type == typeof(string) || type == typeof(int)))
                {
                    if (type == typeof(string))
                    {
                        //枚举转名称
                        result= Enum.GetName(value.GetType(), value);
                    }
                    if (type == typeof(int))
                    {
                        //枚举转名称
                        result = (int)value;
                    }
                }
                else
                {
                    //可以转换，但效率低（可优化为常用的转换分细类，提高效率，但分类增多）
                    //比如枚举名《=》枚举《=》枚举值
                    result = Convert.ChangeType(value, type);
                }
            }
            catch (EnumException enumError)
            {
                throw(enumError);
            }
            catch (Exception error)
            {
                result = default(T);
                if (throwError)
                    throw new Exception("Error:Change Failed!", error);

            }

            return (T)result;
        }
        public static T CastTo<T>(this object value, T defaultValue, bool throwError = false)
        {
            object result = null;
            Type type = typeof(T);
            try
            {
                if (type == value.GetType()) return (T)value;
                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else if (!type.IsInterface && type.IsGenericType)
                {
                    Type innerType = type.GetGenericArguments()[0];
                    object innerValue = CastTo(value, innerType, throwError);
                    result = Activator.CreateInstance(type, new object[] { innerValue });
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch (Exception error)
            {
                result = defaultValue;
                if (throwError)
                    throw new Exception("Error:Change Failed!", error);

            }

            return (T)result;
        }


        /// <summary>
        /// 数字转换(暂时只做了string/byte/int32/float/double/decimal之间的转换,可增加数据类型)
        /// 例子："4.222"->"4.22" "4.222".CastToNum<string>(2);
        /// "4.33"->4.3 "433".CastToNum<double>(2);
        /// </summary>默认四舍五入
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="digitIfNeed">若是小数，保留的小数位（若是整数，无用）</param>
        /// <param name="throwError">false:转化失败时返回该类型默认值;true:转换失败抛出异常</param>
        /// <param name="defaultValue">false:转化失败时返回该类型默认值 该值要起作用，则throwError必须为false!!!</param>
        /// <returns></returns>
        public static T CastToNum<T>(this object value, byte digitIfNeed = byte.MaxValue, bool throwError = true, T defaultValue = default(T), string format = "G", NumberFormatInfo numberFormatInfo = null)
        {
            Type type = typeof(T);
            List<Type> listType = new List<Type>() { typeof(string), typeof(byte), typeof(int), typeof(Decimal), typeof(double), typeof(float) };
            if (!listType.Contains(type))
                throw new Exception("Type Error:T must be 'string' or'byte' or 'double' or 'float' or 'Decimal'");

            object result = null;
            try
            {
                if (type != typeof(string) && type == value.GetType())
                {
                    return (T)value;
                }

                //数字string 转 格式化的string
                if (type == typeof(string) && type == value.GetType())
                {
                    double numDouble = double.Parse(value.ToString());

                    NumberFormatInfo provider;
                    if (numberFormatInfo != null)
                        provider = numberFormatInfo;
                    else
                    {
                        provider = new NumberFormatInfo();
                        provider.NumberDecimalDigits = digitIfNeed;
                    }
                    result = Math.Round(numDouble, digitIfNeed).ToString(format, provider);
                }
                //string 转数字保留digitIfNeed位小数，全小数点后0不会保留
                if (value is string)
                {
                    if (type == typeof(byte))
                        result = byte.Parse(value.ToString());
                    if (type == typeof(int))
                        result = int.Parse(value.ToString());
                    if (digitIfNeed == byte.MaxValue)//不需要考虑小数位
                    {
                        switch (type.Name.ToLower())
                        {
                            case "double":
                                //效率比Convert高
                                result = double.Parse(value.ToString());
                                break;
                            case "single":
                                result = float.Parse(value.ToString());
                                break;
                            case "decimal":
                                result = decimal.Parse(value.ToString());
                                break;
                        }
                        //result = Convert.ChangeType(value, type);
                    }
                    else
                    {
                        switch (type.Name.ToLower())
                        {
                            case "double":
                                //效率比Convert高
                                double numDouble = double.Parse(value.ToString());
                                result = Math.Round(numDouble, digitIfNeed);
                                break;
                            case "single":
                                Single numFloat = Single.Parse(value.ToString());
                                result = (float)Math.Round(numFloat, digitIfNeed);
                                break;
                            case "decimal":
                                Decimal numDecimal = Decimal.Parse(value.ToString());
                                result = Math.Round(numDecimal, digitIfNeed);
                                break;
                        }

                    }
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch (Exception error)
            {
                result = defaultValue;
                if (throwError)
                    throw new Exception("Error:Change Failed!", error);

            }

            return (T)result;
        }
        #endregion

        #region 深拷贝
        /// <summary>
        /// 二进制深拷贝（适合所有情况，但必须有[Seriable]）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopyByBinary<T>(this T source)
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
            Stream stream = new System.IO.MemoryStream();
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
        public static T DeepCopyByXml<T>(this T obj)
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
        #endregion
    }

    [Serializable]
    public class EnumException : Exception
    {
        public EnumException()
        {
        }
        public EnumException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// 实现ISerialization接口所需要的反序列化构造函数。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private EnumException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        ///  重写GetObjectData方法。如果添加了自定义字段，一定要重写基类GetObjectData方法的实现
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // 序列化自定义数据成员
            //info.AddValue("StringInfo", stringInfo);

            // 调用基类方法，序列化它的成员
            base.GetObjectData(info, context);
        }

    }
}
