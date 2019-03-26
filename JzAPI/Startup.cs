using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DAL;
using Microsoft.EntityFrameworkCore;
using DAL.Ioc;

namespace JzAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加ef的依赖
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));
            //ioc依赖
            BLLDIRegister sdr = new BLLDIRegister();
            sdr.DIRegister_DAL(services);

            ////添加授权信息
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //});

            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = "yourdomain.com",//Audience
                        ValidIssuer = "yourdomain.com",//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
                    };

                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new MyTokenValidator());
                    options.Events = new JwtBearerEvents
                    {
                        //重写OnMessageReceived
                        OnMessageReceived = context => {
                            var token = context.Request.Headers["mytoken"];
                            context.Token = token.FirstOrDefault();
                            return Task.CompletedTask;
                        }
                    };
                });
            //添加Claim授权
            services.AddAuthorization(options => {
                options.AddPolicy("admin", policy => { policy.RequireClaim("admin"); });
            });
            services.AddAuthorization(options => {
                options.AddPolicy("vip", policy => { policy.RequireClaim("vip"); });
            });
            services.AddAuthorization(options => {
                options.AddPolicy("guester", policy => { policy.RequireClaim("guester"); });
            });

            services.AddMvc();
            //添加跨域
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                   .AllowCredentials()
                   .Build());

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCors("CorsPolicy");
        }
    }
}
