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
        public List<Focus> GetListByClientid(int clientid)
        {
            var list= GetListData();
            if (clientid != 0)
            {
                list = list.Where(x => x.ClientId == clientid).OrderByDescending(x=>x.Id);
            }
            return list.ToList();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        public int Add(Focus focus)
        {

            focus.CreateTime = DateTime.Now;
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
                _context.Focus.Update(focus);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int clientid,string typeid)
        {
            var ls = GetListData().Where(x => x.ClientId == clientid);
            if (ls.Count()>0&&!string.IsNullOrEmpty(typeid))
            {
                var focus = ls.FirstOrDefault(x => x.TypeId == typeid);
                focus.IsDel = true;
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