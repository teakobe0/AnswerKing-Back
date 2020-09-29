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
    /// 课程合并表数据访问层
    /// </summary>
    public class ClassCombineDAL:BaseDAL, IClassCombineDAL
    {
        public ClassCombineDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<ClassCombine> GetList(int targetId)
        {
            var list= GetListData();
            if (targetId != 0)
            {
                list = list.Where(x => x.TargetId == targetId);
            }
            return list.ToList();
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
                ClassCombine cc = _context.ClassCombine.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("ClassCombine:Delete" + cc.ToJson());
                cc.IsDel = true;
                _context.ClassCombine.Update(cc);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public int Cancel(int id)
        //{
        //    if (id != 0)
        //    {
        //        var cc = _context.ClassCombine.FirstOrDefault(x => x.Id == id);
        //        //恢复课程
        //        var cla = _context.Class.FirstOrDefault(x => x.Id == cc.OriginalId);
        //        cla.IsDel = false;
        //        //恢复课程资料的classid
        //        var ci = _context.ClassInfo.Where(x => x.OriginClassId == cc.OriginalId && x.ClassId == cc.TargetId);
        //        foreach(var i in ci)
        //        {
        //            i.ClassId = cc.OriginalId;
        //            i.OriginClassId = 0;
        //        }
        //        //恢复答案里面classid
        //        var cic = _context.ClassInfoContent.Where(x => x.OriginClassId == cc.OriginalId && x.ClassId == cc.TargetId);
        //        foreach(var t in cic)
        //        {
        //            t.ClassId = cc.OriginalId;
        //            t.OriginClassId = 0;
        //        }
        //        cc.IsDel = true;
        //        return _context.SaveChanges();
        //    }
        //    return 0;
        //}

        private IQueryable<ClassCombine> GetListData()
        {
            return _context.ClassCombine.Where(x => x.IsDel == false);
        }
    }
}
