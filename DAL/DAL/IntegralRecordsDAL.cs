using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
   public class IntegralRecordsDAL:BaseDAL, IIntegralRecordsDAL
    {
        public IntegralRecordsDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据客户id检索
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<IntegralRecords> GetList(int clientid)
        {
            var list = GetListData().Where(x => x.ClientId == clientid);
            return list.ToList();

        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<IntegralRecords> GetList()
        {
            return GetListData().ToList();

        }


        private IQueryable<IntegralRecords> GetListData()
        {
            return _context.IntegralRecords.Where(x => 1 == 1);
        }
    }
}
