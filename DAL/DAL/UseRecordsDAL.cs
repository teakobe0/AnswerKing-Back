﻿using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class UseRecordsDAL : BaseDAL, IUseRecordsDAL
    {
        public UseRecordsDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 根据客户id，课程资料id检索课程资料是有用/无用
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="classInfoid"></param>
        /// <returns></returns>
        public UseRecords GetUseRecords(int ID, int classInfoid)
        {
            return _context.UseRecords.Where(x => x.ClassInfoId == classInfoid && x.ClientId == ID).OrderByDescending(x => x.Id).FirstOrDefault();

        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        public List<UseRecords> GetList()
        {
            return GetListData().ToList();

        }

        private IQueryable<UseRecords> GetListData()
        {
            return _context.UseRecords.Where(x => 1 == 1);
        }

    }
}