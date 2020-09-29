using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    /// <summary>
    /// 地区表数据访问层
    /// </summary>
    public class AreaDAL : BaseDAL, IAreaDAL
    {
        public AreaDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据国家id检索对应的州/省份
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Area> GetList(string name)
        {
            var list = GetListData();
            if (!string.IsNullOrEmpty(name))
            {
                var countryid = _context.Area.FirstOrDefault(x => x.Country == name).Id;
                list = list.Where(x => x.ParentId == countryid);
            }

            return list.ToList();

        }
        /// <summary>
        /// 查询所有国家名称
        /// </summary>
        /// <returns></returns>
        public List<Area> GetCountryList()
        {
            var list = GetListData();
            list = list.Where(x => x.ParentId == 0);
            return list.ToList();

        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Area> GetList()
        {
            return GetListData().ToList();
        }


        private IQueryable<Area> GetListData()
        {
            return _context.Area.Where(x => 1 == 1);
        }

    }
}
