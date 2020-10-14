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
            try
            {
                var bidds = from x in _biddal.GetListByCid(ID)
                            select new
                            {
                                x.Id,
                                x.CreateTime,
                                x.QuestionId,
                                title= _quedal.GetQuestion(x.QuestionId).Title,
                                bstatus = _quedal.GetQuestion(x.QuestionId).Answerer == ID ? "已中竞拍" : "未中竞拍"
                            };
                r.Data = bidds;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
    }
}