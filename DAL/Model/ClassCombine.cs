using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 课程合并表
    /// </summary>
  public class ClassCombine:BaseModel
    {
        public int OriginalId { get; set; }//原始id(合并前的课程id)
        public int TargetId { get; set; }//目标id(合并后的课程id)
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
