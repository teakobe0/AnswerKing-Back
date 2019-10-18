using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassCombineDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<ClassCombine> GetList(int targetId);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         int Del(int id);
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Cancel(int id);
    }
}
