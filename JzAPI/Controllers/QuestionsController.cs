using System;
using System.Collections.Generic;
using System.Linq;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static DAL.Tools.EnumAll;

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

                //查询客户积分
                var client = _clientdal.GetClientById(ID);
                if (client.Integral >= question.Currency)
                {
                    if (question.Id != 0)
                    {
                        if (question.Status == (int)questionStatus.Answer)
                        {
                            r.Msg = "该问题已经有人回答，不能关闭该问题。";
                        }
                        else
                        {
                            //修改question状态为已删除，退回发布问题的积分
                            _quedal.Del(question.Id);
                            question.Id = 0;
                        }
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
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "你的积分不足，不能发布问题";
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
                if (type == "time")
                {
                    ls = ls.Where(x => x.Answerer == 0&& x.Status != (int)questionStatus.Close).OrderByDescending(x => x.EndTime).ToList();
                }
                else if (type == "finish")
                {
                    ls = ls.Where(x => x.Status == (int)questionStatus.Complete).OrderByDescending(x => x.Id).ToList();
                }
                else if (type == "currency")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.Currency).ToList();
                }
                else
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.CreateTime).ToList();
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
                        //修改问题表里面answerer
                        _quedal.Update(questionid, int.Parse(bidding.CreateBy));
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
                var que = _quedal.GetQuestion(questionid);
                if (que.Status == (int)questionStatus.Answer)
                {
                    r.Data = _quedal.Evaluate(questionid, content, grade);

                    if ((int)r.Data == 0)
                    {
                        r.Status = RmStatus.Error;
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "未回答的问题不能评价。";
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
                var que = _quedal.GetQuestion(questionid);
                if (que.Status!= (int)questionStatus.Complete)
                {
                    r.Data = _quedal.ForService(questionid, reason);
                    if ((int)r.Data == 0)
                    {
                        r.Status = RmStatus.Error;
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "已完成的问题不能的申请客服。";
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
                var que = _quedal.GetQuestion(questionid);
                var bidd = _biddal.GetBidding(que.Id, que.Answerer);

                if (que.Status == (int)questionStatus.Answer)
                {
                    r.Data = _quedal.Edit(questionid);
                    if ((int)r.Data == 0)
                    {
                        r.Status = RmStatus.Error;
                    }
                }
                else
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "只有已回答的问题才能提交修改。";

                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }

        public class binfo
        {
            public Bidding bidding { get; set; }
            public string name { get; set; }
            public string image { get; set; }
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
                List<binfo> bls = new List<binfo>();
                binfo binfo = null;
                var que = _quedal.GetQuestion(questionid);
                //已选竞拍者
                string url = AppConfig.Configuration["imgurl"];
                if (que.Answerer != 0)
                {
                    answer = _ansdal.Answer(questionid);

                    if (answer != null&& answer.Content.Contains("<img src=\""))
                    {
                        answer.Content = answer.Content.Replace("<img src=\"", "<img src=\"" + url);
                    }
                    var bidding = _biddal.GetBidding(questionid, que.Answerer);
                    que.EndTime = bidding.EndTime;
                    que.Currency = bidding.Currency;
                    
                }
                else
                {
                    var biddings = _biddal.GetList(questionid);
                    if (biddings.Count() > 0)
                    {
                        foreach (var item in biddings)
                        {
                            binfo = new binfo();
                            binfo.bidding = item;
                            var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                            binfo.name = client.Name;
                            binfo.image = !string.IsNullOrEmpty(client.Image) ? AppConfig.Configuration["imgurl"] + client.Image : client.Image;
                            bls.Add(binfo);
                        }
                    }
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
                foreach (var item in question)
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