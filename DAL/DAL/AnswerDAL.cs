using DAL.IDAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    public class AnswerDAL:BaseDAL, IAnswerDAL
    {
        public AnswerDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Answer> GetList()
        {
            return GetListData().ToList();

        }

        private IQueryable<Answer> GetListData()
        {
            return _context.Answer.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public int Add(Answer answer)
        {
            answer.CreateTime = DateTime.Now;
            _context.Answer.Add(answer);
            //变更问题状态为已回答
            var question = _context.Question.FirstOrDefault(x => x.Id ==answer.QuestionId);
            question.Status =(int)questionStatus.Answer ;
            _context.Update(question);
            return _context.SaveChanges();
             
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public int Edit(Answer answer)
        {
            _context.Answer.Update(answer);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据答案id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Answer GetAnswer(int id)
        {
            return _context.Answer.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 根据问题id查询
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        public Answer Answer(int questionid)
        {
            return _context.Answer.FirstOrDefault(x => x.QuestionId==questionid);
        }
       /// <summary>
       /// 根据客户id查询
       /// </summary>
       /// <param name="clientid"></param>
       /// <returns></returns>
        public List<Answer> GetList(int clientid)
        {
            var list = GetListData().Where(x => x.CreateBy == clientid.ToString());
            return list.ToList();

        }
    }
}
