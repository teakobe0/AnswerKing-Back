
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IBiddingDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<Bidding> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bidding"></param>
        /// <returns></returns>
        int Add(Bidding bidding);
        /// <summary>
        /// 根据问题和客户id检索
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Bidding GetBidding(int questionId, int clientId);
        /// <summary>
        /// 根据问题id检索
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        List<Bidding> GetList(int questionid);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<Bidding> GetListByCid(int clientid);
        /// <summary>
        /// 根据问题id删除该问题的竞拍记录
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
         int DelLs(int questionid);
    }
}

        
