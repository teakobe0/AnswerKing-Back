using DAL.IDAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class FavouriteDAL : BaseDAL, IFavouriteDAL
    {
        public FavouriteDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="favourite"></param>
        /// <returns></returns>
        public int Add(Favourite favourite)
        {
            int num = 0;
            favourite.CreateTime = DateTime.Now;
            _context.Favourite.Add(favourite);
            num = _context.SaveChanges();
            var que = _context.Question.FirstOrDefault(x => x.Id == favourite.QuestionId);
            //更新ClientQuestionInfo
            if (que.Answerer != 0)
            {
                ClientQuestionInfo info = new ClientQuestionInfo();
                info.ClientId = que.Answerer;
                info.Favourites = 1;
                CommonUpdateInfo(info);
            }
            return num;
        }

        public List<Favourite> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Favourite> GetListData()
        {
            return _context.Favourite.Where(x => x.IsDel == false);
        }
        /// <summary>
        /// 根据问题id查询该问题收藏数量
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
        public int GetNum(int questionid)
        {
            if (questionid != 0)
            {
                return _context.Favourite.Where(x => x.QuestionId == questionid).Count(); 
            }
            return 0;
        }
    }
}
