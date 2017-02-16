using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.gdce_task.Model;

namespace MVC.Controllers
{
    public class WorkerAController : ControllerBase
    {
        // GET: /WorkerA/


        //----------任务列表视图-----------------------


        public static List<T_Task> taskList;
        public ActionResult List()
        {
            String uid = Convert.ToString(Session["uid"]);
            if (String.IsNullOrWhiteSpace(uid))
            {
                return Fail("没有足够的参数");
            }
            else
            {
                //翻页，页数查询条件
                int page = 1;
                if (!String.IsNullOrEmpty(Request["p"]))
                {
                    page = Convert.ToInt32(Request["p"]);
                }
                //关键字模糊查询
                String search_criteria = "";
                if (!String.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    search_criteria = Request.QueryString["s"];
                }

                int total;
                T_Task entity = new T_Task();
                //任务状态查询
                if (!String.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    entity.statusId = Int32.Parse(Request.QueryString["status"]);
                }
                T_TaskDAO entityDao = new T_TaskDAO();
                List<T_Task> entitys =
                entityDao.Search(entity, page, this.pageSize, search_criteria, T_Task.Columns.statusId, AscDesc.ASC, out total);
                ViewBag.list = entitys;

                T_UserDAO udao = new T_UserDAO();
                T_User q = udao.GetByModel(new T_User() { id = uid });
                T_Task_StatusDAO sdao = new T_Task_StatusDAO();
                ViewBag.uid = uid;

                Session["roleid"] = q.roleId;
                ViewBag.status = sdao.GetAllByModel(null);
                //ViewBag.list = taskList;
                return View();
            }
        }

    }
}
