using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IOrderDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<Order> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        string Add(Order order);
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        int ChangeStatus(string orderNo,DateTime paytime );
        /// <summary>
        /// 根据订单号查询
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Order GetOrder(string orderNo);
      
    }
}
