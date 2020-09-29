using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 评论表
    /// </summary>
    public class Comment:BaseModel
    {
        public string  ParentId { get; set; } //评论父级id
        public int ClientId { get; set; }//客户id
        public string Contents { get; set; }//评论内容
        public int ClassInfoId { get; set; }//课程资料id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
