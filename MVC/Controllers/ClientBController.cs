using com.gdce_task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC.Controllers
{
    public class ClientBController : ControllerBase
    {
        //
        // GET: /Task/
        //----------全局申明-----------------------
        T_TaskDAO entityDao = new T_TaskDAO();

        T_Task_HistoryDAO h_entityDao = new T_Task_HistoryDAO();

        T_Task_StatusDAO s_entityDao = new T_Task_StatusDAO();


        String uid;

        //----------发起任务-----------------------
        public static List<T_Task> taskList;
        public ActionResult test(String a, String b, String u, String t)
        {
            if (String.IsNullOrWhiteSpace(a))
            {
                return Fail("error");
            }
            T_TaskDAO T_Taskdao = new T_TaskDAO();
            String uid = Convert.ToString(Session["uid"]);
            T_Task task = new T_Task()
            {
                title = b,
                content = a,
                userId = uid,
                statusId = 2
            };
            int id = T_Taskdao.Add(task);
            //int id = (int)T_Taskdao.GetByModel(task).id;
            T_Task_HistoryDAO histdao = new T_Task_HistoryDAO();
            var time = t;
            T_Task_History history = new T_Task_History()
            {

                content = "发布任务，任务内容为：" + a,
                userName = u,
                taskId = id,
                descr = "发布任务",
                userId = uid,
            };
            histdao.Add(history);
            T_Task_History history2 = new T_Task_History()
            {

                content = "通过任务的审核，等待任务分发",
                userName = u,
                taskId = id,
                descr = "审核任务",
                userId = uid,
            };
            histdao.Add(history2);

            //管理员用户发起任务默认通过已通过审核，同时在添加两条历史记录

            return Success("ok", null);

        }
        //----------通过任务审核-----------------------

        public ActionResult adopt(String tid, String u)
        {
            if (String.IsNullOrWhiteSpace(tid))
            {
                return Fail("error");
            }
            String uid = Convert.ToString(Session["uid"]);
            DateTime time = DateTime.Now;
            T_Task_HistoryDAO adoptdao = new T_Task_HistoryDAO();
            T_Task_History adopt = new T_Task_History()
            {
                taskId = Convert.ToInt32(tid),
                userName = u,
                content = "通过任务的审核，等待任务分发",
                descr = "审核任务",
                userId = uid,
            };
            int id = adoptdao.Add(adopt);

            T_TaskDAO Taskupdatedao = new T_TaskDAO();
            T_Task task = Taskupdatedao.GetById(Convert.ToInt32(tid));
            task.statusId = 2;
            Taskupdatedao.Update(task);

            //int Update Taskupdatedao = entityDao.Update(new T_Task(){id = Convert.ToInt32(tid)});




            return Success("ok", null);

        }


        //-----------通过任务验收，任务打上结束标记-----------------------
        public ActionResult finish(String tid, String u)
        {
            if (String.IsNullOrWhiteSpace(tid))
            {
                return Fail("error");
            }
            String uid = Convert.ToString(Session["uid"]);
            DateTime time = DateTime.Now;
            T_Task_HistoryDAO finishdao = new T_Task_HistoryDAO();
            T_Task_History finish = new T_Task_History()
            {
                taskId = Convert.ToInt32(tid),
                userName = u,
                content = "通过此任务的验收，任务完成",
                descr = "验收任务",
                userId = uid,
            };
            int id = finishdao.Add(finish);

            T_TaskDAO Taskupdatedao = new T_TaskDAO();

            T_Task task = Taskupdatedao.GetById(Convert.ToInt32(tid));
            task.endTime = DateTime.Now;

            task.statusId = 5;
            Taskupdatedao.Update(task);

            return Success("ok", null);

        }
        //----------发起任务视图-----------------------
        public ActionResult ClientBAdd()
        {
            String uid = Convert.ToString(Session["uid"]);
            if (String.IsNullOrWhiteSpace(uid))
            {
                return Fail("系统出错，请重试");
            }
            else
            {

                return View();
            }
        }
        //----------发起任务视图-----------------------
        public ActionResult ClientBList()
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
            T_Task entity = new T_Task();

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

            Session["roleid"] = q.roleId;

            ViewBag.status = sdao.GetAllByModel(null);


            ViewBag.total = total;

            ViewBag.pageSize = this.pageSize;

            ViewBag.status = sdao.GetAllByModel(null);



            ViewBag.list = entitys;

            return View(entitys);

        }
        //----------审核任务视图-----------------------
        public ActionResult ClientBCheck()
        {
            int total = 0;
            List<T_Task> list = entityDao.Search(new T_Task() { statusId=1}, 1, 10, "", T_Task.Columns.statusId, AscDesc.DESC, out total);


            ViewBag.total = total;

            ViewBag.list = list;

            return View();
        }
        //----------验收任务视图-----------------------
        public ActionResult ClientBAccept()
        {
            int total = 0;
            List<T_Task> list = entityDao.Search(new T_Task() { statusId = 4 }, 1, 10, "", T_Task.Columns.statusId, AscDesc.DESC, out total);

            ViewBag.total = total;


            //ViewBag.taskId = T_Task.Columns.id;

            ViewBag.list = list;

            return View();
        }
        //----------任务列表视图-----------------------
        public ActionResult List()
        {
            uid = Request["uid"];
            if (String.IsNullOrWhiteSpace(Request["uid"]))
            {
                return Fail("没有足够的参数");
            }
            else
            {
                T_Task_HistoryDAO historydao = new T_Task_HistoryDAO();
                T_Task_History history = new T_Task_History() { descr = uid };
                List<T_Task_History> HistoryList = historydao.GetAllByModel(history);

                T_TaskDAO T_taskdao = new T_TaskDAO();
                taskList = new List<T_Task>();
                T_Task_StatusDAO statusdao = new T_Task_StatusDAO();
                foreach (T_Task_History item in HistoryList)
                {
                    T_Task task = new T_Task() { id = item.taskId };
                    T_Task t = T_taskdao.GetByModel(task);

                    T_Task_Status status = new T_Task_Status() { id = t.statusId };
                    T_Task_Status s = statusdao.GetByModel(status);

                    item.taskIdEntity.content = s.descr;

                }

                ViewBag.uid = uid;
                ViewBag.list = HistoryList;
                return View();
            }
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
