using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<Class> GetList();
        /// <summary>s
        /// 新增课程
        /// </summary>
        /// <param name="clat"></param>
        /// <returns></returns>
        Class Add(Class clat);
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetList(int universityId, string name);
        /// <summary>
        /// 根据课程id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Class GetClass(int id);
        /// <summary>
        /// 根据客户id检索课程
        /// </summary>
        /// <returns></returns>
        List<Class> GetList(int clientId);
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Edit(Class cla);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 查询同一学校的该课程名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool GetName(int universityId, string name, int id);
        /// <summary>
        /// 根据课程名称检索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetListbyname(string name);
        /// <summary>
        /// 获取当前导入数据的最大id
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<Class> ls);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetList(string name);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetLs(int universityid);
        /// <summary>
        /// 查询列表 根据条件
        /// </summary>
        /// <param name="universityid"></param>
        /// <param name="alif"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Class> GetList(int universityid, string alif, string name);
        Class GetRandomClass();
    }
}
