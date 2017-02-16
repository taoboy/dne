using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using com.gdce_task.Model;

namespace MVC.Controllers
{
    public class WorkerBController : ControllerBase
    {
        //
        // GET: /WorkerB/

        T_TaskDAO entityDao = new T_TaskDAO();

        T_Task_HistoryDAO h_entityDao = new T_Task_HistoryDAO();

        T_Task_StatusDAO s_entityDao = new T_Task_StatusDAO();
        public ActionResult Check()
        {
            String uid = Convert.ToString(Session["uid"]);
            if (String.IsNullOrWhiteSpace(uid))
            {
                return Fail("没有足够的参数");
            }
            else
            {
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task() {statusId = 2};
                List<T_Task> tasklist = taskdao.GetAllByModel(task);

                T_Task_HistoryDAO historydao = new T_Task_HistoryDAO();
                foreach (T_Task item in tasklist)
                {
                    T_Task_History history = new T_Task_History() {taskId = item.id,content = "发起任务"};
                    T_Task_History hislist = historydao.GetByModel(history);
                    if (hislist == null)
                    {

                    }
                    else
                    {
                        item.statusIdEntity.descr = hislist.userName;
                    }
                }

                ViewBag.list = tasklist;
                ViewBag.uid = uid;
                return View();
            }
        }

        public ActionResult UserCenter()
        {
            String uid = Convert.ToString(Session["uid"]);
            T_UserDAO userdao = new T_UserDAO();
            T_User q = userdao.GetByModel(new T_User() { id = uid });

            Session["phone"] = q.phone;
            return View();
        }

        public JsonResult changePass()
        {
            String uid = Convert.ToString(Session["uid"]);
            String pass = Request["pass"];
            String oldpass = Request["old"];
            T_UserDAO userdao = new T_UserDAO();
            T_User entity = new T_User() { id = uid, password = oldpass };
            entity = userdao.GetByModel(entity);
            if (entity == null)
            {
                return Fail("密码错误，请重新输入");
            }
            else
            {

                T_User q = userdao.GetByModel(new T_User() { id = uid });

                q.password = pass;

                userdao.Update(q);

                pass = "";
                //   var tempEntity = new { roleid=entity.roleId };
                //    string json5 = JsonConvert.SerializeObject(tempEntity);


                return Success("修改密码成功", entity);
                // 
            }
        }



        public ActionResult phone(String p)
        {
            if (String.IsNullOrWhiteSpace(p))
            {
                return Fail("error");
            }

            String uid = Convert.ToString(Session["uid"]);
            T_UserDAO phonedao = new T_UserDAO();
            T_User phone = phonedao.GetById(uid);
            phone.phone = p;

            phonedao.Update(phone);
            int result = phonedao.Update(phone);
            if (result > 0)
            {
                Session["phone"] = p;
                return Success("已通过！", null);
            }
            else
            {
                return Success("已通过，但未记录", null);
            }

        }

        public JsonResult doCheck()
        {
            String uid = Convert.ToString(Session["uid"]);
            var taskid = Convert.ToInt32(Request["id"]);
            var time = Request["time"];
            if (String.IsNullOrWhiteSpace(time))
            {
                return Fail("请填写时间");
            }
            else
            {
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task() { id = taskid };
                task = taskdao.GetByModel(task);
                task.statusId = 3;
                task.endTime = Convert.ToDateTime(time);
                task.beginTime = DateTime.Now;
                int taskresult = taskdao.Update(task);
                if (taskresult > 0)
                {
                    T_UserDAO userdao = new T_UserDAO();
                    T_User user = new T_User() { id = uid };
                    user = userdao.GetByModel(user);

                    T_Task_HistoryDAO historydao = new T_Task_HistoryDAO();
                    T_Task_History history = new T_Task_History() { taskId = taskid, userName = user.name, content = "接受任务", descr = uid };
                    int result = historydao.Add(history);
                    if (result > 0)
                    {
                        return Success("已通过！", null);
                    }
                    else
                    {
                        return Success("已通过，但未记录", null);
                    }
                }
                else
                {
                    return Fail("操作失败，请重试。");
                }
            }
        }
        public ActionResult Distribute()
        {
            String uid = Convert.ToString(Session["uid"]);
            if (String.IsNullOrWhiteSpace(uid))
            {
                return Fail("没有足够的参数");
            }
            else
            {
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task() { statusId = 2 };
                List<T_Task> tasklist = taskdao.GetAllByModel(task);
               
                T_UserDAO userdao = new T_UserDAO();
                T_User user = new T_User() { roleId = 3 };
                List<T_User> userlist = userdao.GetAllByModel(user);

                T_User q = userdao.GetByModel(new T_User() { id = uid });

                Session["username"] = q.name;

                Session["dept"] = q.dept;

                Session["roleid"] = q.roleId;


                ViewBag.tasklist = tasklist;
                ViewBag.userlist = userlist;
                
                ViewBag.uid = uid;
                return View();
            }
        }

        public JsonResult doDistribute()
        {
            String uid = Convert.ToString(Session["uid"]);
            var title = Request["title"];
            var userid = Request["userid"];
            var name = Request["name"];
            var content = Request["content"];
            var starttime = Request["starttime"];
            var endtime = Request["endtime"];

            if (String.IsNullOrWhiteSpace(content) || String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(userid) || String.IsNullOrWhiteSpace(title))
            {
                return Fail("参数不完整，请填写完毕再提交。");
            }
            else
            {
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task(){id = Convert.ToInt32(title)};
                task = taskdao.GetByModel(task);
                task.beginTime = Convert.ToDateTime(starttime);
                task.endTime = Convert.ToDateTime(endtime);
                task.statusId = 3;
                if (taskdao.Update(task) > 0) {
                    T_Task_UserDAO taskuserdao = new T_Task_UserDAO();
                    T_Task_User taskuser = new T_Task_User() { userId = userid, taskId = task.id, descr = content };
                    if (taskuserdao.Add(taskuser) > 0)
                    {
                        T_Task_HistoryDAO historydao = new T_Task_HistoryDAO();
                        T_Task_History history = new T_Task_History() { taskId = task.id, descr = "分配任务" };
                        history = historydao.GetByModel(history);
                        if (history == null)
                        {
                            T_UserDAO userdao = new T_UserDAO();
                            T_User user = new T_User() { id = uid };
                            user = userdao.GetByModel(user);
                            history = new T_Task_History() { taskId = task.id, userName = user.name, descr = "分配任务",content="任务已分配。"};
                            if (historydao.Add(history) > 0)
                            {
                                return Success("已成功分发任务给" + name, null);
                            }
                            else
                            {
                                return Fail("已分配，但未记录到任务历史中。");
                            }
                        }
                        else
                        {
                             return Success("已成功分发任务给" + name, null);
                        }
                    }
                    else
                    {
                        return Fail("任务还没成功分配给" + name);
                    }
                }
                else
                {
                    return Fail("任务还没成功分配给" + name);
                }
            }
        }

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

            Session["username"] = q.name;

            Session["dept"] = q.dept;

            Session["roleid"] = q.roleId;
            

            ViewBag.total = total;

            ViewBag.pageSize = this.pageSize;

            ViewBag.status = sdao.GetAllByModel(null);



            ViewBag.list = entitys;

            return View(entitys);

            if (String.IsNullOrWhiteSpace(uid))
            {
                return Fail("没有足够的参数");
            }
            else
            {
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task() { };
                List<T_Task> tasklist = taskdao.GetAllByModel(task);

                T_Task_StatusDAO statusdao = new T_Task_StatusDAO();
                T_Task_Status status = new T_Task_Status() { };
                List<T_Task_Status> statuslist = statusdao.GetAllByModel(status);

                ViewBag.uid = Session["uid"];
                ViewBag.list = tasklist;
                ViewBag.statuslist = statuslist;
                return View();
            }
        }
    }
}
