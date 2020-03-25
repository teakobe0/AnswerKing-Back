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
    }
}
