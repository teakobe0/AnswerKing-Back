using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using JzAPI.paypal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : BaseController
    {
        private IOrderDAL _orderdal;
        private IClientDAL _clidal;
        public OrderController(IOrderDAL orderdal, IClientDAL clidal)
        {
            _orderdal = orderdal;
            _clidal = clidal;
        }
        /// <summary>
        /// 获取客户订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrder")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel GetOrder()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data =_orderdal.GetListByClientid(ID);
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 查询商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGoods")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel GetGoods()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string type = AppConfig.Configuration["Goods"];
            string[] strArray = type.Split(';');
            goods g = null;
            List<goods> ls = new List<goods>();
            foreach (var item in strArray)
            {
                g = new goods();
                var arr = item.Split(":");
                g.name = arr[0].ToString();
                g.price = arr[1];
                ls.Add(g);
            }
            r.Data = ls;
            return r;
        }
        public class goods
        {
            public string name { get; set; }
            public string price { get; set; }
        }
        /// <summary>
        ///  获取paypal订单,保存订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="clientid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrder")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel SaveOrder(string orderId, int clientid)
        {
            bool issameprice = false;
            ResultModel r = new ResultModel();
            var json = HttpHelper.Get(
                string.Format(PaypalConfig.url, orderId),
                PaypalConfig.cilentid, PaypalConfig.clientsecret,
                Encoding.UTF8);
            var payment = Newtonsoft.Json.JsonConvert.DeserializeObject<Payment>(json);
            DAL.Model.Order order = new DAL.Model.Order();
            var payprice = payment.purchase_units[0].amount.value.ToString();
            var ordername = "";
            string type = AppConfig.Configuration["Goods"];
            string[] strArray = type.Split(';');
            foreach (var item in strArray)
            {
                var arr = item.Split(":");
                var name = arr[0].ToString();
                var price = arr[1];
                if (payprice == price.ToString())
                {
                    issameprice = true;
                    ordername = name;
                }
            }
            if (issameprice)
            {
                order.Price = payprice;
                order.ClientId = clientid == 0 ? ID : clientid;
                order.Name = ordername;
                order.CreateTime = payment.create_time == null ? DateTime.Parse(payment.update_time) : DateTime.Parse(payment.create_time);
                order.OrderNo = payment.id;
                order.Status = (int)OrderStatus.paid;
                order.Currency = payment.purchase_units[0].amount.currency_code;
                order.PayType = (int)PayType.paypal;
                order.PayTime = DateTime.Parse(payment.update_time);
                r.Data = _orderdal.AddPaypal(order);

                if ((int)r.Data == 1)
                {
                    //修改客户身份、有效期
                    var date = order.PayTime.AddMonths(int.Parse(ordername.Substring(0, ordername.IndexOf('个'))));
                    _clidal.ChangeEffectiveDate(ID, date);
                    r.Status = RmStatus.OK;
                }
                else
                {
                    r.Msg = "失败";
                    r.Status = RmStatus.Error;
                    Utils.WriteInfoLog("保存db异常" + "，订单号：" + payment.id + "，时间：" + DateTime.Parse(payment.update_time) + ",金额：" + payprice);
                }
            }
            else
            {
                r.Msg = "支付失败";
                r.Status = RmStatus.Error;
                Utils.WriteInfoLog("支付异常" + "，订单号：" + payment.id + "，时间：" + DateTime.Parse(payment.update_time) + ",金额：" + payprice);
            }
            return r;
        }
    }
}