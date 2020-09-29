using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.IDAL;
using DAL.Model;
using DAL.Model.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JzAPI.Controllers
{
    /// <summary>
    /// 关注表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Focus")]
    public class FocusController : BaseController
    {
        private IFocusDAL _focusdal;
      
        public FocusController(IFocusDAL focusdal)
        {
            _focusdal = focusdal;
        }
        /// <summary>
        /// 根据客户id检索关注
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Focus")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Focus()
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _focusdal.GetListByClientid(ID,false);

            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 新增关注
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Focus focus)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                focus.ClientId = ID;
                r.Data = _focusdal.Add(focus);
                if ((int)r.Data == 0)
                {
                    r.Status = RmStatus.Error;
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
        /// <summary>
        /// 删除关注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Del")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Del(int id)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                r.Data = _focusdal.Del(id);
                if ((int)r.Data == 0)
                {
                    r.Status = RmStatus.Error;
                    r.Msg = "删除失败。";
                }
            }
            catch (Exception ex)
            {
                r.Status = RmStatus.Error;
            }
            return r;
        }
    }
}