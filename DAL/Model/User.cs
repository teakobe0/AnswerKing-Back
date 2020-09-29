using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 管理员表
    /// </summary>
   public class User:BaseUser
    {
        public string Group { get; set; }//部门
        public bool isTerminated { get; set; }//是否解雇
    }
}
