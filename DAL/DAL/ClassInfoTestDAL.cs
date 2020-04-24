﻿using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DAL.Tools.EnumAll;

namespace DAL.DAL
{
    public class ClassInfoTestDAL : BaseDAL, IClassInfoTestDAL
    {
        public ClassInfoTestDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<ClassInfoTest> GetList()
        {
            return GetListData().ToList();

        }
        public ClassInfoTest GetRandomClassInfo()
        {
            return _context.ClassInfoTest.Where(x => 1 == 1).OrderBy(x => Guid.NewGuid()).First();
        }
        public ClassInfoTest GetRandom()
        {
            var ClassInfo = _context.ClassInfoTest.Where(x => x.ClientId == 0);
            if (ClassInfo.Count() > 0)
            {
                return _context.ClassInfoTest.Where(x => x.ClientId == 0).OrderBy(x => Guid.NewGuid()).First();
            }
            return null;
        }
        /// <summary>
        /// 根据课程id检索
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public List<ClassInfoTest> GetLs(int classid)
        {
            if (classid != 0)
            {
                var list = GetListData();
                list = list.Where(x => x.ClassTestId == classid);
                return list.ToList();
            }
            return null;
        }
        private IQueryable<ClassInfoTest> GetListData()
        {
            return _context.ClassInfoTest.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 新增订单(题库集）
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        public ClassInfoTest Add(ClassInfoTest cit)
        {
            cit.CreateTime = DateTime.Now;
            _context.ClassInfoTest.Add(cit);
            _context.SaveChanges();
            return cit;
        }
        /// <summary>
        /// 修改课程资料
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        public int ChangeClassInfo(ClassInfoTest classInfoTest)
        {
            _context.ClassInfoTest.Update(classInfoTest);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据题库集id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <returns></returns>
        public ClassInfoTest GetClassInfoTest(int id)
        {
            return _context.ClassInfoTest.FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        public int Edit(ClassInfoTest cit)
        {
            _context.ClassInfoTest.Update(cit);
            return _context.SaveChanges();
        }

        /// <summary>
        /// 更改题库集状态
        /// </summary>
        /// <param name="cit"></param>
        /// <returns></returns>
        public int Change(int id)
        {
            var cit = _context.ClassInfoTest.FirstOrDefault(x => x.Id == id);
            cit.Status = (int)classInfoTestStatus.NoAudit;
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
                ClassInfoTest cit = _context.ClassInfoTest.FirstOrDefault(x => x.Id == id);
                cit.IsDel = true;
                _context.ClassInfoTest.Update(cit);
                return _context.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 查询该客户是否创建过该课程的订单
        /// </summary>
        /// <param name="clientid"></param>
        /// <param name="classtestid"></param>
        /// <returns></returns>
        public ClassInfoTest GetClassInfoTest(int clientid, int classtestid)
        {
            return _context.ClassInfoTest.FirstOrDefault(x => x.IsDel == false && x.ClientId == clientid && x.ClassTestId == classtestid);
        }
        /// <summary>
        /// 根据客户id检索课程资料
        /// </summary>
        /// <returns></returns>
        public List<ClassInfoTest> GetList(int clientId)
        {
            var list = GetListData().ToList();
            if (clientId != 0)
            {
                list = list.Where(x => x.ClientId == clientId).ToList();
            }
            return list;
        }
        /// <summary>
        /// 根据课程资料单号查询
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public List<ClassInfoTest> GetListByno(int no, int status)
        {
            var list = GetListData();
            if (no != 0)
            {
                list = list.Where(x => x.Id == no);

            }
            if (status != -1)
            {
                list = list.Where(x => x.Status == status);
            }
            return list.OrderBy(x => x.Id).ToList();
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="classInfoTest"></param>
        /// <returns></returns>
        public int Audit(ClassInfoTest classInfoTest)
        {
            var cit = _context.ClassInfoTest.FirstOrDefault(x => x.Id == classInfoTest.Id);
            if (cit.Status != (int)classInfoTestStatus.Edit)
            {
                //添加积分
                var client = _context.Client.FirstOrDefault(x => x.Id == classInfoTest.ClientId);
                client.Integral += Integral.addcontent;
                //积分记录表
                IntegralRecords ir = new IntegralRecords();
                ir.ClientId = client.Id;
                ir.Integral = Integral.addcontent;
                ir.Source = "贡献资源";
                ir.CreateTime = DateTime.Now;
                _context.IntegralRecords.Add(ir);
            }
            cit.Status = (int)classInfoTestStatus.Audited;
            return _context.SaveChanges();
        }
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        public int GetImportMaxid()
        {
            var c = _context.ClassInfoTest.OrderByDescending(x => x.RefId).FirstOrDefault();
            return c == null ? 0 : c.RefId;
        }
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public int AddImportData(List<ClassInfoTest> ls)
        {
            int num = 0;
            foreach (var item in ls)
            {
                _context.ClassInfoTest.Add(item);
                num += _context.SaveChanges();
            }
            return num;
        }
        /// <summary>
        /// 修改课程资料有用、没用
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="classInfoId"></param>
        /// <param name="type"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public int Change(int clientId, int classInfoId, string type, int check, DateTime? time)
        {
            var ci = _context.ClassInfoTest.FirstOrDefault(x => x.Id == classInfoId);
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
                    urs.CreateTime = time == null ? DateTime.Now : (ur.CreateTime > time ? time.Value.AddDays(1) : ur.CreateTime.AddDays(1));
                    _context.UseRecords.Add(urs);
                    if (ur.Type == "Y")
                    {
                        ci.Use -= 1;
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
       
        public int GetClients()
        {
            return GetListData().Where(x => x.ClientId != 0).GroupBy(x => x.ClientId).Count();

        }
    }
}
