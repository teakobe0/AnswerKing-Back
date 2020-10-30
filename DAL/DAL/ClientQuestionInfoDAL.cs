using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class ClientQuestionInfoDAL : BaseDAL, IClientQuestionInfoDAL
    {
        public ClientQuestionInfoDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        public int Add(ClientQuestionInfo cqi)
        {
            cqi.CreateTime = DateTime.Now;
            _context.ClientQuestionInfo.Add(cqi);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 根据客户id查询(接口),增加浏览次数
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ClientQuestionInfo GetClientQuestionInfoJK(int clientId)
        {
            var cqinfo = _context.ClientQuestionInfo.FirstOrDefault(x => x.ClientId == clientId);
            cqinfo.Views += 1;
            _context.ClientQuestionInfo.Update(cqinfo);
            _context.SaveChanges();
            return cqinfo;
        }
        /// <summary>
        /// 根据客户id查询
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ClientQuestionInfo GetClientQuestionInfo(int clientId)
        {
            var cqinfo = _context.ClientQuestionInfo.FirstOrDefault(x => x.ClientId == clientId);
            return cqinfo;
        }

        public List<ClientQuestionInfo> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<ClientQuestionInfo> GetListData()
        {
            return _context.ClientQuestionInfo.Where(x => x.IsDel == false);
        }
    }
}
