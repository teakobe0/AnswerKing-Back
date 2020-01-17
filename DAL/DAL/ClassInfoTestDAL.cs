﻿using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// 根据课程资料id检索
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
    }
}