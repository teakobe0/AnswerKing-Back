using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
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
        public List<UniversityCombine> GetList(int targetId)
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
                UniversityCombine uc = _context.UniversityCombine.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("UniversityCombine:Delete" + uc.ToJson());
                uc.IsDel = true;
                _context.UniversityCombine.Update(uc);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Cancel(int id)
        {
            if (id != 0)
            {
                var uc = _context.UniversityCombine.FirstOrDefault(x => x.Id == id);
                //恢复学校
                var university = _context.University.FirstOrDefault(x => x.Id == uc.OriginalId);
                university.IsDel = false;
                //恢复课程里面的学校id
                var clas = _context.Class.Where(x => x.OriginUniversityId == uc.OriginalId && x.UniversityId == uc.TargetId);
                foreach(var item in clas)
                {
                    item.UniversityId = uc.OriginalId;
                    item.University = university.Name;
                    item.OriginUniversityId = 0;
                }
                //恢复答案
                var cic = _context.ClassInfoContent.Where(x => x.OriginUniversityId == uc.OriginalId && x.UniversityId == uc.TargetId);
                foreach(var i in cic)
                {
                    i.UniversityId = uc.OriginalId;
                    i.OriginUniversityId = 0;
                }
                uc.IsDel = true;
                return _context.SaveChanges();

            }
            return 0;
        }
        private IQueryable<UniversityCombine> GetListData()
        {
            return _context.UniversityCombine.Where(x => x.IsDel == false);
        }
    }
}
