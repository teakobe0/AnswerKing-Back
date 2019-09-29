using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class UniversityCombine : BaseModel
    {
        public int OriginalId { get; set; }//原始id(合并前的学校id)
        public int TargetId { get; set; }//目标id(合并后的学校id)
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
