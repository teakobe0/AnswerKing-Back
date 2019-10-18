using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassDAL : BaseDAL, IClassDAL
    {

        public ClassDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetList(string name)
        {
            var list = GetListData().OrderBy(x=>x.Name.Replace("\"","").Trim()).ToList();
            
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.ToLower().Contains(name.Trim().ToLower())).ToList();
            }
            return list;

        }
        public List<Class> GetLs(string name)
        {
            var list = GetListData();

            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Contains(name));
            }
            return list.ToList();

        }
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Add(Class cla)
        {
            cla.CreateTime = DateTime.Now;
            cla.UniversityId = _context.University.FirstOrDefault(x => x.Name == cla.University).Id;
            _context.Class.Add(cla);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 查询课程名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetName(string name, string university, string professor)
        {
            return _context.Class.Any(x => x.Name == name && x.University == university && x.Professor == professor); ;
        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="alif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetList(int universityid, string alif, string name)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityId == universityid);

            }
            if (!string.IsNullOrEmpty(alif))
            {
                list = list.Where(x => x.Name.Substring(0, 1).ToUpper() == alif);

            }
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.Name.Contains(name));
            }
            return list.ToList();

        }
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Class> GetList(int universityid)
        {
            var list = GetListData();
            if (universityid != 0)
            {
                list = list.Where(x => x.UniversityId == universityid);
                return list.ToList();
            }
            return null;
        }
        /// <summary>
        /// 根据id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Class GetClass(int id)
        {
            return _context.Class.FirstOrDefault(x => x.Id == id);
        }

        public Class GetRandomClass()
        {
            return _context.Class.Where(x => 1 == 1).OrderBy(x => Guid.NewGuid()).First();
        }
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int ChangeClass(int ID, Class cla)
        {
            var data = _context.Class.FirstOrDefault(x => x.Id == ID);
            data.Name = cla.Name;
            data.University = cla.University;
            data.UniversityId = _context.University.FirstOrDefault(x => x.Name == cla.University).Id;
            data.Difficulty = cla.Difficulty;
            data.Professor = cla.Professor;
            data.Memo = cla.Memo;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int ChangeClass(Class cla)
        {
            cla.UniversityId = _context.University.FirstOrDefault(x => x.Name == cla.University).Id;
            _context.Class.Update(cla);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        public int Update(Class cla)
        {
            _context.Class.Update(cla);
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
                Class cla = _context.Class.FirstOrDefault(x => x.Id == id);
                Utils.WriteInfoLog("Class:Delete" + cla.ToJson());
                cla.IsDel = true;
                _context.Class.Update(cla);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 根据学校id查询
        /// </summary>
        /// <returns></returns> 
        public List<Class> GetClasses(int universityid)
        {
            var list = GetListData().Where(x => x.UniversityId == universityid);
            return list.ToList();
        }
        /// <summary>
        /// 查询所有学校名称
        /// </summary>
        /// <returns></returns>
        public List<Class> GetUnversitys(int classreid)
        {
            var list = _context.Class.Where(x => x.RefId > classreid).DistinctBy(x => x.University);
            return list.ToList();
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<Class> GetList()
        {
            return GetListData().ToList();

        }
        private IQueryable<Class> GetListData()
        {
            return _context.Class.Where(x => 1 == 1);
        }
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var c = _context.Class.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<Class> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.Class.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 查询全部导入的数据
        /// </summary>
        /// <returns></returns>
        public List<Class> GetImportList()
        {
            var list = GetListData().Where(x => x.RefId != 0);
            return list.ToList();
        }
        /// <summary>
        /// 合并课程
        /// </summary>
        /// <param name="cbrows"></param>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public int Combine(List<Class> cbrows, int targetid,int LoginId)
        {
            foreach (var item in cbrows)
            {
                if (item.Id != targetid)
                {
                    //_context.Class.Remove(item);
                    ClassCombine combine = new ClassCombine();
                    combine.OriginalId = item.Id;
                    combine.TargetId = targetid;
                    combine.CreateTime = DateTime.Now;
                    combine.CreateBy = LoginId.ToString();
                    _context.ClassCombine.Add(combine);
                    Utils.WriteInfoLog("Class:CombineDel" + item.ToJson() + ",targetid:" + targetid);
                    item.IsDel = true;
                    _context.Class.Update(item);
                    var classinfo = _context.ClassInfo.Where(x => x.ClassId == item.Id);
                    foreach (var ci in classinfo)
                    {
                        ci.ClassId = targetid;
                        ci.OriginClassId = item.Id;
                    }
                    var classinfocontent = _context.ClassInfoContent.Where(x => x.ClassId == item.Id);
                    foreach (var co in classinfocontent)
                    {
                        co.ClassId = targetid;
                        co.OriginClassId = item.Id;
                    }
                }
            }
            return _context.SaveChanges();
        }
    }
}
