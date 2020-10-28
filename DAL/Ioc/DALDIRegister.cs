using DAL.DAL;
using DAL.IDAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Ioc
{
    /// <summary>
    /// 依赖注入映射关系配置类
    /// </summary>
    public class DALDIRegister
    {
        public void DIRegister_DAL(IServiceCollection services)
        {
            services.AddTransient(typeof(IClientDAL), typeof(ClientDAL));
            services.AddTransient(typeof(IUserDAL), typeof(UserDAL));
            services.AddTransient(typeof(IHomeDAL), typeof(HomeDAL));
            services.AddTransient(typeof(IAreaDAL), typeof(AreaDAL));
            services.AddTransient(typeof(IImportRecordsDAL), typeof(ImportRecordsDAL));
            services.AddTransient(typeof(ICommentDAL), typeof(CommentDAL));
            services.AddTransient(typeof(INoticeDAL), typeof(NoticeDAL));
            services.AddTransient(typeof(IOrderDAL), typeof(OrderDAL));
            services.AddTransient(typeof(IUseRecordsDAL), typeof(UseRecordsDAL));
            services.AddTransient(typeof(IFocusDAL), typeof(FocusDAL));
            services.AddTransient(typeof(IFeedbackDAL), typeof(FeedbackDAL));
            services.AddTransient(typeof(IUniversityCombineDAL), typeof(UniversityCombineDAL));
            services.AddTransient(typeof(IClassCombineDAL), typeof(ClassCombineDAL));
            services.AddTransient(typeof(IClassDAL), typeof(ClassDAL));
            services.AddTransient(typeof(IUniversityDAL), typeof(UniversityDAL));
            services.AddTransient(typeof(IClassInfoDAL), typeof(ClassInfoDAL));
            services.AddTransient(typeof(IClassInfoContentDAL), typeof(ClassInfoContentDAL));
            services.AddTransient(typeof(IQuestionDAL), typeof(QuestionDAL));
            services.AddTransient(typeof(IAnswerDAL), typeof(AnswerDAL));
            services.AddTransient(typeof(IBiddingDAL), typeof(BiddingDAL));
            services.AddTransient(typeof(IIntegralRecordsDAL), typeof(IntegralRecordsDAL));
            services.AddTransient(typeof(IClientQuestionInfoDAL), typeof(ClientQuestionInfoDAL));
            services.AddTransient(typeof(IFavouriteDAL), typeof(FavouriteDAL));
        }
    }
}
