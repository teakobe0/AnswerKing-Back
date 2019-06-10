using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JzAPI.paypal
{
    public class Payment
    {
        public string id { get; set; }

        public string intent { get; set; }

        public List<purchase_units> purchase_units { get; set; }

        public payer payer { get; set; }

        public string create_time { get; set; }
        public string update_time { get; set; }

        public List<links> links { get; set; }
        public string status { get; set; }
    }
    public class purchase_units
    {
        public string reference_id { get; set; }
        public amount amount { get; set; }
        public payee payee { get; set; }
        public shipping shipping { get; set; }
        public payments payments { get; set; }
    }

    public class amount
    {
        public string currency_code { get; set; }

        public decimal value { get; set; }
    }

    public class payee
    {
        public string email_address { get; set; }
        public string merchant_id { get; set; }
    }

    public class shipping
    {
        public name name { get; set; }
        public address address { get; set; }
    }

    public class name
    {
        public string full_name { get; set; }
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class address
    {
        public string address_line_1 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class payments
    {
        public List<captures> captures { get; set; }
    }

    public class captures
    {
        public string id { get; set; }
        public string status { get; set; }
        public amount amount { get; set; }
        public string final_capture { get; set; }
        public seller_protection seller_protection { get; set; }
        public seller_receivable_breakdown seller_receivable_breakdown { get; set; }
        public List<links> links { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
    }

    public class seller_protection
    {
        public string status { get; set; }
        public object dispute_categories { get; set; }
    }

    public class seller_receivable_breakdown
    {
        public amount gross_amount { get; set; }
        public amount paypal_fee { get; set; }
        public amount net_amount { get; set; }
    }

    public class links
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class payer
    {
        public name name { get; set; }
        public string email_address { get; set; }
        public string payer_id { get; set; }
        public address address { get; set; }
    }
   
    public enum OrderStatus
    {
        unpaid=0,
        paid=1
    }
    public enum PayType
    {
        weixin = 1,
        zhifubao = 2,
        paypal=3
    }
}
