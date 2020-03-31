using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.IDAL;
using DAL.Model;
using System.Collections;
using DAL.Tools;
using Microsoft.AspNetCore.Authorization;
using DAL.Model.Const;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using JzAPI.tool;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/File")]
    public class FileController : BaseController
    {

        private IHostingEnvironment _environment;
        public FileController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// 上传答案图片
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadImg")]
        [Authorize(Roles = C_Role.all)]
        public async Task<IActionResult> UploadImg(IFormCollection collection, int classInfoTestId)
        {
            
            if (classInfoTestId==0)
            {
                return BadRequest("题库集单号不能为空");
            }
            else
            {
                var files = collection.Files;
                long size = files.Sum(f => f.Length);
                var filePath = "";
                filePath = CheckDirectory(classInfoTestId);
                string file = "";
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string suffix = formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
                        var number = Guid.NewGuid().ToString();
                        string filename = number + suffix;
                        string pathImg = Path.Combine(filePath, filename);
                        try
                        {
                            using (var stream = new FileStream(pathImg, FileMode.CreateNew))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            file = "/ClassinfoImg/" + classInfoTestId + "/" + filename;
                        }
                        catch (IOException e)
                        {
                            return BadRequest("该文件已存在！请重命名后重新上传");
                        }
                    }
                }
                return Ok(new { count = files.Count, size, file });
            }
        }

        private string CheckDirectory(int classinfoid)
        {
            string url = AppConfig.Configuration["uploadurl"];
            var filePath = Path.Combine(url, "ClassinfoImg");
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, classinfoid.ToString());
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            return filePath;
        }
    }
    
}