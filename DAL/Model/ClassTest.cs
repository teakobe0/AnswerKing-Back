using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
   public class ClassTest: BaseModel
    {
        public string Name { get; set; }//课程名称
        public int UniversityTestId { get; set; }//学校id
        public int ClientId { get; set; }//客户id
        public string Professor { get; set; }//教授
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除

    }
    public class UniversityTest : BaseModel
    {
        public string Name { get; set; }//学校名称
        public int ClientId { get; set; }//客户id
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }

    public class ClassInfoTest : BaseModel
    {
        public string Name { get; set; } //标题
        public int ClientId { get; set; }//客户id
        public int ClassTestId { get; set; }//课程id
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }

    public class ClassInfoContentTest : BaseModel
    {
        public string Name { get; set; }//题目
        public string NameUrl { get; set; }//题目url
        public string Contents { get; set; }//答案内容
        public string Url { get; set; }//答案内容url
        public int ClassWeek { get; set; }//每周课程名称
        public string ClassWeekType { get; set; }//每周课程类型名称
        public int ClassTestId { get; set; }//课程id
        public int ClassInfoTestId { get; set; }//课程资料id
        public int UniversityTestId { get; set; }//学校id
        public int ClientId { get; set; }//客户id
        public bool IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
      
    }
}


