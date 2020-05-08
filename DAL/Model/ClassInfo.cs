using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{


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

}