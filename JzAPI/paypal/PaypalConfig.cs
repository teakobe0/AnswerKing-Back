using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JzAPI.paypal
{
    public class PaypalConfig
    {
        /// <summary>
        /// 测试环境，后期配置正式id，secret,url
        /// </summary>
        public static string cilentid = "AegO1ccZLZRm3W9ds6eajow6UyairqNqktC6RyyE61ieEcivlmi3YJF7SPghAFJXq_PV6FYkRJ2XwKpp";
        public static string clientsecret = "EI5XM355w0XR_CXZJo64rvznW_d1xDvHQ64EouMAUJg5x7wyXekWgR6a2Td1Ul4ZWrju1sKoQByDB1O7";
        public static string url = "https://api.sandbox.paypal.com/v2/checkout/orders/{0}";
    }
}
