using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    /// <summary>
    /// 推广表数据访问层
    /// </summary>
    public class PromotionDAL : BaseDAL, IPromotionDAL
    {
        public PromotionDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Promotion> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Promotion> GetListData()
        {
            return _context.Promotion.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Add(Promotion promotion)
        {
            promotion.CreateTime = DateTime.Now;
            _context.Promotion.Add(promotion);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        public int Edit(Promotion promotion)
        {
            _context.Promotion.Update(promotion);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Promotion GetPromotion(int id)
        {
            return _context.Promotion.FirstOrDefault(x => x.Id == id);

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            if (id != 0)
            {
                var pro = _context.Promotion.FirstOrDefault(x => x.Id == id);
                pro.IsDel = true;
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Promotion> GetList(string name)
        {

            var ls = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                ls = ls.Where(x => x.Name.Trim().Contains(name.Trim()));
            }
            return ls.ToList();
        }
    }
}

