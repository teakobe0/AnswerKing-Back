using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassWeekDAL
    {
        /// <summary>
        /// 查询列表 根据课程资料id检索
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        List<ClassWeek> GetList(int classinfoid);
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        List<ClassWeek> GetList();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classWeek"></param>
        /// <returns></returns>
        int Add(ClassWeek classWeek);
        /// <summary>
        /// 修改每周课程
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="classWeek"></param>
        /// <returns></returns>
        int ChangeInfo(int ID, ClassWeek classWeek);
        /// <summary>
        /// 修改每周课程
        /// </summary>
        /// <param name="classWeek"></param>
        /// <returns></returns>
        int ChangeInfo(ClassWeek classWeek);
        /// <summary>
        /// 根据每周课程id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassWeek GetClassWeek(int id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 根据课程资料单号检索
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        List<ClassWeek> GetListByNo(int no);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 获取全部导入数据
        /// </summary>
        /// <returns></returns>
        List<ClassWeek> GetImportList();
        /// <summary>
        /// 根据课程资料id
        /// </summary>
        /// <param name="classinfoid"></param>
        /// <returns></returns>
        List<ClassWeek> GetListByClassinfoid(int classinfoid);
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<ClassWeek> ls);



    }
}
