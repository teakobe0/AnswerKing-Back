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
    }
}
