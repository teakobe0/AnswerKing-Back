using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class OrderDAL:BaseDAL, IOrderDAL
    {
        public OrderDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Order> GetList()
        {
            return GetListData().ToList();
        }
        /// <summary>
        /// 根据客户id检索订单
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Order> GetListByClientid(int clientid)
        {
            if (clientid != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClientId == clientid).OrderByDescending(x=>x.Id);
                return list.ToList();
            }
            return null;

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string  Add(Order order)
        {
            order.CreateTime = DateTime.Now;
            order.Status = 0;
            Random r = new Random();
            int i = r.Next(1000, 9999);
            order.OrderNo = DateTimeToUnixTimestamp(order.CreateTime).ToString()+i;
            _context.Order.Add(order);
            _context.SaveChanges();
            return order.OrderNo;
        }
        public int AddPaypal(Order order)
        {
            _context.Order.Add(order);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public int ChangeStatus(string orderNo,DateTime paytime)
        {
            var order = _context.Order.FirstOrDefault(x => x.OrderNo == orderNo);
            order.Status = 1;
            order.PayTime =paytime;
            return _context.SaveChanges();
        }
      
        /// <summary>
        /// 根据订单号查询
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public Order GetOrder(string orderNo)
        {
            return _context.Order.FirstOrDefault(x => x.OrderNo == orderNo);
        }

        private IQueryable<Order> GetListData()
        {
            return _context.Order.Where(x => 1 == 1);
        }
    }
}
