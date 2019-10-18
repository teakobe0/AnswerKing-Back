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
        List<Focus> GetListByClientid(int clientid,bool all);
        /// <summary>
        /// 根据客户id,类型id(课程id，课程资料id)检索关注
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        Focus GetFocus(int clientid, string typeid);
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
    }
}
