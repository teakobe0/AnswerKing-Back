using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DAL;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 客户问题信息表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/ClientQuestionInfo")]
    public class ClientQuestionInfoController : BaseController
    {
        private IClientQuestionInfoDAL _clientqidal;
        private IClientDAL _clientdal;
        public ClientQuestionInfoController(IClientQuestionInfoDAL clientqidal, IClientDAL clientdal)
        {
            _clientqidal = clientqidal;
            _clientdal = clientdal;
        }
        [HttpGet]
        [Route("GetClientQuestionInfo")]
        public ResultModel GetClientQuestionInfo(int clientid)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            info info = new info();
            string url = AppConfig.Configuration["imgurl"];
            try
            {
                info.cqinfo = _clientqidal.GetById(clientid);
                var client = _clientdal.GetClientById(clientid);
                info.qname = client.Name;
                info.qimage = !string.IsNullOrEmpty(client.Image) ? url + client.Image : client.Image;
                r.Data = info;
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        public class info
        {
            public ClientQuestionInfo cqinfo { get; set; }
            public string qname { get; set; }
            public string qimage { get; set; }
        }
    }
}