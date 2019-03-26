using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{

    public class Comment:BaseModel
    {
        public string  ParentId { get; set; } //评论父级id
        public int ClientId { get; set; }//客户id
        public string Contents { get; set; }//评论内容
        public int ClassInfoId { get; set; }//课程资料id
    }
}
