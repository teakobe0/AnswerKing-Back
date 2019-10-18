using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClassInfoDAL : BaseDAL, IClassInfoDAL
    {

        public ClassInfoDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据课程id检索
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public List<ClassInfo> GetList(int classid)
        {
            if (classid != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassId == classid);
                return list.ToList();
            }
            return null;
        }

        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <returns></returns>
        public ClassInfo GetClassInfo(int id)
        {
            return _context.ClassInfo.FirstOrDefault(x => x.Id == id);
        }

        public ClassInfo GetRandomClassInfo()
        {
            return _context.ClassInfo.Where(x => 1 == 1).OrderBy(x => Guid.NewGuid()).First();
        }
        public ClassInfo GetRandom()
        {
            return _context.ClassInfo.Where(x => x.ClientId == 0).OrderBy(x => Guid.NewGuid()).First();
        }
        /// <summary>
        /// 根据课程资料单号查询
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public List<ClassInfo> GetListByno(int no)
        {
            var list = GetListData();
            if (no != 0)
            {
                list = list.Where(x => x.Id == no);

            }
            return list.ToList();


        }
        public int GetClients()
        {
            return GetListData().Where(x=>x.ClientId!=0).GroupBy(x=>x.ClientId).Count();

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        public int Add(ClassInfo classInfo)
        {
            //var maxclassinfo = _context.ClassInfo.OrderByDescending(x => x.Id).FirstOrDefault();
             //classInfo.Id = maxclassinfo == null || maxclassinfo.Id < InitID ? InitID : maxclassinfo.Id + 1;
            classInfo.CreateTime = DateTime.Now;
            _context.ClassInfo.Add(classInfo);
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
                ClassInfo classInfo = _context.ClassInfo.FirstOrDefault(x => x.Id == id);
                classInfo.IsDel = true;
                _context.ClassInfo.Update(classInfo);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 修改课程资料有用、没用
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoId"></param>
        /// <param name="type"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public int Change(int clientId, int classInfoId, string type, int check,DateTime? time)
        {
            var ci = _context.ClassInfo.FirstOrDefault(x => x.Id == classInfoId);
            UseRecords urs = null;
            if (check == 1)
            {
                var ur = _context.UseRecords.Where(x => x.ClassInfoId == classInfoId && x.ClientId == clientId).OrderByDescending(x => x.CreateTime).FirstOrDefault();
                if (ur != null && ur.Check == 1)
                {
                    urs = new UseRecords();
                    urs = Utils.TransReflection<UseRecords, UseRecords>(ur);
                    urs.Id = 0;
                    urs.Check = -1;
                    urs.Type = ur.Type;
                    urs.CreateTime =time==null?DateTime.Now:(ur.CreateTime > time ?  time.Value.AddDays(1) : ur.CreateTime.AddDays(1));
                    _context.UseRecords.Add(urs);
                    if (ur.Type == "Y")
                    {
                        ci.Use-= 1;
                    }
                    else
                    {
                        ci.NoUse -= 1;
                        
                    }
                }
            }
            if (type == "Y")
            {
                ci.Use += check;
            }
            else
            {
                ci.NoUse += check;
               

            }
            if (ci.Use == -1)
            {
                ci.Use = 0;
            }
            if (ci.NoUse == -1)
            {
                ci.NoUse = 0;
            }
            urs = new UseRecords();
            urs.ClassInfoId = classInfoId;
            urs.ClientId = clientId;
            urs.Check = check;
            urs.Type = type;
            urs.CreateTime = time != null ? time.Value : DateTime.Now;
            _context.UseRecords.Add(urs);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 修改课程资料
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        public int ChangeClassInfo(ClassInfo classInfo)
        {
            _context.ClassInfo.Update(classInfo);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 更新分数
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        public int UpdateGrade(int id,int grade)
        {
            var ci = _context.ClassInfo.FirstOrDefault(x => x.Id == id);
            ci.TotalGrade = grade;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<ClassInfo> GetList()
        {
            return GetListData().OrderByDescending(x => x.Id).ToList();
        }

        private IQueryable<ClassInfo> GetListData()
        {
            return _context.ClassInfo.Where(x => x.IsDel ==false);
        }
        /// <summary>
        /// 获取导入数据最大的
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var ci = _context.ClassInfo.Where(x => x.RefId != 0).OrderByDescending(x => x.RefId).FirstOrDefault();
            return ci == null ? 0 : ci.RefId;

        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassInfo> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassInfo.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 查询全部导入的数据
        /// </summary>
        /// <returns></returns>
        public List<ClassInfo> GetImportList()
        {
            var list = _context.ClassInfo.Where(x => x.RefId != 0);
            return list.ToList();
        }
    }
}
