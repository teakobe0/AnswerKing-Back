using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DAL
{
    public class BaseDAL
    {
        protected DataContext _context;

        //public const int InitID = 1000000000;
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }

    }
}
