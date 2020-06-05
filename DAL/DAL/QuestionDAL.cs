using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
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

        private IQueryable<Question> GetListData()
        {
            return _context.Question.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int Add(Question question)
        {

            question.Status = (int)questionStatus.Save;
            question.CreateTime = DateTime.Now;
            _context.Question.Add(question);
            return _context.SaveChanges();
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
                //删除竞拍表关于该问题的所有竞拍记录
                var biddings = _context.Bidding.Where(x => x.QuestionId == id);
                if (biddings.Count() > 0)
                {
                    foreach (var i in biddings)
                    {
                        i.IsDel = true;
                    }
                }
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
                //评分计入该答案的评价中
                var answer = _context.Answer.FirstOrDefault(x => x.QuestionId == id);
                answer.Sign = grade;
                //提问人支付积分
                var payclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(que.CreateBy));
                payclient.Integral -= que.Currency;
                _context.Client.Update(payclient);
                //积分记录表
                IntegralRecords integral = new IntegralRecords();
                integral.ClientId = int.Parse(que.CreateBy);
                integral.Integral = que.Currency;
                integral.Source = "发布问题";
                integral.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(integral);
                _context.SaveChanges();
                //回答人获取积分
                var answerclient = _context.Client.FirstOrDefault(x => x.Id == que.Answerer);
                answerclient.Integral += que.Currency;
                _context.Client.Update(answerclient);
                //积分记录表
                IntegralRecords records = new IntegralRecords();
                records.ClientId = que.Answerer;
                records.Integral = que.Currency;
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
        /// 提交修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public int Edit(int id)
        {
            if (id != 0)
            {
                var que = _context.Question.FirstOrDefault(x => x.Id == id);
                que.Status = (int)questionStatus.Edit;
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
        /// 根据客户id检索问题
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Question> GetList(int clientid)
        {
            var list = GetListData().Where(x => x.CreateBy == clientid.ToString());
            return list.ToList();

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
                que.Status =(int) questionStatus.Choose;
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
            //系统赠送积分(提问人)
            if (qintegral != 0)
            {
                var fclient = _context.Client.FirstOrDefault(x => x.Id == int.Parse(que.CreateBy));
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
                records.Source = "回答问题";
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
                notice.ContentsUrl = "您投诉的问题编号："+que.Number+",已经处理完成。";
                _context.Notice.Add(notice);
            }
            que.Status = (int)questionStatus.Close;
            que.IsAudit = true;
            return _context.SaveChanges();
        }
    }
}
