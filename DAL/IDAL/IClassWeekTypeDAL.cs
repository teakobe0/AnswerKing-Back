using DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.IDAL
{
    public interface IClassWeekTypeDAL
    {
        /// <summary>
        /// 根据课程资料单号检索
        /// </summary>
        /// <returns></returns>
        object GetList(int classinfoid, int pagenum, int pagesize, out int PageTotal);
        List<ClassWeekType> GetList();
        /// <summary>
        /// 根据每周课程类型id检索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ClassWeekType GetClassWeekType(int id);
        /// <summary>
        /// 根据每周课程id检索课程类型
        /// </summary>
        /// <param name="classWeekId"></param>
        /// <returns></returns>
        List<ClassWeekType> GetClassWeek(int classWeekId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        int Add(ClassWeekType classWeekType);
        /// <summary>
        /// 修改每周课程类型
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="classWeekType"></param>
        /// <returns></returns>
        int ChangeInfo(ClassWeekType classWeekType);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
        /// <summary>
        /// 获取当前导入数据的最大
        /// </summary>
        /// <returns></returns>
        int GetImportMaxid();
        /// <summary>
        /// 获取全部导入数据
        /// </summary>
        /// <returns></returns>
        List<ClassWeekType> GetImportList();
        /// <summary>
        /// 根据每周课程id检索课程类型(前台)
        /// </summary>
        /// <param name="classWeekId"></param>
        /// <returns></returns>
        List<ClassWeekType> ClassWeekTypes(int classWeekId);
        /// <summary>
        /// 导入数据ls
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        int AddImportData(List<ClassWeekType> ls);
    }
}
