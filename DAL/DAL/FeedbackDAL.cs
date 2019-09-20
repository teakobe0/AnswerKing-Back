using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class FeedbackDAL : BaseDAL, IFeedbackDAL
    {
        public FeedbackDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public int Add(Feedback feedback)
        {

            feedback.CreateTime = DateTime.Now;
            _context.Feedback.Add(feedback);
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
                var feedback = _context.Feedback.FirstOrDefault(x => x.Id == id);
                feedback.IsDel = true;
                _context.Feedback.Update(feedback);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<Feedback> GetList(string name)
        {
            var ls = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                ls = ls.Where(x => x.Name.Contains(name));
            }
            return ls.ToList();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<Feedback> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Feedback> GetListData()
        {
            return _context.Feedback.Where(x => 1 == 1);
        }
    }
}