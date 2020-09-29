using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Ioc
{
    /// <summary>
    /// 配置DAL层的依赖注入
    /// </summary>
    public class BLLDIRegister
    {
        public void DIRegister_DAL(IServiceCollection services)
        {
            //配置一个依赖注入映射关系
            // services.AddTransient(typeof(IExampleBLL), typeof(ExampleBLL));

            //注册DAL层的依赖注入
            DALDIRegister sdr = new DALDIRegister();
            sdr.DIRegister_DAL(services);
        }
    }
}
