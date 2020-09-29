using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    /// <summary>
    /// 问题表数据访问层
    /// </summary>
    public class QuestionDAL : BaseDAL, IQuestionDAL
    {
        public QuestionDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Question> GetList()
        {
            return GetListData().ToList();

        }
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <returns></returns>
        public List<Question> GetList(int clientId)
        {
            var list = GetListData().OrderByDescending(x => x.Id).ToList();
            if (clientId != 0)
            {
                list = list.Where(x => x.CreateBy == clientId.ToString()).ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据客户id检索(回答者)
        /// </summary>
        /// <returns></returns>
        public List<Question> GetListByClientid(int clientId)
        {
            var list = GetListData().ToList();
            if (clientId != 0)
            {
                list = list.Where(x => x.Answerer == clientId).ToList();
            }
            return list;
        }

        private IQueryable<Question> GetListData()
        {
            return _context.Question.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public Question Add(Question question)
        {

            question.Status = (int)questionStatus.Save;
            question.CreateTime = DateTime.Now;
            _context.Question.Add(question);
            //先扣除客户发布问题的积分
            var client = _context.Client.FirstOrDefault(x => x.Id == int.Parse(question.CreateBy));
            client.Integral -= question.Currency;
            _context.Client.Update(client);
            //积分记录表
            IntegralRecords integral = new IntegralRecords();
            integral.Integral = question.Currency;
            integral.CreateBy = client.Id.ToString();
            integral.CreateTime = DateTime.Now;
            integral.Source = "发布问题";
            _context.IntegralRecords.Add(integral);
            _context.SaveChanges();
            return question;

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.IsDel = true;
                que.Status = (int)questionStatus.Close;
                //删除竞拍表关于该问题的所有竞拍记录
                var biddings = _context.Bidding.Where(x => x.QuestionId == id);
                foreach (var i in biddings)
                {
                    i.IsDel = true;
                }
                var fclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(que.CreateBy));
                if (que.Status == (int)questionStatus.Choose)
                {
                    var bidding = biddings.FirstOrDefault(x => x.CreateBy == que.Answerer.ToString());
                    //退回提问人在发布问题时扣除的积分
                    if (bidding != null)
                    {
                        if (bidding.Currency > que.Currency)
                        {
                            fclient.Integral += bidding.Currency;
                        }
                        else
                        {
                            fclient.Integral += que.Currency;
                        }
                    }
                }
                else
                {
                    fclient.Integral += que.Currency;
                }
                _context.Client.Update(fclient);
                //积分记录表
                IntegralRecords integrals = new IntegralRecords();
                integrals.ClientId = int.Parse(que.CreateBy);
                integrals.Integral = fclient.Integral;
                integrals.Source = "系统退回发布问题积分";
                integrals.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(integrals);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public int Evaluate(int id, string content, int grade)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.Evaluate = content;
                que.Status = (int)questionStatus.Complete;
                que.Sign = grade;
                var bidding = _context.Bidding.FirstOrDefault(x => x.CreateBy == que.Answerer.ToString() && x.QuestionId == que.Id);
                if (que.Currency > bidding.Currency)
                {
                    var integral = que.Currency - bidding.Currency;
                    //将多扣除的积分退回提问人
                    var tclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(que.CreateBy));
                    tclient.Integral += integral;
                    _context.Client.Update(tclient);
                    //积分记录表
                    IntegralRecords record = new IntegralRecords();
                    record.ClientId = int.Parse(que.CreateBy);
                    record.Integral = integral;
                    record.Source = "系统退回积分";
                    record.CreateTime = DateTime.Now;
                    _context.IntegralRecords.Add(record);
                    _context.SaveChanges();
                }
                //回答人获取积分
                var answerclient = _context.Client.FirstOrDefault(x => x.Id == que.Answerer);
                answerclient.Integral += bidding.Currency;
                _context.Client.Update(answerclient);
                //积分记录表
                IntegralRecords records = new IntegralRecords();
                records.ClientId = que.Answerer;
                records.Integral = bidding.Currency;
                records.Source = "回答问题";
                records.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(records);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 申请客服
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public int ForService(int id, string reason)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.Memo = reason;
                que.Status = (int)questionStatus.ForService;
                return _context.SaveChanges();
            }
            return 0;

        }

        /// <summary>
        /// 根据问题id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Question GetQuestion(int id)
        {
            return _context.Question.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public object GetLs(int number, int status, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData();
            var list = from x in ls
                       select new
                       {
                           x.Id,
                           x.Content,
                           x.Number,
                           x.Answerer,
                           cname = x.Answerer == 0 ? "暂时无答题人" : _context.Client.FirstOrDefault(z => z.Id == x.Answerer).Name,
                           x.Status,
                           x.EndTime,
                           x.Currency,
                           x.Title
                       };
            if (number != 0)
            {
                list = list.Where(x => x.Number == number);
            }
            if (status != -1)
            {
                list = list.Where(x => x.Status == status);
            }
            PageTotal = list.Count();
            list = list.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x => x.Id);
            return list.ToList();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(int id, int clientid)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.Answerer = clientid;
                que.Status = (int)questionStatus.Choose;
                _context.Update(que);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ChangeStatus(int id)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.Status = (int)questionStatus.Answer;
                _context.Update(que);
                return _context.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int Audit(int id, int userid, int qintegral, int aintegral)
        {
            var que = _context.Question.FirstOrDefault(x => x.Id == id);
            var bidding = _context.Bidding.FirstOrDefault(x => x.QuestionId == id && x.CreateBy == que.Answerer.ToString());
            var fclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(que.CreateBy));
            if (bidding != null)
            {
                if (bidding.Currency > que.Currency)
                {
                    fclient.Integral += bidding.Currency;
                }
                else
                {
                    fclient.Integral += que.Currency;
                }
            }
            else
            {
                fclient.Integral += que.Currency;
            }
            _context.Client.Update(fclient);
            _context.SaveChanges();
            //积分记录表
            IntegralRecords integrals = new IntegralRecords();
            integrals.ClientId = int.Parse(que.CreateBy);
            integrals.Integral = fclient.Integral;
            integrals.Source = "系统退回发布问题积分";
            integrals.CreateTime = DateTime.Now;
            _context.IntegralRecords.Add(integrals);
            _context.SaveChanges();
            //系统赠送积分(提问人)
            if (qintegral != 0)
            {
                fclient.Integral += qintegral;
                _context.Client.Update(fclient);
                //积分记录表
                IntegralRecords integral = new IntegralRecords();
                integral.ClientId = int.Parse(que.CreateBy);
                integral.Integral = qintegral;
                integral.Source = "系统赠送";
                integral.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(integral);
                _context.SaveChanges();
            }
            //系统赠送积分(回答人)
            if (aintegral != 0)
            {
                var hclient = _context.Client.FirstOrDefault(x => x.Id == que.Answerer);
                hclient.Integral += aintegral;
                _context.Client.Update(hclient);
                //积分记录表
                IntegralRecords records = new IntegralRecords();
                records.ClientId = que.Answerer;
                records.Integral = aintegral;
                records.Source = "系统赠送";
                records.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(records);
            }
            if (que.Status == (int)questionStatus.ForService)
            {
                //发通知
                Notice notice = new Notice();
                notice.CreateTime = DateTime.Now;
                notice.CreateBy = userid.ToString();
                notice.ReceiveId = que.Answerer;
                notice.ContentsUrl = "您投诉的问题编号：" + que.Number + ",已经处理完成。";
                _context.Notice.Add(notice);
            }
            que.Status = (int)questionStatus.Close;
            que.IsAudit = true;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改未处理订单状态
        /// </summary>
        /// <returns></returns>
        public int ChangeStatus()
        {
            var que = GetList().Where(x => x.Status == (int)questionStatus.Answer);
            foreach (var item in que)
            {
                var answer = _context.Answer.Where(x => x.QuestionId == item.Id).OrderByDescending(x=>x.Id).FirstOrDefault();
                if (answer.CreateTime.AddDays(7) >= DateTime.Now)
                {
                    item.Status = (int)questionStatus.Complete;
                    _context.Question.Update(item);
                    var bidding = _context.Bidding.FirstOrDefault(x => x.CreateBy == item.Answerer.ToString() && x.QuestionId == item.Id);
                    if (item.Currency > bidding.Currency)
                    {
                        var integral = item.Currency - bidding.Currency;
                        //将多扣除的积分退回提问人
                        var tclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(item.CreateBy));
                        tclient.Integral += integral;
                        _context.Client.Update(tclient);
                        //积分记录表
                        IntegralRecords record = new IntegralRecords();
                        record.ClientId = int.Parse(item.CreateBy);
                        record.Integral = integral;
                        record.Source = "系统退回积分";
                        record.CreateTime = DateTime.Now;
                        _context.IntegralRecords.Add(record);
                        _context.SaveChanges();
                    }
                    //回答人获取积分
                    var answerclient = _context.Client.FirstOrDefault(x => x.Id == item.Answerer);
                    answerclient.Integral += bidding.Currency;
                    _context.Client.Update(answerclient);
                    //积分记录表
                    IntegralRecords records = new IntegralRecords();
                    records.ClientId = item.Answerer;
                    records.Integral = bidding.Currency;
                    records.Source = "回答问题";
                    records.CreateTime = DateTime.Now;
                    _context.IntegralRecords.Add(records);

                }
            }
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int DelImg(int id, string img)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                if (!string.IsNullOrEmpty(que.Img))
                {
                    que.Img = que.Img.Replace(img, "");
                    return _context.SaveChanges();
                }
            }
            return 0;

        }
    }
}
