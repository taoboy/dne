using com.gdce_task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC.Controllers
{
    public class ClientAController : ControllerBase
    {
        //
        // GET: /Task/

        T_TaskDAO entityDao = new T_TaskDAO();

        public static List<T_Task> taskList;


        //----------发布任务-----------------------
        public ActionResult test(String a, String b, String u)
        {


            if (String.IsNullOrWhiteSpace(a))
            {
                return Fail("error");
            }


            T_TaskDAO T_Taskdao = new T_TaskDAO();
            String d = Convert.ToString(Session["uid"]); 
            T_Task task = new T_Task()
            {
                title = b,
                content = a,
                statusId = 1,
                userId = d,
            };
            int id = T_Taskdao.Add(task);
            T_Task_HistoryDAO histdao = new T_Task_HistoryDAO();
            var time = System.DateTime.Now.ToString("d");
            T_Task_History history = new T_Task_History()
            {

                content = "发布任务，任务内容为："+ a,
                userName = u,
                taskId = id,
                descr = "发布任务",
                userId = d,

            };
            histdao.Add(history);



            return Success("ok", null);

        }


        


        //----------发起任务视图-----------------------
        public ActionResult ClientAAdd()
        {
            //checkRole();


            return View();
        }

        //----------任务列表视图-----------------------
        public ActionResult ClientAList()
        {
            //---查询翻页,p代表页数
            int page = 1;
            if (!String.IsNullOrEmpty(Request["p"]))
            {
                page = Convert.ToInt32(Request["p"]);
            }
            //--查询关键字
            String search_criteria = "";
            if (!String.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search_criteria = Request.QueryString["s"];
            }

            int total;
            T_Task entity = new T_Task();
            //----查询的任务状态分类
            if (!String.IsNullOrEmpty(Request.QueryString["status"]))
            {
                entity.statusId = Int32.Parse(Request.QueryString["status"]);
            }

            List<T_Task> entitys =
            entityDao.Search(entity, page, this.pageSize, search_criteria, T_Task.Columns.statusId, AscDesc.ASC, out total);

            T_Task_StatusDAO sdao = new T_Task_StatusDAO();

            T_UserDAO udao = new T_UserDAO();

            T_TaskDAO tdao = new T_TaskDAO();


            String uid = Convert.ToString(Session["uid"]);

            T_User q = udao.GetByModel(new T_User() { id = uid });

            Session["username"] = q.name;

            Session["dept"] = q.dept;

            Session["roleid"] = q.roleId;

            ViewBag.status = sdao.GetAllByModel(null);


            ViewBag.total = total;

            ViewBag.pageSize = this.pageSize;

            ViewBag.status = sdao.GetAllByModel(null);



            ViewBag.list = entitys;

            return View(entitys);
            
        }

        [ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult UpdateTask(T_Task entity)
        {
            if (entityDao.Update(entity) > 0)
                return Success("更新成功!", null);
            else
                return Fail("更新失败!");
        }

        [ValidateInput(false)]//要加上这一句，否则会造成富文本里面的HTML代码危险警报
        public JsonResult AddTask(T_Task entity)
        {
            if (entityDao.Add(entity) > 0)
                return Success("新增成功!", null);
            else
                return Fail("新增失败!");
        }

    }
}
