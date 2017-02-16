/**  
* file: _UnknownCon.aspx.cs
* Type: MVC Controller Class 
* Genreated date: 2016/8/1 11:22:16
* Host: kspc
* 
*┌───────────────────────────┐
*│　Copyright (c) 2015 Ks_Studio. All rights reserved.	 │
*└───────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using com.gdce_task.Model;
using System.IO;


namespace MVC.Controllers
{
    public class UploadController : ControllerBase
    {
        //
        // GET: /Func/

        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult UploadImg()
        {
            String result = "";
            Response.ContentType = "text/html";
            try
            {
                // 下面这句是最重要的，取得HttpPostedFile对象后就可以调用他的SaveAs方法了  
                HttpPostedFileBase file = Request.Files[0];

                //检查文件大小，理论上到不了这里，在前面就被IIS的配置截住了
                if ((1.0 * file.ContentLength / 1024 / 1024) > 10)
                {
                    return Fail("上传文件不能超过10MB");
                }

                //Users user = (Users)Session["User"];
                //user = (new UsersBLL()).GetById(1);
                String year = DateTime.Now.Year.ToString();//获取年份作为目录，每人每年一个目录
                String prefix = DateTime.Now.ToString("MMdd");//月份和日期作为前缀
                String name = (prefix + "_" + Guid.NewGuid().ToString()).Replace("-", "");//唯一id作为文件名

                string[] arr = file.FileName.Split('.');
                string ext = arr[arr.Length - 1];//获取原扩展名

                //String path = "/Upload/" + year + "/" + user.userName + "/";
                //string savePath = Server.MapPath("~" + path + name + "." + ext);

                //Directory.CreateDirectory(Server.MapPath("~" + path));

                //file.SaveAs(savePath);

                //Attachment att = new Attachment();
                //AttachmentBLL attBll = new AttachmentBLL();
                //att.name = file.FileName;
                //att.url = path + name + "." + ext;
                //att.filetype = ext;
                //att.uid = user.id;
                //att.size = file.ContentLength;

                //int id = attBll.Add(att);

                //result = "{\"success\":true,\"message\":\"上传成功！\",\"fileid\":" + id + ",\"filename\":\""
                //    + file.FileName + "\",\"url\":\"" + attBll.GetById(id).url + "\"}";

                return Success("ok",null);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


    }
}