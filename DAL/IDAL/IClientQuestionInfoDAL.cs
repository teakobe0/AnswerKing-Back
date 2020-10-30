using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClientQuestionInfoDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        int Add(ClientQuestionInfo cqi);
        /// <summary>
        /// 根据客户id查询(接口),增加浏览次数
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientQuestionInfo GetClientQuestionInfoJK(int clientId);
        /// <summary>
        /// 根据客户id查询
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientQuestionInfo GetClientQuestionInfo(int clientId);
    }
}
