using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    public class BaseDAL
    {
        protected DataContext _context;

        //public const int InitID = 1000000000;
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
        /// <summary>
        /// 更新客户问题信息表
        /// </summary>
        /// <param name="cqinfo"></param>
        /// <returns></returns>
        public int CommonUpdateInfo(ClientQuestionInfo cqinfo)
        {
            var qinfo = _context.ClientQuestionInfo.FirstOrDefault(x => x.ClientId == cqinfo.ClientId);
            if (qinfo == null)
            {
                ClientQuestionInfo info = new ClientQuestionInfo();
                info.ClientId = cqinfo.ClientId;
                info.BiddingQuestions = cqinfo.BiddingQuestions;
                info.CreateTime = DateTime.Now;
                _context.ClientQuestionInfo.Add(info);
            }
            else
            {
                qinfo.CompletedQuestions += cqinfo.CompletedQuestions;
                qinfo.BiddingQuestions += cqinfo.BiddingQuestions;
                qinfo.GoodReviews += cqinfo.GoodReviews;
                qinfo.BadReviews += cqinfo.BadReviews;
                qinfo.Upvote += cqinfo.Upvote;
                qinfo.Downvote += cqinfo.Downvote;
                qinfo.Favourites += cqinfo.Favourites;
                qinfo.Views += cqinfo.Views;
                if (qinfo.GoodReviews != 0)
                {
                    //计算好评率
                    qinfo.GoodReviewRate = Math.Round((decimal)qinfo.GoodReviews / (qinfo.GoodReviews + qinfo.BadReviews), 2);
                    //最近好评率(最近10个) 
                    var que = _context.Question.Where(x => x.Answerer == qinfo.ClientId&&x.Status==(int) questionStatus.Complete).OrderByDescending(x=>x.Id).Take(10);
                    int good = 0;
                    int total = 0;
                    foreach (var item in que)
                    {
                        if (item.Sign == 5)
                        {
                            good += 1;
                        }
                        total += 1;
                    }
                    qinfo.RecentGoodReviewRate = Math.Round((decimal)good / total, 2);
                }
                qinfo.UpdateTime = DateTime.Now;
                _context.ClientQuestionInfo.Update(qinfo);
            }
            return _context.SaveChanges();
        }
    }
}
