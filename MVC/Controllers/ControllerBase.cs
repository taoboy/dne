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
using System.Data;
using System.Web.Script.Serialization;

using com.gdce_task.Model;

namespace MVC.Controllers
{
    public class ControllerBase : Controller
    {
        //用于判断是否在子类Controller使用统一过滤器的开关，可以在子类Controller的构造函数里面开关
        protected bool isAjaxRequest = false;
        protected bool isCheckLogin = true;
        protected bool isCatchException = true;
        
        protected int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
 
        /// <summary>
        /// 统一的错误处理方法
        /// </summary>
        /// <param name="ex"></param>
        protected JsonResult Error(Exception ex)
        {
            String msg = "错误来源：" + ex.Source
                + "<br/>错误信息："
                + ex.Message.Replace("\r", "").Replace("\n", "").Replace("\"", "\\\"")
                + "<br/>错误跟踪："
                + ex.StackTrace.Replace("\r", "").Replace("\n", "")
                ;

            JsonModel json = new JsonModel(false, msg, null);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Success(String msg, DataTable dt)
        {
            if (dt == null)
                return Json(new JsonModel(true, msg, null), JsonRequestBehavior.AllowGet);

            List<Dictionary<String, Object>> list = new List<Dictionary<string, Object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<String, Object> dict = new Dictionary<string, Object>();

                foreach (DataColumn dc in dt.Columns)
                    dict[dc.ColumnName] = dr[dc.ColumnName];

                list.Add(dict);
            }

            JsonModel json = new JsonModel(true, msg, list);
            
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Success(String msg, Object data)
        {
            JsonModel json = new JsonModel(true, msg, data);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Fail(String msg)
        {
            JsonModel json = new JsonModel(false, msg, null);

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 所有Controller都应该继承该基类，
        /// 在所有Action执行之前所做的预处理，
        /// 比如鉴权、i18n等
        /// </summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //UsersBLL uBll = new UsersBLL();

            //Users u = new Users();

            ////从cookie抽取数据以自动登录
            //if (Session["User"] == null && Request.Cookies["uname"] != null && Request.Cookies["upass"] != null)
            //{
            //    u.userName = Request.Cookies["uname"].Value;
            //    u.password = BLLHelper.GetMD5(Request.Cookies["upass"].Value);

            //    List<Users> us = uBll.GetAllByModel(u);

            //    if (us.Count > 0)
            //    {
            //        u = us[0];
            //        Session["User"] = u;
            //        String privious = Request.QueryString["refer"];                    
            //    }
            //}


            if (Session["uid"] == null) {
                filterContext.Result = RedirectToRoute(new { Controller = "Login", Action = "Login", refer = Request.Url.AbsolutePath });
            }

            base.OnActionExecuting(filterContext);
        }

        //统一的异常处理方法
        protected override void OnException(ExceptionContext filterContext)
        {
            if (this.isAjaxRequest)
            {
                //Ajax请求允许跨域
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            if (isCatchException)
            {
                //默认拦截所有异常
                base.OnException(filterContext);

                filterContext.ExceptionHandled = true;//这一句非常重要
                Response.Clear();
                Response.Write(new JavaScriptSerializer().Serialize(Error(filterContext.Exception).Data));
            }
        }

        //protected ActionResult checkRole() {

        //    String id = Session["roleid"].ToString();
        //    switch (id)
        //    {
        //        case "1":
        //            return Redirect  ("http://www.baidu.com");          //("~/ClientA/ClientAAdd");
        //            break;
        //        case "2":
        //            return Redirect ("http://www.baidu.com");         //("~/ClientB/ClientBAdd");
        //            break;
        //        case "3":
        //            return Redirect  ("http://www.baidu.com");          //("~/WorkerA/List");
        //            break;
        //        case "4":
        //            return Redirect  ("http://www.baidu.com");          //("~/WorkerB/Distribute");
        //            break;
        //        default:
        //            return Redirect("~/Login/Login");
        //    }

        //}


    }
}