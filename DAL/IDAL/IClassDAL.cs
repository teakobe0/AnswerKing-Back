using DAL.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace DAL.IDAL
{
    public interface IClassDAL
    {
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetList(string name);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="alif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetList(int universityid, string alif, string name);
        List<Class> GetList(int universityid);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<Class> GetList();
        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Add(Class cla);
        /// <summary>
        /// 查询课程名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(string name, string university, string professor);
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        ///根据id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Class GetClass(int id);
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cla"></param>
        /// <returns></returns>
        int ChangeClass(int ID, Class cla);
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int ChangeClass(Class cla);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<Class> ls);
        int Combine(List<Class> cbrows, int targetid);
        /// <summary>
        /// 查询全部导入的数据
        /// </summary>
        /// <returns></returns>
        List<Class> GetImportList();
        /// <summary>
        /// 查询所有学校名称
        /// </summary>
        /// <returns></returns>
        List<Class> GetUnversitys(int classreid);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
         int Update(Class cla);
        /// <summary>
        /// 根据学校id查询
        /// </summary>
        /// <returns></returns> 
        List<Class> GetClasses(int universityid);
    }
}
