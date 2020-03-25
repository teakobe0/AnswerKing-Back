using DAL.IDAL;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    public class BiddingDAL : BaseDAL, IBiddingDAL
    {
        public BiddingDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Bidding> GetList()
        {
            return GetListData().ToList();

        }

        private IQueryable<Bidding> GetListData()
        {
            return _context.Bidding.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        public int Add(Bidding bidding)
        {
            bidding.CreateTime = DateTime.Now;
            _context.Bidding.Add(bidding);
            //变更问题状态为正在竞拍
            var que = _context.Question.FirstOrDefault(x => x.Id == bidding.QuestionId);
            que.Status = (int)questionStatus.Bidding;
            _context.Update(que);
            return _context.SaveChanges();
           
        }
        /// <summary>
        /// 根据问题和客户id检索
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Bidding GetBidding(int questionId, int clientId)
        {
            
            var bidding= _context.Bidding.FirstOrDefault(x => x.QuestionId == questionId && x.CreateBy == clientId.ToString());
            if (bidding != null)
            {
                //修改问题表里面answerer
                var que = _context.Question.FirstOrDefault(x => x.Id == questionId);
                que.Answerer =int.Parse(bidding.CreateBy);
                _context.SaveChanges();
               
            }
            return bidding;
        }
        /// <summary>
        /// 根据问题id
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        public List<Bidding> GetList(int questionid)
        {
            if (questionid != 0)
            {
                var list = GetList().Where(x => x.QuestionId == questionid);
                return list.ToList();
            }
            return null;
        }
    }
}
