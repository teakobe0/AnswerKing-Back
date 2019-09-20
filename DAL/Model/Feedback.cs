using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class Feedback:BaseModel
    {
        public string Name { get; set; } //主题
        public string Url { get; set; } //页面地址
        public string Content { get; set; }//问题描述内容
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
