using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Model
{
    /// <summary>
    /// 客户问题信息表
    /// </summary>
    public class ClientQuestionInfo:BaseModel
    {
        public int ClientId { get; set; } //客户id
        public int CompletedQuestions { get; set; }//完成的回答
        public int BiddingQuestions { get; set; } //参与的回答 
        public int GoodReviews { get; set; } //好评数
        public int BadReviews { get; set; } //差评数
        public decimal GoodReviewRate { get; set; }//好评率
        public decimal RecentGoodReviewRate { get; set; } //最近好评率(最近10个) 
        public int Upvote { get; set; } //赞
        public int Downvote { get; set; } //踩
        public int Favourites { get; set; } //被收藏次数
        public int Views { get; set; } //浏览次数
        public DateTime UpdateTime { get; set; }//最近更新时间
        public bool IsDel { get; set; }//是否删除 0：未删除 1：已删除
    }
}
