using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    public class Client:BaseUser
    {
        
        public string Sex { get; set; }//性别
        public DateTime Birthday { get; set; }//生日
        public DateTime EffectiveDate { get; set; }//有效期
        public string Image { get; set; }//客户图像


    }
}
