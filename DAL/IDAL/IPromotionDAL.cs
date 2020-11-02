using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IPromotionDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Add(Promotion promotion);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Promotion GetPromotion(int id);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        int Edit(Promotion promotion);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据课程名称检索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Promotion> GetList(string name);
    }
}
