using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JzAPI.paypal
{
    /// <summary>
    /// paypal支付配置文件
    /// </summary>
    public class PaypalConfig
    {
        /// <summary>
        /// 测试环境，后期配置正式id，secret,url
        /// </summary>
        //public static string cilentid = "AegO1ccZLZRm3W9ds6eajow6UyairqNqktC6RyyE61ieEcivlmi3YJF7SPghAFJXq_PV6FYkRJ2XwKpp";
        //public static string clientsecret = "EI5XM355w0XR_CXZJo64rvznW_d1xDvHQ64EouMAUJg5x7wyXekWgR6a2Td1Ul4ZWrju1sKoQByDB1O7";
        //public static string url = "https://api.sandbox.paypal.com/v2/checkout/orders/{0}";
        public static string cilentid = "AVplzXK74mZi6ltEo8QhoMMUdjc-OxXpinwbbgEtgePr8kT9zBMur4DtdQOOyNV76xUBRlcGm_llrO9o";
        public static string clientsecret = "EJfGSbr-gHYmCmXvSIVHpiy2EhseIEQuaWVyRaKZlQkGw8i_LEaQhz0TP_ixPvh29jDZdJiQ68DZF0XI";
        public static string url = "https://api.paypal.com/v2/checkout/orders/{0}";
    }
}
