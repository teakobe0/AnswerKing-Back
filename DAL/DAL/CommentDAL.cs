﻿using DAL.IDAL;
using DAL.Model;
using DAL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.DAL
{
    public class CommentDAL : BaseDAL, ICommentDAL
    {
        public CommentDAL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表全部数据
        /// </summary>
        /// <returns></returns>
        public List<Comment> GetList()
        {
            return GetListData().ToList();
        }

        private IQueryable<Comment> GetListData()
        {
            return _context.Comment.Where(x => 1 == 1);
        }
        /// <summary>
        /// 查询列表全部数据 根据条件
        /// </summary>
        /// <returns></returns>
        public List<Comment_v> GetList(int classInfoid)
        {
            List<Comment_v> ls = new List<Comment_v>();
            Comment_v cv = null;

            var list = _context.Comment.OrderBy(x => x.CreateTime).ToList();

            if (classInfoid != 0)
            {
                list = list.Where(x => x.ClassInfoId == classInfoid).ToList();
                var parent = list.Where(x => x.ParentId == "0");
                foreach (var item in parent)
                {

                    cv = new Comment_v();
                    cv = Utils.TransReflection<Comment, Comment_v>(item);
                    cv.name = _context.Client.FirstOrDefault(x => x.Id == item.ClientId).Name;
                    ls.Add(cv);
                    var son = list.Where(x => x.ParentId == item.Id.ToString());
                    foreach (var ea in son)
                    {
                        cv = new Comment_v();
                        cv = Utils.TransReflection<Comment, Comment_v>(ea);
                        cv.replyname = _context.Client.FirstOrDefault(x => x.Id == ea.ClientId).Name;
                        cv.name = _context.Client.FirstOrDefault(x => x.Id == item.ClientId).Name;
                        ls.Add(cv);
                        var sonreply = list.Where(x => x.ParentId.Contains("," + ea.Id + ",") ||
                         (x.ParentId.Contains("," + ea.Id) && x.ParentId.Substring(x.ParentId.LastIndexOf(",") + 1) == ea.Id.ToString()));
                        foreach (var cc in sonreply)
                        {
                            cv = new Comment_v();
                            cv = Utils.TransReflection<Comment, Comment_v>(cc);
                            cv.replyname = _context.Client.FirstOrDefault(x => x.Id == cc.ClientId).Name;
                            int cid = _context.Comment.FirstOrDefault(x => x.Id == int.Parse(cc.ParentId.Substring(cc.ParentId.LastIndexOf(",") + 1))).ClientId;
                            cv.name = _context.Client.FirstOrDefault(x => x.Id == cid).Name;
                            ls.Add(cv);
                        }
                    }
                }
            }
            return ls;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int Add(Comment comment)
        {

            comment.CreateTime = DateTime.Now;
            _context.Comment.Add(comment);
            return _context.SaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            var co = _context.Comment.FirstOrDefault(x => x.Id == id);
            var dellist = _context.Comment.Where(x=>
            x.ParentId==co.Id.ToString()
            || (x.ParentId.Contains(co.Id + ",")&&x.ParentId.IndexOf(co.Id.ToString())==0)
            || x.ParentId.Contains(","+co.Id+",")
            ||( x.ParentId.Contains("," + co.Id)&&x.ParentId.Substring(x.ParentId.LastIndexOf(",") + 1)==co.Id.ToString()));
            _context.Comment.RemoveRange(dellist);
            _context.Comment.Remove(co);
            return _context.SaveChanges();
        }

    }

    public class Comment_v : Comment
    {
        public string replyname { get; set; }//回复人昵称
        public string name { get; set; }//被回复昵称
        public string contenturl { get; set; } //回复内容 地址
    }
}
