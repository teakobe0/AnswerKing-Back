using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
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

        /// <summary>
        /// 赠送积分
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public int Give(int clientId)
        {
            //添加积分
            var client = _context.Client.FirstOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                client.Integral += Integral.addcontent;
                //积分记录表
                IntegralRecords ir = new IntegralRecords();
                ir.ClientId = client.Id;
                ir.Integral = Integral.addcontent;
                ir.Source = "贡献资源";
                ir.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(ir);
            }
            return _context.SaveChanges();
        }

        private IQueryable<IntegralRecords> GetListData()
        {
            return _context.IntegralRecords.Where(x => 1 == 1);
        }
    }
}
