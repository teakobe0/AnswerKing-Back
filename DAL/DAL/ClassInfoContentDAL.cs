using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassInfoContentDAL : BaseDAL, IClassInfoContentDAL
    {

        public ClassInfoContentDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<ClassInfoContent> GetList()
        {
            return GetListData().ToList();
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public List<ClassInfoContent> GetList(string searchText)
        {
            var list = GetListData().Where(x => x.IsAudit ==1);
            if (!string.IsNullOrEmpty(searchText))
            {
                list = list.Where(x => x.Contents.Contains(searchText));
            }
            return list.ToList();
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        public List<ClassInfoContent> GetList(int classweekid)
        {
            var list = GetListData();
            if (classweekid != 0)
            {
                list = list.Where(x => x.ClassWeekId == classweekid);
                return list.ToList();
            }
            return null;

        }

        /// <summary>
        /// 根据每周课程类型id检索
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        public List<ClassInfoContent> GetByTypeid(int classweektypeid, int id)
        {
            var list = GetListData();
            if (classweektypeid != 0)
            {
                var cic = _context.ClassWeekType.FirstOrDefault(x => x.Id == classweektypeid).ClassWeekTypeId;
                if (cic == 0)
                {
                    var refid = _context.ClassWeekType.FirstOrDefault(x => x.Id == classweektypeid).RefId;
                    list = list.Where(x => x.CwtParentId == refid);
                }
                else
                {
                    if (id != 0)
                    {
                        list = list.Where(x => x.Id == id);
                    }
                    else
                    {
                        list = list.Where(x => x.ClassWeekTypeId == classweektypeid);
                    }
                }
                return list.ToList();
            }
            return null;

        }

        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="classweekid"></param>
        /// <returns></returns>
        public object GetListbycinid(int classinfoid, int status, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData();
            if (classinfoid != 0)
            {
                ls = ls.Where(x => x.ClassInfoId == classinfoid);
            }
            if (status != -1)
            {
                ls = ls.Where(x => x.IsAudit == status);
            }
            var list = from x in ls
                       select new
                       {
                           x.ClassInfoId,
                           x.Contents,
                           ClassWeek = _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId) != null ? _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId).No.ToString() : "",
                           x.Id,
                           x.ClassId,
                           ClassWeekType = _context.ClassWeekType.FirstOrDefault(z => z.Id == x.ClassWeekTypeId) != null ? _context.ClassWeekType.FirstOrDefault(z => z.Id == x.ClassWeekTypeId).ContentType : "",
                           x.ClassWeekId,
                           x.ClassWeekTypeId,
                           x.CwtParentId,
                           x.IsAudit

                       };
            list = list.Where(x => x.CwtParentId != 0 && x.ClassInfoId != -99);
            PageTotal = list.Count();
            list = list.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x => x.Id);
            return list.ToList();
        }
        private IQueryable<ClassInfoContent> GetListData()
        {
            return _context.ClassInfoContent.Where(x => x.Status==0);
        }
        /// <summary>
        /// 根据答案id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassInfoContent GetClassInfoContent(int id)
        {
            return _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);

        }
        /// <summary>
        /// 检索下一个未审核答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassInfoContent GetNext(int id)
        {
            return _context.ClassInfoContent.FirstOrDefault(x => x.Id > id && x.IsAudit == 0 && x.CwtParentId != 0);
        }

        /// <summary>
        /// 新增答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        public int Add(ClassInfoContent classInfoContent)
        {
            classInfoContent.ClassId = _context.ClassInfo.FirstOrDefault(x => x.Id == classInfoContent.ClassInfoId).ClassId;
            classInfoContent.UniversityId = _context.Class.FirstOrDefault(x => x.Id == classInfoContent.ClassId).UniversityId;
            classInfoContent.CwtParentId = _context.ClassWeekType.FirstOrDefault(x => x.Id == classInfoContent.ClassWeekTypeId).ClassWeekTypeId;
            classInfoContent.IsAudit = 0;
            classInfoContent.CreateTime = DateTime.Now;
            _context.ClassInfoContent.Add(classInfoContent);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 编辑答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        public int ChangeInfo(int Id, ClassInfoContent classInfoContent)
        {
            var content = _context.ClassInfoContent.FirstOrDefault(x => x.Id == Id);
            content.Contents = classInfoContent.Contents;
            return _context.SaveChanges();

        }
        /// <summary>
        /// 编辑答案
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        public int ChangeInfo(ClassInfoContent classInfoContent)
        {
            _context.ClassInfoContent.Update(classInfoContent);
            return _context.SaveChanges();

        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="classInfoContent"></param>
        /// <returns></returns>
        public int Audit(ClassInfoContent classInfoContent)
        {
            var cic = _context.ClassInfoContent.FirstOrDefault(x => x.Id == classInfoContent.Id);
            if (cic.IsAudit == 1)
            {
                return 0;
            }
            else
            {
                cic.IsAudit = 1;
                cic.Url = classInfoContent.Url;
                cic.Contents = classInfoContent.Contents;
                Utils.WriteInfoLog("ClassInfoContent:Audit" + cic.ToJson());
                return _context.SaveChanges();
            }
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
                var cic = _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);
                cic.Status = -1;
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var cic = _context.ClassInfoContent.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return cic == null ? 0 : cic.RefId;

        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="cic"></param>
        /// <returns></returns>
        public int AddImportData(ClassInfoContent cic)
        {
            _context.ClassInfoContent.AddRange(cic);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public int DelImg(int id, string img)
        {
            var cic = _context.ClassInfoContent.FirstOrDefault(x => x.Id == id);
            if (cic.Url != null && cic.Url != "")
            {

                cic.Url = cic.Url.Replace(img, "");
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }

        }
    }
}
