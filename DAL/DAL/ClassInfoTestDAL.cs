using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
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
    }
}
