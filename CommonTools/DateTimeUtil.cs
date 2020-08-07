using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{
    public  sealed class DateTimeUtil
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string StandardString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CompactString(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DetailString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static TimeSpan TimeSpan(DateTime dt1, DateTime dt2)
        {
            if (dt1 > dt2)
                return dt1 - dt2;
            else
                return dt2 - dt1;
        }

        /// <summary>
        /// 程序执行时间测试
        /// </summary>
        /// <param name="dateBegin">开始时间</param>
        /// <param name="dateEnd">结束时间</param>
        /// <returns>返回(秒)单位，比如: 0.00239秒</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.TotalMilliseconds.ToString();
        }
    }
}
