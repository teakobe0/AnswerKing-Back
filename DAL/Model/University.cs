using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class University : BaseModel
    {
        public string Name { get; set; }//学校名称
        public string Intro { get; set; }//学校简介
        public string Image { get; set; }//图片
        public string Country { get; set; }//国家
        public string State { get; set; }//州/省份
        public string City { get; set; }//城市
        public string Address { get; set; }//地址

    }
}
