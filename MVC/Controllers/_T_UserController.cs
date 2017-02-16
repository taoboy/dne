/**  
* file: _T_UserCon.aspx.cs
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

namespace MVC.Controllers
{
    public partial class _T_UserController : ControllerBase
    {
        T_UserDAO entityDao = new T_UserDAO();
             
        public ActionResult List()
        {
            int page = 1;
            if (!String.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }
            
            String search_criteria = "";
            if (!String.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search_criteria = Request.QueryString["s"];
            }
            
            int total;

            //Dictionary<T_UserColumns, AscDesc> orderBy = new Dictionary<T_UserColumns, AscDesc>();
            //List<T_User> entitys = entityDao.WebSearch(page, this.pageSize, search_criteria, orderBy, out total);

            T_User entity = new T_User();	
            List<T_User> entitys = 
			entityDao.Search(entity, page, this.pageSize,search_criteria,T_User.Columns.id, AscDesc.ASC, out total);

            ViewBag.total = total;
            ViewBag.pageSize = this.pageSize;

            return View("List", entitys);
        }


        public ActionResult Show()
        {
            T_User entity = new T_User();
            try
            {
                if(!String.IsNullOrEmpty(Request.QueryString["id"]))
                    entity = entityDao.GetById(Convert.ToString(Request.QueryString["id"]));
            }
            catch (Exception ex) { }

            return View("Show", entity);
        }
        
        public ActionResult Edit()
        {
            T_User entity = new T_User();

            String id = Convert.ToString("-1");
            if (!String.IsNullOrWhiteSpace(Request.QueryString["id"]))
                id = Convert.ToString(Request.QueryString["id"]);

            entity = entityDao.GetById(id);

            return View("Edit", entity);
        }

		[ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult Update(T_User entity) {
            if (entityDao.Update(entity) > 0)
                return Success("更新成功!", null);
            else
                return Fail("更新失败!");
        }

		[ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult Add(T_User entity) {
            if (entityDao.Add(entity) > 0)
                return Success("新增成功!", null);
            else
                return Fail("新增失败!");
        }
        
        public JsonResult Del(String id)
        {
            if (entityDao.Del(id) > 0)
                return Success("删除成功!", null);
            else
                return Fail("删除失败!");
        }

    }
}
