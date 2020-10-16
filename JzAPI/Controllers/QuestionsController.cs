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
    /// <summary>
    /// 问题表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Questions")]
    public class QuestionsController : BaseController
    {
        private IQuestionDAL _quedal;
        private IBiddingDAL _biddal;
        private IAnswerDAL _ansdal;
        private IClientDAL _clientdal;
        private INoticeDAL _noticedal;
        public QuestionsController(IQuestionDAL quedal, IBiddingDAL biddal, IAnswerDAL ansdal, IClientDAL clientdal, INoticeDAL noticedal)
        {
            _quedal = quedal;
            _biddal = biddal;
            _ansdal = ansdal;
            _clientdal = clientdal;
            _noticedal = noticedal;
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
                List<Bidding> bls = null;
                if (question.Id != 0)
                {
                    if (question.Status == (int)questionStatus.Answer)
                    {
                        r.Msg = "该问题已经有人回答，不能重新发布。";
                        r.Status = RmStatus.Error;
                    }
                    else if (question.Status == (int)questionStatus.ForService)
                    {
                        r.Msg = "该问题已经申请客服，不能重新发布。";
                        r.Status = RmStatus.Error;
                    }
                    else if (question.Status == (int)questionStatus.Complete)
                    {
                        r.Msg = "该问题已经完成，不能重新发布。";
                        r.Status = RmStatus.Error;
                    }
                    else if (question.Status == (int)questionStatus.Close)
                    {
                        r.Msg = "该问题已经关闭，不能重新发布。";
                        r.Status = RmStatus.Error;
                    }
                    else
                    {
                        //修改question状态为已删除
                        bls = _biddal.GetList(question.Id);
                        _quedal.Del(question.Id);
                        question.Id = 0;
                    }
                }
                if (r.Msg == null)
                {
                    if (!string.IsNullOrEmpty(question.Title) && !string.IsNullOrEmpty(question.Content))
                    {

                        question.CreateBy = ID.ToString();
                        string url = AppConfig.Configuration["imgurl"];
                        question.Img = !string.IsNullOrEmpty(question.Img) ? question.Img.Replace(url, "") : question.Img;
                        r.Data = _quedal.Add(question);
                        //发通知
                        if (bls != null)
                        {
                            Notice notice = null;
                            string qurl = AppConfig.Configuration["questionurl"] + question.Id;
                            foreach (var item in bls)
                            {
                                notice = new Notice();
                                notice.Type = (int)noticeType.System;
                                notice.ReceiveId = int.Parse(item.CreateBy);
                                notice.ContentsUrl = "您竞拍的原问题:" + item.QuestionId + ",内容已经发生改变,请您重新参与竞拍,<a href=" + qurl + ">新问题地址</a>";
                                _noticedal.Add(notice);
                            }
                        }

                    }
                    else
                    {
                        r.Status = RmStatus.Error;
                        r.Msg = "问题的标题和内容不能为空";
                    }
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
        public ResultModel QuestionPage(string name, string type, int pagenum = 1, int pagesize = 40)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                que que = null;
                List<que> ques = new List<que>();
                var ls = _quedal.GetList().Where(x => x.EndTime >= DateTime.Now);
                if (!string.IsNullOrEmpty(name))
                {
                    ls = ls.Where(x => x.Title.Contains(name) || x.Content.Contains(name)).ToList();
                }
                if (type == "new")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.CreateTime).ToList();
                }
                else if (type == "currency")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.Currency).ToList();
                }
                else if (type == "finish")
                {
                    ls = ls.Where(x => x.Status == (int)questionStatus.Complete).OrderByDescending(x => x.Id).ToList();
                }
                else if (type == "retime")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderBy(x => x.EndTime).ToList();
                }
                else
                {
                    ls = ls.OrderByDescending(x => x.Id).ToList();
                }
                //后期是否需要图片显示
                string url = AppConfig.Configuration["imgurl"];
                foreach (var item in ls)
                {
                    que = new que();
                    var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                    if (!string.IsNullOrEmpty(item.Img))
                    {
                        if (item.Img.Contains("|"))
                        {
                            Array qimgs = item.Img.Split("|");
                            item.Img = "";
                            foreach (var t in qimgs)
                            {
                                item.Img += "|" + url + t;
                            }
                            item.Img = item.Img.Substring(1);
                        }
                        else
                        {
                            item.Img = url + item.Img;
                        }
                    }
                    que.question = item;
                    que.qname = client.Name;
                    que.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    ques.Add(que);
                }
                page.Data = ques.Skip(pagesize * (pagenum - 1)).Take(pagesize);
                page.PageTotal = ques.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 我的问题列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("MyQuestionPage")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel MyQuestionPage(string name, string type, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                queinfo queinfo = null;
                List<queinfo> queinfos = new List<queinfo>();
                string url = AppConfig.Configuration["imgurl"];
                var ls = _quedal.GetList().Where(x => x.EndTime >= DateTime.Now);
                if (!string.IsNullOrEmpty(name))
                {
                    ls = ls.Where(x => x.Title.Contains(name) || x.Content.Contains(name)).ToList();
                }
                if (type == "new")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.CreateTime).ToList();
                }
                else if (type == "currency")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderByDescending(x => x.Currency).ToList();
                }
                else if (type == "finish")
                {
                    ls = ls.Where(x => x.Status == (int)questionStatus.Complete).OrderByDescending(x => x.Id).ToList();
                }
                else if (type == "retime")
                {
                    ls = ls.Where(x => x.Answerer == 0 && x.Status != (int)questionStatus.Close).OrderBy(x => x.EndTime).ToList();
                }
                else
                {
                    ls = ls.OrderByDescending(x => x.Id).ToList();
                }
                foreach (var item in ls)
                {
                    queinfo = new queinfo();
                    var bidding = _biddal.GetBidding(item.Id, ID);
                    var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                    if (!string.IsNullOrEmpty(item.Img))
                    {
                        if (item.Img.Contains("|"))
                        {
                            Array qimgs = item.Img.Split("|");
                            item.Img = "";
                            foreach (var t in qimgs)
                            {
                                item.Img += "|" + url + t;
                            }
                            item.Img = item.Img.Substring(1);
                        }
                        else
                        {
                            item.Img = url + item.Img;
                        }
                    }
                    queinfo.que = item;
                    queinfo.qname = client.Name;
                    queinfo.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    if (bidding != null && item.Status == (int)questionStatus.Bidding)
                    {
                        queinfo.bidd = "正在竞拍";
                    }
                    if (item.CreateBy == ID.ToString())
                    {
                        queinfo.myque = "我的提问";
                    }
                    if (item.Answerer == ID && item.Status == (int)questionStatus.Complete)
                    {
                        queinfo.myanswer = "我的回答";
                    }
                    queinfos.Add(queinfo);
                }
                page.Data = queinfos.Skip(pagesize * (pagenum - 1)).Take(pagesize);
                page.PageTotal = queinfos.Count();
                r.Data = page;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }

        public class queinfo
        {
            public Question que { get; set; }
            public string qname { get; set; }
            public string qimage { get; set; }
            public string myque { get; set; }
            public string bidd { get; set; }
            public string myanswer { get; set; }
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
                        // 查询发布人积分
                        if (client.Integral < bidding.Currency)
                        {
                            r.Msg = "积分不足，不能选择该竞拍者。";
                            r.Status = RmStatus.Error;
                            r.Data = null;
                        }
                        //扣除发布人积分
                        _clientdal.Deduct(ID, bidding.Currency);
                        //更新问题表
                        _quedal.Update(questionid, bidding);
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

        public class binfo
        {
            public Bidding bidding { get; set; }
            public string bname { get; set; }
            public string bimage { get; set; }
        }
        public class que
        {
            public Question question { get; set; }
            public string qname { get; set; }
            public string qimage { get; set; }
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
                List<Answer> als = new List<Answer>();
                List<binfo> bls = new List<binfo>();
                binfo binfo = null;
                que que = new que();
                string url = AppConfig.Configuration["imgurl"];
                var question = _quedal.GetQuestion(questionid);
                if (!string.IsNullOrEmpty(question.Img))
                {
                    if (question.Img.Contains("|"))
                    {
                        Array qimgs = question.Img.Split("|");
                        question.Img = "";
                        foreach (var t in qimgs)
                        {
                            question.Img += "|" + url + t;
                        }
                        question.Img = question.Img.Substring(1);
                    }
                    else
                    {
                        question.Img = url + question.Img;
                    }
                }
                var client = _clientdal.GetClientById(int.Parse(question.CreateBy));
                que.question = question;
                que.qname = client.Name;
                que.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                //已选竞拍者
                if (question.Answerer != 0)
                {
                    als = _ansdal.GetLs(questionid);
                    foreach (var item in als)
                    {
                        if (!string.IsNullOrEmpty(item.Img))
                        {
                            if (item.Img.Contains("|"))
                            {
                                Array aimgs = item.Img.Split("|");
                                item.Img = "";
                                foreach (var m in aimgs)
                                {
                                    item.Img += "|" + url + m;
                                }
                                item.Img = item.Img.Substring(1);
                            }
                            else
                            {
                                item.Img = url + item.Img;
                            }
                        }
                    }
                    var bidding = _biddal.GetBidding(questionid, question.Answerer);
                    question.EndTime = bidding.EndTime;
                    binfo = new binfo();
                    binfo.bidding = bidding;
                    client = _clientdal.GetClientById(int.Parse(bidding.CreateBy));
                    binfo.bname = client.Name;
                    binfo.bimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    bls.Add(binfo);
                }
                else
                {
                    var biddings = _biddal.GetList(questionid);
                    foreach (var item in biddings)
                    {
                        binfo = new binfo();
                        binfo.bidding = item;
                        client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                        binfo.bname = client.Name;
                        binfo.bimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                        bls.Add(binfo);
                    }

                }

                r.Data = new { que, bls, als };
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
        public ResultModel MyQuestion(string name, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                List<qinfo> ls = new List<qinfo>();
                qinfo qinfo = null;
                var question = _quedal.GetList(ID);
                if (!string.IsNullOrEmpty(name))
                {
                    question = question.Where(x => x.Title.Contains(name) || x.Content.Contains(name)).ToList();
                }
                string url = AppConfig.Configuration["imgurl"];
                foreach (var item in question)
                {
                    qinfo = new qinfo();
                    if (!string.IsNullOrEmpty(item.Img))
                    {
                        if (item.Img.Contains("|"))
                        {
                            Array qimgs = item.Img.Split("|");
                            item.Img = "";
                            foreach (var t in qimgs)
                            {
                                item.Img += "|" + url + t;
                            }
                            item.Img = item.Img.Substring(1);
                        }
                        else
                        {
                            item.Img = url + item.Img;
                        }
                    }
                    qinfo.que = item;
                    var bidding = _biddal.GetList(item.Id);
                    qinfo.number = bidding.Count();
                    ls.Add(qinfo);
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
        /// 问题状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("QuestionStatus")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel QuestionStatus()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var que = _quedal.GetList(ID);
                //竞拍中
                int bnum = que.Where(x => x.Status == (int)questionStatus.Bidding).Count();
                //待回答
                int nonum = que.Where(x => x.Status == (int)questionStatus.Choose).Count();
                //已回答
                int answernum = que.Where(x => x.Status == (int)questionStatus.Answer).Count();

                r.Data = new { bnum, nonum, answernum };

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 回答状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AnswerStatus")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel AnswerStatus()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                var que = _quedal.GetListByClientid(ID);
                //竞拍中
                int bnum = que.Where(x => x.Status == (int)questionStatus.Bidding).Count();
                //待回答
                int nonum = que.Where(x => x.Status == (int)questionStatus.Choose).Count();
                //已回答
                int answernum = que.Where(x => x.Status == (int)questionStatus.Answer).Count();

                r.Data = new { bnum, nonum, answernum };

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }

        /// <summary>
        /// 最新问题ls
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNewQuestion")]
        public ResultModel GetNewQuestion(DateTime time)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;

            try
            {
                queinfo queinfo = null;
                List<queinfo> queinfos = new List<queinfo>();
                var ls = _quedal.GetList().Where(x => x.CreateTime >= time).OrderByDescending(x => x.CreateTime);
                string url = AppConfig.Configuration["imgurl"];
                foreach (var item in ls)
                {
                    var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                    queinfo = new queinfo();
                    queinfo.qname = client.Name;
                    queinfo.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    queinfo.que = item;
                    queinfos.Add(queinfo);
                }
                r.Data = queinfos;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        ///获取数量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNumber")]
        public ResultModel GetNumber(DateTime time)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                int num = 0;
                DateTime datetime = DateTime.MinValue;
                var ls = _quedal.GetList().Where(x => DateTime.Parse(x.CreateTime.ToString("G")) > time).OrderByDescending(x => x.CreateTime);
                if (ls.Count() > 0)
                {
                    num = ls.Count();
                    datetime = ls.FirstOrDefault().CreateTime;
                }
                r.Data = new { num, datetime };
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="imgurl"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("RemoveImg")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel RemoveImg(int questionid, string imgurl)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                if (questionid != 0)
                {
                    //删除数据库图片
                    int clientid = int.Parse(_quedal.GetQuestion(questionid).CreateBy);
                    if (clientid == ID)
                    {
                        r.Data = _quedal.DelImg(questionid, imgurl);
                    }
                    else
                    {
                        r.Status = RmStatus.Error;
                        r.Msg = "你没有权限操作";
                    }
                }
                if (r.Msg == null)
                {
                    //转换为绝对路径
                    string path = AppConfig.Configuration["uploadurl"] + imgurl;
                    //删除本地
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
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