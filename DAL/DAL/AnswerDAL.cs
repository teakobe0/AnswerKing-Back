using DAL.IDAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    /// <summary>
    /// 回答表数据访问层
    /// </summary>
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
        public List<Answer> GetLs(int questionid)
        {
            var list = GetListData().ToList();
            if (questionid != 0)
            {
                list = list.Where(x => x.QuestionId==questionid).ToList();
            }
            return list;
        }
       /// <summary>
       /// 根据客户id查询
       /// </summary>
       /// <param name="clientid"></param>
       /// <returns></returns>
        public List<Answer> GetList(int clientid)
        {
            var list = GetListData().OrderByDescending(x => x.Id).ToList();
            if (clientid != 0)
            {
                list = list.Where(x => x.CreateBy == clientid.ToString()).ToList();
            }
            return list;

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
                var answer = _context.Answer.FirstOrDefault(x => x.Id == id);
                if (!string.IsNullOrEmpty(answer.Img))
                {
                    answer.Img = answer.Img.Replace(img, "");
                    return _context.SaveChanges();
                }
            }
            return 0;

        }
    }
}
