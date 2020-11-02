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
    /// 竞拍表数据访问层
    /// </summary>
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
            int num = 0;
            bidding.CreateTime = DateTime.Now;
            _context.Bidding.Add(bidding);
            //变更问题状态为正在竞拍
            var que = _context.Question.FirstOrDefault(x => x.Id == bidding.QuestionId);
            que.Status = (int)questionStatus.Bidding;
            que.BiddingNum += 1;
            _context.Update(que);
            num = _context.SaveChanges();
            //更新ClientQuestionInfo
            ClientQuestionInfo info = new ClientQuestionInfo();
            info.ClientId = int.Parse(bidding.CreateBy);
            info.BiddingQuestions = 1;
            CommonUpdateInfo(info);
            //发通知
            Notice notice = new Notice();
            notice.Type = (int)noticeType.System;
            notice.ContentsUrl = "您参与问题" + que.Id + "竞拍";
            notice.SendId = int.Parse(que.CreateBy);
            notice.ReceiveId = int.Parse(bidding.CreateBy);
            notice.CreateTime = DateTime.Now;
            _context.Notice.Add(notice);
            _context.SaveChanges();
            return num;

        }
        /// <summary>
        /// 根据问题和客户id检索
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Bidding GetBidding(int questionId, int clientId)
        {
            var bidding = _context.Bidding.FirstOrDefault(x => x.QuestionId == questionId && x.CreateBy == clientId.ToString() && x.IsDel == false);

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
        /// <summary>
        /// 根据客户id
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Bidding> GetListByCid(int clientid)
        {
            if (clientid != 0)
            {
                var list = GetList().Where(x => x.CreateBy == clientid.ToString()).OrderByDescending(x => x.Id);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        public int Edit(Bidding bidding)
        {
            _context.Bidding.Update(bidding);
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
                var bid = _context.Bidding.FirstOrDefault(x => x.Id == id);
                bid.IsDel = true;
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据问题id删除该问题的竞拍记录
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        public int DelLs(int questionid)
        {
            if (questionid != 0)
            {
                //删除竞拍表关于该问题的所有竞拍记录
                var biddings = _context.Bidding.Where(x => x.QuestionId == questionid);
                foreach (var i in biddings)
                {
                    i.IsDel = true;
                }
                _context.Bidding.UpdateRange(biddings);
                int bnum = biddings.Count();
                var que = _context.Question.FirstOrDefault(x => x.Id == questionid);
                que.BiddingNum -= bnum;
                if (que.BiddingNum < 0)
                {
                    que.BiddingNum = 0;
                }
                _context.Question.Update(que);
                _context.SaveChanges();
            }
            return 0;
        }


    }
}
