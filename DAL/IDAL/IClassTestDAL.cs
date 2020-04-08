using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassTestDAL
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns> 
        List<ClassTest> GetList();
        /// <summary>s
        /// 新增课程
        /// </summary>
        /// <param name="clat"></param>
        /// <returns></returns>
        ClassTest Add(ClassTest clat);
        /// <summary>
        /// 根据条件检索
        /// </summary>
        /// <param name="universityTestId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        List<ClassTest> GetList(int universityTestId, string name);
        /// <summary>
        /// 根据课程id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassTest GeClassTest(int id);
        /// <summary>
        /// 根据客户id检索课程
        /// </summary>
        /// <returns></returns>
        List<ClassTest> GetList(int clientId);
        /// <summary>
        /// 编辑课程
        /// </summary>
        /// <param name="cla"></param>
        /// <returns></returns>
        int Edit(ClassTest cla);
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
        bool GetName(int universityTestId, string name, int id);
        /// <summary>
        /// 根据课程名称检索
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="pagenum"></param>
        /// <param name="pagesize"></param>
        /// <param name="PageTotal"></param>
        /// <returns></returns>
        object GetListbyname(string name, int pagenum, int pagesize, out int PageTotal);
        /// <summary>
        /// 查询列表 带条件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object GetData();
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
        int AddImportData(List<ClassTest> ls);
    }
}
