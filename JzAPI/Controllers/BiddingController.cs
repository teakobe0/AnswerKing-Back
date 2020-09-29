using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 竞拍表控制层
    /// </summary>
    [Produces("application/json")]
    [Route("api/Bidding")]
    public class BiddingController : BaseController
    {
        private IBiddingDAL _biddal;
        private IQuestionDAL _quedal;
        public BiddingController(IBiddingDAL biddal, IQuestionDAL quedal)
        {
            _biddal = biddal;
            _quedal = quedal;
        }
        /// <summary>
        /// 回应(竞拍)问题
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Bidding bidding)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var question = _quedal.GetQuestion(bidding.QuestionId);
                var bid = _biddal.GetBidding(bidding.QuestionId, ID);
                //发布人不能竞拍自己发布的问题
                if (question.CreateBy == ID.ToString())
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "发布人不能竞拍自己发布的问题";
                }
                //已经竞拍的不能重复竞拍
                else if (bid != null)
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "已经竞拍过该问题，不能重复竞拍";
                }
                else
                {
                    bidding.CreateBy = ID.ToString();
                    r.Data = _biddal.Add(bidding);
                }

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        ///我竞拍的问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyBidding")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyBidding()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            List<biddinfo> ls = new List<biddinfo>();
            biddinfo binfo = null;
            try
            {
                var bidds = _biddal.GetListByCid(ID);
                foreach (var item in bidds)
                {
                    binfo = new biddinfo();
                    binfo.bidding = item;
                    var que = _quedal.GetQuestion(item.QuestionId);
                    if (que.Answerer == ID)
                    {
                        binfo.bstatus = "已中竞拍";
                    }
                    else
                    {
                        binfo.bstatus = "未中竞拍";
                    }
                    binfo.title = que.Title;
                    ls.Add(binfo);
                }
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class biddinfo
        {
            public Bidding bidding { get; set; }
            public string title { get; set; }
            public string bstatus { get; set; }

        }
    }
}