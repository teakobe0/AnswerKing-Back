using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassWeekTypeDAL : BaseDAL, IClassWeekTypeDAL
    {
        public ClassWeekTypeDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据每周课程类型id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClassWeekType GetClassWeekType(int id)
        {
            return _context.ClassWeekType.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        public int Add(ClassWeekType classWeekType)
        {
            //查询新增的类型是否存在父级
            var parent = _context.ClassWeekType.FirstOrDefault(x => x.ClassWeekTypeId == 0 && x.ClassWeekId == classWeekType.ClassWeekId && x.ContentType == classWeekType.ContentType);
            if (parent != null)
            {
                classWeekType.ClassWeekTypeId = parent.RefId;
            }
            else
            {
                classWeekType.ClassWeekTypeId = -1;
            }
            classWeekType.CreateTime = DateTime.Now;
            _context.ClassWeekType.Add(classWeekType);
            _context.SaveChanges();
            if (classWeekType.Grade != 0)
            {
                if (classWeekType.ClassWeekTypeId > 0)
                {
                    //更新父级分数
                    var ls = _context.ClassWeekType.Where(x => x.ClassWeekTypeId == classWeekType.ClassWeekTypeId);
                    if (ls.Count() > 0)
                    {
                        int num = ls.Count();
                        int grade = ls.Sum(x => x.Grade);
                        parent.Grade = grade / num;
                        _context.ClassWeekType.Update(parent);
                        _context.SaveChanges();
                    }
                }
                //更新对应周的分数
                var cw = _context.ClassWeek.FirstOrDefault(x => x.Id == classWeekType.ClassWeekId);
                var cwtls = _context.ClassWeekType.Where(x => x.ClassWeekId == cw.Id && (x.ClassWeekTypeId == 0 || x.ClassWeekTypeId == -1)&&x.Grade>0);
                if (cwtls.Count() > 0)
                {
                    int num = cwtls.Count();
                    int grade = cwtls.Sum(x => x.Grade);
                    cw.Grade = grade / num;
                }
            }
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据每周课程id检索课程类型(前台)
        /// </summary>
        /// <param name="classWeekId"></param>
        /// <returns></returns>
        public List<ClassWeekType> ClassWeekTypes(int classWeekId)
        {

            if (classWeekId != 0)
            {
                var list = GetListData().ToList();
                list = list.Where(x => x.ClassWeekId == classWeekId && (x.ClassWeekTypeId == 0 || x.ClassWeekTypeId == -1)).ToList();
                if (list.Count() > 1)
                {
                    var i = 0;
                    list = list.OrderBy(x => { i = Array.IndexOf(new string[] { "Quiz", "Exam", "Assignment", "Discussion" }, x.ContentType); if (i != -1) { return i; } else { return int.MaxValue; } }).ToList();
                }
                return list;
            }
            return null;

        }
        /// <summary>
        /// 根据课程资料单号检索
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        public object GetList(int classinfoid, int pagenum, int pagesize, out int PageTotal)
        {
            PageTotal = 0;
            var ls = GetListData().Where(x => x.ClassWeekTypeId != 0);
            var list = from x in ls
                       join cw in _context.ClassWeek on x.ClassWeekId equals cw.Id
                       join ci in _context.ClassInfo on cw.ClassInfoId equals ci.Id

                       select new
                       {
                           x.ClassWeekTypeId,
                           ClassWeek = _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId) != null ? _context.ClassWeek.FirstOrDefault(z => z.Id == x.ClassWeekId).No.ToString() : "",
                           x.ContentType,
                           x.ClassWeekId,
                           x.Grade,
                           x.Id,
                           ClassInfo = cw.ClassInfoId

                       };
            if (classinfoid != 0)
            {
                list = list.Where(x => x.ClassInfo == classinfoid);
            }
            PageTotal = list.Count();
            list = list.Skip(pagesize * (pagenum - 1)).Take(pagesize).OrderBy(x => x.Id);
            return list.ToList();
        }
        /// <summary>
        /// 根据每周课程id检索课程类型
        /// </summary>
        /// <param name="classWeekId"></param>
        /// <returns></returns>

        public List<ClassWeekType> GetClassWeek(int classWeekId)
        {

            if (classWeekId != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassWeekId == classWeekId && x.ClassWeekTypeId != 0);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 修改每周课程类型
        /// </summary>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        public int ChangeInfo(ClassWeekType classWeekType)
        {
            int num = 0;
            if (classWeekType.ClassWeekTypeId == -1)
            {
                _context.ClassWeekType.Update(classWeekType);
                _context.SaveChanges();
                num = num + 1;
            }
            else
            {
                var model = _context.ClassWeekType.FirstOrDefault(x => x.Id == classWeekType.Id);
                //更新该子集
                model.Grade = classWeekType.Grade;
                _context.ClassWeekType.Update(model);
                _context.SaveChanges();
                num = num + 1;
                if (classWeekType.ClassWeekTypeId > 0)
                {
                    if (classWeekType.ContentType != model.ContentType)
                    {
                        model.ContentType = classWeekType.ContentType;
                        if (classWeekType.RefId != 0)
                        {
                            var cwt = _context.ClassWeekType.FirstOrDefault(x => x.ClassWeekTypeId == 0 && x.ClassWeekId == classWeekType.ClassWeekId && x.ContentType == classWeekType.ContentType);
                            if (cwt != null)
                            {
                                model.ClassWeekTypeId = cwt.RefId;
                            }
                            //更新对应答案里面的父级id
                            if (classWeekType.ClassWeekTypeId != -1)
                            {
                                var cic = _context.ClassInfoContent.FirstOrDefault(x => x.ClassWeekTypeId == classWeekType.Id);
                                if (cic != null)
                                {
                                    cic.CwtParentId = cwt.RefId;
                                    _context.ClassInfoContent.Update(cic);
                                }
                            }
                        }
                        _context.ClassWeekType.Update(model);
                        _context.SaveChanges();
                        num = num + 1;
                        //更新未修改之前的父级得分
                        var bcwt = _context.ClassWeekType.FirstOrDefault(x => x.RefId == classWeekType.ClassWeekTypeId && x.ClassWeekTypeId == 0&&x.ContentType== classWeekType.ContentType);
                        var bls = _context.ClassWeekType.Where(x => x.ClassWeekTypeId == bcwt.RefId && x.Grade > 0 && x.ClassWeekTypeId != 0 && x.ContentType == bcwt.ContentType);
                        if (bls.Count() > 0)
                        {
                            int number = bls.Count();
                            int grade = bls.Sum(x => x.Grade);
                            bcwt.Grade = grade / number;
                        }
                        else
                        {
                            bcwt.Grade = 0;
                        }
                        _context.ClassWeekType.Update(bcwt);
                        _context.SaveChanges();
                        num = num + 1;
                    }
                    //更新类型对应的父级得分
                    var cwtp = _context.ClassWeekType.FirstOrDefault(x => x.RefId == model.ClassWeekTypeId && x.ClassWeekTypeId == 0&&x.ContentType==model.ContentType);
                    var pls = _context.ClassWeekType.Where(x => x.ClassWeekTypeId == cwtp.RefId && x.Grade > 0 && x.ClassWeekTypeId != 0 && x.ContentType == cwtp.ContentType);
                    if (pls.Count() > 0)
                    {
                        int number = pls.Count();
                        int grade = pls.Sum(x => x.Grade);
                        cwtp.Grade = grade / number;
                    }
                    else
                    {
                        cwtp.Grade = 0;
                    }
                    _context.ClassWeekType.Update(cwtp);
                    _context.SaveChanges();
                    num = num + 1;
                }
            }
            //更新对应周的总分
            var cw = _context.ClassWeek.FirstOrDefault(x => x.Id == classWeekType.ClassWeekId);
            var ls = _context.ClassWeekType.Where(x => x.ClassWeekId == cw.Id && (x.ClassWeekTypeId == 0 || x.ClassWeekTypeId == -1) && x.Grade > 0);
            if (ls.Count() > 0)
            {
                int number = ls.Count();
                int sgrade = ls.Sum(x => x.Grade);
                cw.Grade = sgrade / number;
            }
            else
            {
                cw.Grade = 0;
            }
            _context.ClassWeek.Update(cw);
            _context.SaveChanges();
            num = num + 1;
            return num;
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
                var classWeekType = _context.ClassWeekType.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("ClassWeekType:Delete" + classWeekType.ToJson());
                classWeekType.IsDel = true;
                _context.ClassWeekType.Update(classWeekType);
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
            var cwt = _context.ClassWeekType.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return cwt == null ? 0 : cwt.RefId;

        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassWeekType> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassWeekType.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 获取全部导入数据
        /// </summary>
        /// <returns></returns>
        public List<ClassWeekType> GetImportList()
        {
            var list = _context.ClassWeekType.Where(x => x.RefId != 0);
            return list.ToList();
        }
        /// <summary>
        /// 查询列表 全部数据
        /// </summary>
        /// <returns></returns>
        public List<ClassWeekType> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<ClassWeekType> GetListData()
        {
            return _context.ClassWeekType.Where(x => x.IsDel == false);
        }
    }
}
