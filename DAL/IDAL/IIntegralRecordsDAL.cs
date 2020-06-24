using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IIntegralRecordsDAL
    {
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<IntegralRecords> GetList(int clientid);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<IntegralRecords> GetList();
        /// <summary>
        /// 赠送积分
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        int Give(int clientId);
    }
}
