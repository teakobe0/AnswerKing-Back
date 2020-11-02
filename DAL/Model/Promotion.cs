using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 推广表
    /// </summary>
    public class Promotion:BaseModel
    {
        public string Name { get; set; }//推广机构名字
        public string PId { get; set; }//推广机构id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
