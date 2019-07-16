using DAL.DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IFeedbackDAL

    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        int Add(Feedback feedback);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<Feedback> GetList();
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<Feedback> GetList(string name);
    }
}
