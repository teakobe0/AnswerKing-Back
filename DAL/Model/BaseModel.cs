using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    public class BaseModel  
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RefId { get; set; }
        [MaxLength(50)]
        public string CreateBy { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }
    }

    //public class BaseModel_NoIdentity 
    //{
    //    [Key]
    //    [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }

    //    [MaxLength(50)]
    //    public string CreateBy { get; set; }

    //    [Required]
    //    public DateTime CreateTime { get; set; }
    //}
}
