using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 关注表
    /// </summary>
   public class Focus:BaseModel
    {
        public int ClientId { get; set; }//客户id
        public string Name { get; set; } //标题
        public int Type { get; set; }// 类型 1:课程, 2：课程资料(题库集)
        public string  TypeId { get; set; }//类型id  (备注：type:1,存classid；type:2,存classid，classinfoid)
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public DateTime CancelTime { get; set; }//取消关注时间
    }
}
