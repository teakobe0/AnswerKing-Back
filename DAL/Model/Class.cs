﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Class : BaseModel
    {
        public string Name { get; set; }//课程名称
        public int UniversityId { get; set; }//学校id
        public string University { get; set; }//大学名称
        public string Difficulty { get; set; } //难度系数
        public string Professor { get; set; }//教授
        public string Memo { get; set; }//备注
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
        public int OriginUniversityId { get; set; }//合并前的学校id

    }
}
