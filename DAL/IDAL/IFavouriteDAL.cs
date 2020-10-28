using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IFavouriteDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        int Add(Favourite favourite);
        /// <summary>
        /// 根据问题id查询该问题收藏数量
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        int GetNum(int questionid);
    }
}
