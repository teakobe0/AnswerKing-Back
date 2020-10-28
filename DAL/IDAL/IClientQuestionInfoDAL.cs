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
        /// 根据客户id检索
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientQuestionInfo GetById(int clientId);
    }
}
