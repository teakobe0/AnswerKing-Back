using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DAL.Tools
{
    /// <summary>
    /// 验证类
    /// </summary>
    public class ValidateUtil
    {
        //private static Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        private static Regex RegEmail = new Regex(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+$");
        /// <summary>
        /// 校验邮箱正确性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            Match m = RegEmail.Match(value);
            return m.Success;
        }
        public static  IEnumerable<TSource> DistinctBy<TSource, TKey>( IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }
}
