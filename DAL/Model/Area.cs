﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 地区表
    /// </summary>
    public class Area:BaseModel
    {
        public int ParentId { get; set; }//父级id
        public string Country { get; set; }//国家
        public string State { get; set; }//州、省
       
    }
}
