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
        private IClientQuestionInfoDAL _clientqidal;
        public BiddingController(IBiddingDAL biddal, IQuestionDAL quedal, IClientQuestionInfoDAL clientqidal)
        {
            _biddal = biddal;
            _quedal = quedal;
            _clientqidal = clientqidal;
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
        /// 编辑
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit([FromBody] Bidding bidding)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                //竞拍的问题的状态为正在竞拍
                var que = _quedal.GetQuestion(bidding.QuestionId);
                if (que.Status == (int)questionStatus.Bidding)
                {
                    r.Data = _biddal.Edit(bidding);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "竞拍价格不能修改。";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        ///我竞拍中问答
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyBidding")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyBidding(int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                List<Question> ls = new List<Question>();
                Question question = null;
                var bls = _biddal.GetListByCid(ID);
                foreach (var item in bls)
                {
                    var q = _quedal.GetQuestion(item.QuestionId);
                    if (q.Status == (int)questionStatus.Bidding && q.IsDel == false)
                    {
                        question = new Question();
                        question = q;
                        ls.Add(question);
                    }
                }
                var model = from x in ls.Skip(pagesize * (pagenum - 1)).Take(pagesize)
                            select new
                            {
                                x.Id,
                                x.CreateTime,
                                x.Title,
                                x.Status,
                                x.EndTime,
                                currency = _biddal.GetBidding(x.Id, ID).Currency,
                                bnum = _biddal.GetList(x.Id).Count()
                            };
                page.Data = model;
                page.PageTotal = ls.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 邀请竞拍
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="biddingid">被邀请人id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Ask")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Ask(int questionid, int biddingid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                if (questionid != 0 && biddingid != 0)
                {
                    Bidding bidding = new Bidding();
                    bidding.QuestionId = questionid;
                    bidding.CreateBy = biddingid.ToString();
                    r.Data = _biddal.Add(bidding);
                }
                else
                {
                    r.Status = RmStatus.Error;
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
    }
}