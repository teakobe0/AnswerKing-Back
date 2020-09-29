using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using DAL.IDAL;
using DAL.Model;
using JzAPI.zfb;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 支付控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Pay")]
    public class PayController : BaseController
    {
        private IOrderDAL _orderdal;
        private IClientDAL _clidal;
        public PayController(IOrderDAL orderdal, IClientDAL clidal)
        {
            _orderdal = orderdal;
            _clidal = clidal;
        }
        /// <summary>
        /// 获取支付的core_url
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPayUrl")]
        public ResultModel GetPayUrl([FromBody] Order order)
        {

            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            string orderNo = _orderdal.Add(order);
            string body = "";//商品描述 （商品名）

            int type = order.PayType;//1:微信,2:支付宝
            string total_fee = order.Price;//总价
            string name = order.Name;//订单名称
            string product_id = "";//商品表id
            switch (name)
            {
                case "1个月": product_id = "1"; break;
                case "3个月": product_id = "2"; break;
                case "6个月": product_id = "3"; break;
            }
           
            if (type == 1)
            {
                //构造支付地址信息
                WxPayService wxPayService = new WxPayService();
                //获取请求ip     
                //var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = HttpContext.Connection.RemoteIpAddress.ToString();
                }
                r.Data = wxPayService.GetWxSMPayUrl(orderNo, body, total_fee, ip, product_id);
                //返回支付的Url，前端ajax请求得到该url后，将该url赋值到存放图片的src中

            }
            else
            {
                IAopClient client = new DefaultAopClient(ZfbConfig.Gatewayurl, ZfbConfig.AppId, ZfbConfig.PrivateKey, "json", "1.0",
          ZfbConfig.SignType, ZfbConfig.AlipayPublicKey, ZfbConfig.CharSet, false);

                AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();

                request.BizContent = "{" +
                    "\"out_trade_no\":\"" + orderNo + "\"," +
                    "\"total_amount\":\"" + total_fee + "\"," +
                    "\"subject\":\"" + name + "\"" +
                    "}";

                AlipayTradePrecreateResponse response = client.Execute(request);

                JObject json1 = (JObject)JsonConvert.DeserializeObject(response.Body);
                string code = json1["alipay_trade_precreate_response"]["code"].ToString();

                if (code != "10000")
                {
                    r.Status = RmStatus.Error;

                    r.Msg = json1["alipay_trade_precreate_response"]["msg"].ToString();
                }
                string qr_code = json1["alipay_trade_precreate_response"]["qr_code"].ToString();
                r.Data = qr_code;

            }
            return r;

        }
        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPayResult")]
        public ResultModel GetPayResult(string orderNo, string type)
        {
            ResultModel result = new ResultModel();
            result.Status = RmStatus.OK;
            var order = _orderdal.GetOrder(orderNo);
            var cid = order.ClientId;
            var name = order.Name;

            //微信
            if (type == "1")
            {
                WxPayService wxPayService = new WxPayService();
                var xmlres = wxPayService.GetWxSMPayResult(orderNo);
                //失败
                if (xmlres.Element("xml").Element("trade_state").Value != "SUCCESS")
                {
                    result.Status = RmStatus.Error;
                    result.Msg = xmlres.Element("xml").Element("return_msg").Value;
                }
                else
                {
                    //成功，修改订单状态和客户身份、有效期
                    DateTime paytime = Convert.ToDateTime(xmlres.Element("xml").Element("time_end").Value);
                    _orderdal.ChangeStatus(orderNo, paytime);
                    var date = paytime.AddMonths(int.Parse(name.Substring(0, name.IndexOf('个'))));
                    _clidal.ChangeEffectiveDate(cid, date);

                }
            }
            //支付宝
            else
            {

                DefaultAopClient client = new DefaultAopClient(ZfbConfig.Gatewayurl, ZfbConfig.AppId, ZfbConfig.PrivateKey, "json", "1.0",
          ZfbConfig.SignType, ZfbConfig.AlipayPublicKey, ZfbConfig.CharSet, false);


                AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();


                request.BizContent = "{" +
                "\"out_trade_no\":\"" + orderNo + "\"" +
               "}";


                AlipayTradeQueryResponse response = client.Execute(request);


                JObject json1 = (JObject)JsonConvert.DeserializeObject(response.Body);
                string trade_status = json1["alipay_trade_query_response"]["trade_status"].ToString();

                if (trade_status != "TRADE_SUCCESS")
                {
                    result.Status = RmStatus.Error;
                    result.Msg = json1["alipay_trade_query_response"]["trade_status"].ToString();
                }
                else
                {

                    //成功，修改订单状态和客户身份、有效期
                    string sendPayDate = json1["alipay_trade_query_response"]["send_pay_date"].ToString();
                    DateTime paytime = Convert.ToDateTime(sendPayDate);
                    _orderdal.ChangeStatus(orderNo, paytime);
                    var date = paytime.AddMonths(int.Parse(name.Substring(0, name.IndexOf('个'))));
                    _clidal.ChangeEffectiveDate(cid, date);

                }

            }

            return result;
        }

    }
}