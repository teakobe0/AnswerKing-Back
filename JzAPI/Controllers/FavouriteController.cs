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
    /// 收藏表控制层
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Favourite")]
    public class FavouriteController : BaseController
    {
        private IFavouriteDAL _favouritedal;

        public FavouriteController(IFavouriteDAL favouritedal)
        {
            _favouritedal = favouritedal;
        }
       
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="focus"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = C_Role.all)]
        public ResultModel Add([FromBody] Favourite favourite)
        {
            ResultModel r = new ResultModel();
            r.Status = RmStatus.OK;
            try
            {
                favourite.ClientId = ID;
                r.Data = _favouritedal.Add(favourite);

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
       
    }
}