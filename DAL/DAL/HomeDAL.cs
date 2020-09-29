using DAL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    /// <summary>
    /// 后台首页数据访问层
    /// </summary>
    public class HomeDAL : BaseDAL,IHomeDAL
    {
        public HomeDAL(DataContext context)
        {
            _context = context;
        }
        public class seris {
            public string name;
            public int value;
        }

        public object GetAllModuleCount()
        {
            List<seris> ls = new List<seris>();
            ls.Add(new seris { name = "学校", value = _context.University.Count() });

            ls.Add(new seris { name = "课程", value = _context.Class.Count() });
            ls.Add(new seris { name = "课程资料", value = _context.ClassInfo.Count() });
            ls.Add(new seris { name = "答案", value = _context.ClassInfoContent.Count() });
            ls.Add(new seris { name = "管理员", value = _context.User.Count() });
            ls.Add(new seris { name = "客户", value = _context.Client.Count() });
            return ls;
        }

    }
}
