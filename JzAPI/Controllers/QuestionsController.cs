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
        private IFavouriteDAL _favouritedal;
        private IClientQuestionInfoDAL _clientqidal;
        public QuestionsController(IClientQuestionInfoDAL clientqidal, IQuestionDAL quedal, IBiddingDAL biddal, IAnswerDAL ansdal, IClientDAL clientdal, INoticeDAL noticedal, IFavouriteDAL favouritedal)
        {
            _quedal = quedal;
            _biddal = biddal;
            _ansdal = ansdal;
            _clientdal = clientdal;
            _noticedal = noticedal;
            _favouritedal = favouritedal;
            _clientqidal = clientqidal;
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
                string url = AppConfig.Configuration["imgurl"];
                if (!string.IsNullOrEmpty(question.Title) && !string.IsNullOrEmpty(question.Content))
                {
                    question.Img = !string.IsNullOrEmpty(question.Img) ? question.Img.Replace(url, "") : question.Img;
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
                        else if (question.Status == (int)questionStatus.Choose)
                        {
                            r.Msg = "该问题已选竞拍者，不能重新发布";
                            r.Status = RmStatus.Error;
                        }
                        else
                        {
                            int num = _quedal.Edit(question);
                            //有改动，去删除该问题下面的竞拍记录
                            if (num == 1)
                            {
                                _biddal.DelLs(question.Id);
                            }
                        }
                    }
                    else
                    {
                        question.CreateBy = ID.ToString();
                        r.Data = _quedal.Add(question);
                    }
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
        public ResultModel QuestionPage(string name, int classes, string type, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                que que = null;
                List<que> ques = new List<que>();
                var ls = _quedal.GetList().Where(x => x.Status != (int)questionStatus.Close && x.EndTime >= DateTime.Now).OrderByDescending(x => x.Id).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    ls = ls.Where(x => x.Title.Contains(name) || x.Content.Contains(name)).ToList();
                }
                if (classes != 0)
                {
                    ls = ls.Where(x => x.Type == classes).ToList();
                }
                if (type == "new")
                {
                    ls = ls.Where(x => x.Answerer == 0).OrderByDescending(x => x.CreateTime).ToList();
                }
                else if (type == "moods")
                {
                    ls = ls.OrderByDescending(x => x.Views).ToList();
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
        public ResultModel MyQuestionPage(string name, int classes, string type, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                queinfo queinfo = null;
                List<queinfo> queinfos = new List<queinfo>();
                string url = AppConfig.Configuration["imgurl"];
                var ls = _quedal.GetList().Where(x => x.Status != (int)questionStatus.Close && x.EndTime >= DateTime.Now).OrderByDescending(x => x.Id).ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    ls = ls.Where(x => x.Title.Contains(name) || x.Content.Contains(name)).ToList();
                }
                if (classes != 0)
                {
                    ls = ls.Where(x => x.Type == classes).ToList();
                }
                if (type == "new")
                {
                    ls = ls.Where(x => x.Answerer == 0).OrderByDescending(x => x.CreateTime).ToList();
                }
                else if (type == "moods")
                {
                    ls = ls.OrderByDescending(x => x.Views).ToList();
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
                    queinfo.favourite = _favouritedal.GetNum(item.Id);
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
            public int favourite { get; set; }
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
                        r.Data=_quedal.Update(questionid, bidding);
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

      
        public class que
        {
            public Question question { get; set; }
            public string qname { get; set; }
            public string qimage { get; set; }
            public int favourite { get; set; }
        }

        /// <summary>
        /// 问题详情
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Details")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Details(int questionid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                que que = new que();
                string url = AppConfig.Configuration["imgurl"];
                //增加浏览次数
                var question = _quedal.GetQuestionJk(questionid);
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
                que.favourite = _favouritedal.GetNum(questionid);
                que.qname = client.Name;
                que.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                r.Data = que;
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
        public ResultModel MyQuestion(int status, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            try
            {
                List<qinfo> ls = new List<qinfo>();
                qinfo qinfo = null;
                var question = _quedal.GetList(ID);
                if (status != 0)
                {
                    question = question.Where(x => x.Status == status).ToList();
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
                //竞拍中的提问
                int bidding = que.Where(x => x.Status == (int)questionStatus.Bidding).Count();
                //待完成的提问
                int no = que.Where(x => x.Status == (int)questionStatus.Choose).Count();
                //待评价的提问
                int evaluate = que.Where(x => x.Status == (int)questionStatus.Answer).Count();
                //已完成的提问
                int complete = que.Where(x => x.Status == (int)questionStatus.Complete).Count();
                r.Data = new { bidding, no, evaluate, complete };

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
                //竞拍中的回答
                int bidding = 0;
                var bls = _biddal.GetListByCid(ID);
                foreach (var item in bls)
                {
                    var q = _quedal.GetQuestion(item.QuestionId);
                    if (q.Status == (int)questionStatus.Bidding)
                    {
                        bidding += 1;
                    }
                }
                var que = _quedal.GetListByClientid(ID);
                //待完成的回答
                int no = que.Where(x => x.Status == (int)questionStatus.Choose).Count();
                //待评价的回答
                int evaluate = que.Where(x => x.Status == (int)questionStatus.Answer).Count();
                //已完成的回答
                int complete = que.Where(x => x.Status == (int)questionStatus.Answer).Count();
                r.Data = new { bidding, no, evaluate, complete };

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
                    item.Img = !string.IsNullOrEmpty(item.Img) ? url + item.Img : item.Img;
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
        /// <summary>
        /// 查询科目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Classes")]
        public ResultModel Classes()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                Dictionary<int, string> type = new Dictionary<int, string>();
                type.Add(1, "经济 economic");
                type.Add(2, "金融 finance");
                r.Data = type;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 完成的问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CompleteQuestions")]
        public ResultModel CompleteQuestions(int type, int clientid, int pagenum = 1, int pagesize = 10)
        {
            ResultModel r = new ResultModel();
            PageData page = new PageData();
            r.Status = RmStatus.OK;
            List<que> qls = new List<que>();
            que que = null;
            try
            {
                string url = AppConfig.Configuration["imgurl"];
                var ls = _quedal.GetList().Where(x => x.Answerer == clientid && x.Status == (int)questionStatus.Complete);
                if (clientid != 0)
                {
                    ls = ls.Where(x => x.Answerer == clientid);
                }
                if (type != 0)
                {
                    ls = ls.Where(x => x.Type == type);
                }
                foreach (var item in ls.Skip(pagesize * (pagenum - 1)).Take(pagesize))
                {
                    que = new que();
                    que.question = item;
                    var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                    que.qname = client.Name;
                    que.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    qls.Add(que);
                }
                page.Data = qls;
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
        /// 保存补充材料
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Save([FromBody] Question question)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                if (!string.IsNullOrEmpty(question.Img))
                {
                    string url = AppConfig.Configuration["imgurl"];
                    question.Img = "|" + question.Img.Replace(url, "");
                }
                r.Data = _quedal.UpdateImg(question.Id, question.Img);
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
        /// 查询该客户竞拍中的问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBiddingQuestions")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel GetBiddingQuestions(int biddingid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            info info = null;
            List<info> ls = new List<info>();
            try
            {
                var qls = _quedal.GetList().Where(x => x.EndTime > DateTime.Now && x.Status == (int)questionStatus.Bidding && x.CreateBy == ID.ToString());
                foreach (var item in qls)
                {
                    info = new info();
                    var bidding = _biddal.GetBidding(item.Id, biddingid);
                    if (bidding == null)
                    {
                        info.id = item.Id;
                        info.title = item.Title;
                        ls.Add(info);
                    }
                }
                r.Data = ls;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class info
        {
            public string title { get; set; }
            public int id { get; set; }
        }
        /// <summary>
        /// 根据问题id查询竞拍信息
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BiddingInfo")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel BiddingInfo(int questionid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                List<Answer> als = new List<Answer>();
                List<binfo> bls = new List<binfo>();
                binfo binfo = null;
                string url = AppConfig.Configuration["imgurl"];
                var question = _quedal.GetQuestion(questionid);
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
                    binfo = new binfo();
                    binfo.bidding = bidding;
                    if (ID == question.Answerer)
                    {
                        var client = _clientdal.GetClientById(int.Parse(question.CreateBy));
                        binfo.bname = client.Name;
                        binfo.bimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                    }
                    else
                    {
                        var cli = _clientdal.GetClientById(int.Parse(bidding.CreateBy));
                        binfo.bname = cli.Name;
                        binfo.bimage = !string.IsNullOrEmpty(cli.Image) ? url + cli.Image : cli.Image;
                    }
                    bls.Add(binfo);
                }
                else
                {
                    var biddings = _biddal.GetList(questionid);
                    foreach (var item in biddings)
                    {
                        binfo = new binfo();
                        binfo.bidding = item;
                        var client = _clientdal.GetClientById(int.Parse(item.CreateBy));
                        binfo.bname = client.Name;
                        binfo.bimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                        binfo.integral = client.Integral;
                        var clientinfo = _clientqidal.GetClientQuestionInfo(int.Parse(item.CreateBy));
                        binfo.GoodReviewRate = clientinfo.GoodReviewRate;
                        binfo.qnum = _quedal.GetListByClientid(int.Parse(item.CreateBy)).Count();
                        bls.Add(binfo);
                    }
                }
                r.Data = new { bls, als };
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
            public int integral { get; set; }
            public decimal GoodReviewRate { get; set; }
            public int qnum { get; set; }

        }
    }
}