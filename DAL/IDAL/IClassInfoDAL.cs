using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassInfoDAL
    {
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        List<ClassInfo> GetList(int classid);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<ClassInfo> GetList();
        /// <summary>
        /// 根据课程资料id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassInfo GetClassInfo(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        int Add(ClassInfo classInfo);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 修改课程资料(接口)
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        int Change(int ID,int classInfoId, string type, int check);
        /// <summary>
        /// 修改课程资料
        /// </summary>
        /// <param name="classInfo"></param>
        /// <returns></returns>
        int ChangeClassInfo(ClassInfo classInfo);
        /// <summary>
        /// 根据课程资料单号查询
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        List<ClassInfo> GetListByno(int no);
        /// <summary>
        /// 获取导入数据最大的
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        int AddImportData(ClassInfo ci);
        /// <summary>
        /// 查询全部导入的数据
        /// </summary>
        /// <returns></returns>
         List<ClassInfo> GetImportList();

    }
}
