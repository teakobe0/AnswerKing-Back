using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class Class: BaseModel
    {
        public string Name { get; set; }//课程名称
        public int UniversityId { get; set; }//学校id
        public string University { get; set; }//大学名称
        public int ClientId { get; set; }//客户id
        public string Professor { get; set; }//教授
        public string Memo { get; set; }//备注
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除

    }
    public class University : BaseModel
    {
        public string Name { get; set; }//学校名称
        public string Intro { get; set; }//学校简介 
        public string Image { get; set; }//图片
        public string Country { get; set; }//国家
        public string State { get; set; }//州/省份
        public string City { get; set; }//城市
        public int ClientId { get; set; }//客户id
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }

    public class ClassInfo : BaseModel
    {
        public string Name { get; set; } //标题
        public int ClientId { get; set; }//客户id
        public int ClassId { get; set; }//课程id
        public int Grade { get; set; } //得分
        public int Use { get; set; } //有用
        public int NoUse { get; set; }//没用
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public int Status { get; set; } //状态 0：未创建 1：全新未审核  2：已审核 3：修改未审核
    }

    public class ClassInfoContent : BaseModel
    {
        public string Name { get; set; }//题目
        public string NameUrl { get; set; }//题目url
        public string Contents { get; set; }//答案内容
        public string Url { get; set; }//答案内容url
        public int ClassWeek { get; set; }//每周课程名称
        public string ClassWeekType { get; set; }//每周课程类型名称
        public int ClassId { get; set; }//课程id
        public int ClassInfoId { get; set; }//课程资料id
        public int UniversityId { get; set; }//学校id
        public int ClientId { get; set; }//客户id
        public int Grade { get; set; } //得分
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}


