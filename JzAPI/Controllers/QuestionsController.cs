using System;
using System.Collections.Generic;
using System.Linq;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{

    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Questions")]
    public class QuestionsController : BaseController
    {
        private IQuestionDAL _quedal;
        private IBiddingDAL _biddal;
        private IAnswerDAL _ansdal;
        private IClientDAL _clientdal;
        public QuestionsController(IQuestionDAL quedal, IBiddingDAL biddal, IAnswerDAL ansdal, IClientDAL clientdal)
        {
            _quedal = quedal;
            _biddal = biddal;
            _ansdal = ansdal;
            _clientdal = clientdal;
        }
        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Question question)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {

                if (question.Id != 0)
                {
                    //修改question状态为已删除
                    _quedal.Del(question.Id);
                    question.Id = 0;
                }
                if (!string.IsNullOrEmpty(question.Title) && !string.IsNullOrEmpty(question.Content))
                {

                    question.CreateBy = ID.ToString();
                    r.Data = _quedal.Add(question);
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "问题的标题和内容不能为空";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 问题列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QuestionPage")]
        public ResultModel QuestionPage(string type, int pagenum = 1, int pagesize = 40)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                var ls = _quedal.GetList();
                if (string.IsNullOrEmpty(type))
                {
                    ls = ls.OrderByDescending(x => x.CreateTime).ToList();
                }
                if (type == "time")
                {
                    ls = ls.OrderByDescending(x => x.EndTime).ToList();

                }
                else
                {
                    ls = ls.OrderByDescending(x => x.Currency).ToList();
                }

                page.Data = ls.Skip(pagesize * (pagenum - 1)).Take(pagesize);
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
        /// 回应(竞拍)问题
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBidding")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel AddBidding([FromBody] Bidding bidding)
        {

            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                bidding.CreateBy = ID.ToString();
                r.Data = _biddal.Add(bidding);
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 选择竞拍者
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="clientid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Choose")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Choose(int questionid, int clienid, string password)
        {

            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var client = _clientdal.GetClientById(ID);
                if (client.Password == password)
                {
                    if (questionid != 0 && clienid != 0)
                    {
                        var bidding = _biddal.GetBidding(questionid, clienid);
                        r.Data = bidding.EndTime;
                    }
                    else
                    {
                        r.Status = RmStatus.Error;
                    }

                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "密码错误";
                }


            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="content">内容</param>
        /// <param name="grade">评分，5：好评，1：差评</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Evaluate")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Evaluate(int questionid, string content, int grade)
        {

            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _quedal.Evaluate(questionid, content, grade);
                if ((int)r.Data == 0)
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
        /// <summary>
        /// 申请客服
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ForService")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel ForService(int questionid, string reason)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _quedal.ForService(questionid, reason);
                if ((int)r.Data == 0)
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
        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="quetionid"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Edit")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Edit(int questionid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _quedal.Edit(questionid);
                if ((int)r.Data == 0)
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
        /// <summary>
        /// 问题详情
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Details")]
        public ResultModel Details(int questionid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                Answer answer = null;
                List<Bidding> bls = null;
                var que = _quedal.GetQuestion(questionid);
                //已选择竞拍者
                if (que.Answerer != 0)
                {
                    answer = _ansdal.Answer(questionid);
                }
                else
                {
                    bls = _biddal.GetList(questionid);
                }

                r.Data = new { que, bls, answer };
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class qinfo
        {
            public Question que { get; set; }
            public int number { get; set; }
        }

        /// <summary>
        ///我发布的问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyQuestion")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyQuestion()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                List<qinfo> ls = new List<qinfo>();
                qinfo qinfo = null;
                var question = _quedal.GetList(ID);
                foreach(var item in question)
                {
                    qinfo = new qinfo();
                    qinfo.que = item;
                    var bidding = _biddal.GetList(item.Id);
                    qinfo.number = bidding.Count();
                    ls.Add(qinfo);
                }
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        

    }
}