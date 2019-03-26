using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class ResultModel
    {
        /// <summary>
        /// api 状态
        /// </summary>
        public RmStatus Status { get; set; }
        /// <summary>
        /// api 返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 后台返回msg 
        /// </summary>
        public string Msg { get; set; }
    }
    public enum RmStatus
    {
        OK=1,
        Error=2,
    }
    public class PageData
    {
        public int PageTotal { get; set; }
        public object Data { get; set; }
    }
}
