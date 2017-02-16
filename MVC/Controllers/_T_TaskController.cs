/**  
* file: _T_TaskCon.aspx.cs
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
    public partial class _T_TaskController : ControllerBase
    {
        T_TaskDAO entityDao = new T_TaskDAO();
             
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

            //Dictionary<T_TaskColumns, AscDesc> orderBy = new Dictionary<T_TaskColumns, AscDesc>();
            //List<T_Task> entitys = entityDao.WebSearch(page, this.pageSize, search_criteria, orderBy, out total);

            T_Task entity = new T_Task();	
            List<T_Task> entitys = 
			entityDao.Search(entity, page, this.pageSize,search_criteria,T_Task.Columns.id, AscDesc.ASC, out total);

            ViewBag.total = total;
            ViewBag.pageSize = this.pageSize;

            return View("List", entitys);
        }


        public ActionResult Show()
        {
            T_Task entity = new T_Task();
            try
            {
                if(!String.IsNullOrEmpty(Request.QueryString["id"]))
                    entity = entityDao.GetById(Convert.ToInt32(Request.QueryString["id"]));
            }
            catch (Exception ex) { }

            return View("Show", entity);
        }
        
        public ActionResult Edit()
        {
            T_Task entity = new T_Task();

            Int32 id = Convert.ToInt32("-1");
            if (!String.IsNullOrWhiteSpace(Request.QueryString["id"]))
                id = Convert.ToInt32(Request.QueryString["id"]);

            entity = entityDao.GetById(id);

            return View("Edit", entity);
        }

		[ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult Update(T_Task entity) {
            if (entityDao.Update(entity) > 0)
                return Success("更新成功!", null);
            else
                return Fail("更新失败!");
        }

		[ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult Add(T_Task entity) {
            if (entityDao.Add(entity) > 0)
                return Success("新增成功!", null);
            else
                return Fail("新增失败!");
        }
        
        public JsonResult Del(Int32 id)
        {
            if (entityDao.Del(id) > 0)
                return Success("删除成功!", null);
            else
                return Fail("删除失败!");
        }

    }
}
