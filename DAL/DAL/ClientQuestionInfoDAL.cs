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
        /// 根据客户id检索
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ClientQuestionInfo GetById( int clientId)
        {

            var cqinfo = _context.ClientQuestionInfo.FirstOrDefault(x =>x.ClientId == clientId && x.IsDel == false);
            cqinfo.Views += 1;
            _context.SaveChanges();
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
