using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
   public class UniversityCombineDAL:BaseDAL, IUniversityCombineDAL
    {
        public UniversityCombineDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<UniversityCombine> GetList()
        {
            return GetListData().ToList();

        }
        private IQueryable<UniversityCombine> GetListData()
        {
            return _context.UniversityCombine.Where(x => 1 == 1);
        }
    }
}
