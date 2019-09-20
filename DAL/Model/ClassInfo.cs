using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    public class ClassInfo : BaseModel
    {
        public int ClientId { get; set; }//客户id
        public int ClassId { get; set; }//课程id
        public int TotalGrade { get; set; }//总分
        public string Evaluate { get; set; }//评价
        public int Use { get; set; } //有用
        public int NoUse { get; set; }//没用
        public int PageViews { get; set; }//浏览量
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }

    public class ClassWeek : BaseModel
    {
        public int ClassInfoId { get; set; }//课程资料id
        public int No { get; set; }//每周课程名称
        public int Grade { get; set; }//每周课程得分
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }

    public class ClassWeekType : BaseModel
    {
        public int ClassWeekId { get; set; }//每周课程id
        public int ClassWeekTypeId { get; set; }//每周课程类型id
        public string ContentType { get; set; }//内容类型
        public int Grade { get; set; }//每周课程内容得分
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
    public class ClassInfoContent : BaseModel
    {
        public string Contents { get; set; }//文字问题、答案内容
        public string Url { get; set; }//图片问题、答案内容
        public int CwtParentId { get; set; } //每周课程类型父级id(其实存储的是ClassWeekType表中ClassWeekTypeId这个字段)
        public int ClassWeekId { get; set; }//每周课程id
        public int ClassWeekTypeId { get; set; }//每周课程类型id
        public int ClassId { get; set; }//课程id
        public int ClassInfoId { get; set; }//课程资料id
        public int UniversityId { get; set; }//学校id
        public int IsAudit { get; set; }//是否审核 0：未审核，1：已审核
        public int Status { get; set; }//状态 0：未删除，1：已删除
    }
}
