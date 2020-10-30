using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DAL;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 客户问题信息表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/ClientQuestionInfo")]
    public class ClientQuestionInfoController : BaseController
    {
        private IClientQuestionInfoDAL _clientqidal;
        private IClientDAL _clientdal;
        private IQuestionDAL _quedal;
        private IBiddingDAL _biddal;
        public ClientQuestionInfoController(IBiddingDAL biddal, IClientQuestionInfoDAL clientqidal, IClientDAL clientdal, IQuestionDAL quedal)
        {
            _clientqidal = clientqidal;
            _clientdal = clientdal;
            _quedal = quedal;
            _biddal = biddal;
        }
        /// <summary>
        /// 根据客户id查询客户问题信息
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetClientQuestionInfo")]
        public ResultModel GetClientQuestionInfo(int clientid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            info info = new info();
            try
            {
                    string url = AppConfig.Configuration["imgurl"];
                    info.cqinfo = _clientqidal.GetClientQuestionInfoJK(clientid);
                    var client = _clientdal.GetClientById(clientid);
                    info.qname = client.Name;
                    info.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    var overque = _quedal.GetListByClientid(clientid).Where(x => x.Status == (int)questionSign.Overtime);
                    if (overque != null)
                    {
                        int num = overque.Count();
                        int total = info.cqinfo.CompletedQuestions + num;
                        info.overtimerate = Math.Round((decimal)num / total, 2);
                    }
                    else
                    {
                        info.overtimerate = 0;
                    }
                    //竞拍中的回答
                    int bidding = 0;
                    var bls = _biddal.GetListByCid(clientid);
                    foreach (var item in bls)
                    {
                        var q = _quedal.GetQuestion(item.QuestionId);
                        if (q.Status == (int)questionStatus.Bidding)
                        {
                            bidding += 1;
                        }
                    }
                    info.bunm = bidding;
                    r.Data = info;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class info
        {
            public ClientQuestionInfo cqinfo { get; set; }
            public string qname { get; set; }
            public string qimage { get; set; }
            public decimal overtimerate { get; set; }
            public int bunm { get; set; }
        }
    }
}