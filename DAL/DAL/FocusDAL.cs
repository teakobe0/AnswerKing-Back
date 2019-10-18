using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class FocusDAL : BaseDAL, IFocusDAL
    {
        public FocusDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据客户id检索关注
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Focus> GetListByClientid(int clientid, bool all)
        {
            var list = GetListData();
            if (clientid != 0)
            {
                list = list.Where(x => x.ClientId == clientid);
            }
            if (all == false)
            {
                list = list.Where(x => x.IsDel == false);
            }
            return list.ToList();
        }
        /// <summary>
        /// 根据客户id,类型id(课程id，课程资料id)检索关注
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public Focus GetFocus(int clientid, string typeid)
        {
            var focus = _context.Focus.FirstOrDefault(x => x.IsDel == false && x.ClientId == clientid && x.TypeId == typeid);
            return focus;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        public int Add(Focus focus)
        {
            focus.CreateTime = focus.CreateTime == DateTime.MinValue ? DateTime.Now : focus.CreateTime;
            _context.Focus.Add(focus);
            return _context.SaveChanges();
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
                var focus = _context.Focus.FirstOrDefault(x => x.Id == id);
                focus.IsDel = true;
                focus.CancelTime = DateTime.Now;
                _context.Focus.Update(focus);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        public List<Focus> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Focus> GetListData()
        {
            return _context.Focus.Where(x => 1 == 1);
        }
    }
}