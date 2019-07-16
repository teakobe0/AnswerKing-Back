using DAL.DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IFocusDAL

    {
        /// <summary>
        /// 根据客户id检索关注
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        List<Focus> GetListByClientid(int clientid);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        int Add(Focus focus);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        int Del(int clientid,string  typeid);
    }
}
