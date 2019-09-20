using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    public class Order : BaseModel
    {
        public string Name { get; set; }//订单名称
        public int ClientId { get; set; }//客户id
        public string OrderNo { get; set; } //订单号
        public DateTime PayTime { get; set; }//支付时间
        public int PayType { get; set; }//支付方式 1:微信, 2:支付宝
        public int Status { get; set; }//支付状态 0:未支付，1:已支付
        public string Price { get; set; }//订单金额
        public string Currency { get; set; }//订单支付货币
        public string Memo { get; set; }//备注
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除

    }
}
 