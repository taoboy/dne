using com.gdce_task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC.Controllers
{
    public class TaskController : ControllerBase
    {
        //
        // GET: /Task/

        T_TaskDAO entityDao = new T_TaskDAO();

        //----------任务详情视图-----------------------
        public ActionResult Task()
        {
            String taskid = Request["id"];
            String uid = Convert.ToString(Session["uid"]);
            String roleid = Convert.ToString(Session["roleid"]);
            if (String.IsNullOrWhiteSpace(taskid) || String.IsNullOrWhiteSpace(uid) || String.IsNullOrWhiteSpace(roleid))
            {
                return Fail("参数不足，请重试");
            }
            //参数不足会返回错误信息
            else
            {

                T_Task_HistoryDAO historydao = new T_Task_HistoryDAO();
                T_Task_History history = new T_Task_History() { taskId = Convert.ToInt32(taskid) };
                List<T_Task_History> hislist = historydao.GetAllByModel(history);
                ViewBag.list = hislist;
                T_Task_History title = historydao.GetByModel(history);
                ViewBag.uid = uid;
                T_TaskDAO taskdao = new T_TaskDAO();
                T_Task task = new T_Task() { id = Convert.ToInt32(taskid) };
                task = taskdao.GetByModel(task);
                Session["title"] = task.title;
                Session["tid"] = task.id;
                Session["roleid"] = roleid;
                ViewBag.content = task.content;
                ViewBag.createtime = task.createTime;
                ViewBag.begintime = task.beginTime;
                ViewBag.endtime = task.endTime;
                ViewBag.statusid = task.statusId;
                Session["statusid"] = ViewBag.statusid;
                ViewBag.status = task.statusIdEntity.descr;
                //根据任务编号返回跟任务相关的各种信息
                T_Task_UserDAO tuserdao = new T_Task_UserDAO();
                T_Task_User tuser = new T_Task_User() { taskId = Convert.ToInt32(Request["id"]) };
                List<T_Task_User> tuserlist = tuserdao.GetAllByModel(tuser);
                ViewBag.tuser = tuserlist;


                    return View();
            }
        }
        

        //-----提交任务进度-------------
        public JsonResult AddReport()
        {
            String uid = Convert.ToString(Session["uid"]);
            var title = Request["taskTitle"];
            var content = Request["content"];
            var des = Request["descr"];
            var tid = Session["tid"];
            if (!String.IsNullOrWhiteSpace(title) && !String.IsNullOrWhiteSpace(content) && !String.IsNullOrWhiteSpace(des))
            {
                T_UserDAO T_UserDao = new T_UserDAO();
                T_User user = new T_User() { id = uid };
                user = T_UserDao.GetByModel(user); //user.name;
                String u = user.name;
                if (user == null)
                {
                    return Fail("用户验证失败，请重新登录");
                }
                else
                {
                    int i;
                    T_TaskDAO T_TaskDao = new T_TaskDAO();
                    T_Task task = new T_Task() { title = title };
                    task = T_TaskDao.GetByModel(task);// task.id;
                    String descr = "汇报任务";
                    T_Task_HistoryDAO T_Task_HistoryDao = new T_Task_HistoryDAO();
                    if (Convert.ToInt32(des) == 4)
                    {
                        //默认为汇报任务，当选择任务完成时，提交任务完成
                        descr = "任务完成";
                        T_Task_HistoryDAO finishdao = new T_Task_HistoryDAO();
                        T_Task_History finish = new T_Task_History()
                        {
                            taskId = Convert.ToInt32(tid),
                            userName = user.name,
                            content = "任务完成,等待验收",
                            descr = descr,
                            userId=uid,
                        };
                        i = finishdao.Add(finish); 
                        T_TaskDAO Taskupdatedao = new T_TaskDAO();

                        T_Task takk = Taskupdatedao.GetById(Convert.ToInt32(tid));
                        takk.endTime = DateTime.Now;

                        takk.statusId = 4;
                        Taskupdatedao.Update(takk);

                        return Success("提交成功，等待验收", null);
                    }
                    else
                    {
                        T_Task_History history = new T_Task_History();

                        //汇报完成同时生成一条历史记录
                        history.taskId = task.id;
                        history.userName = user.name;
                        history.content = Request["content"];
                        history.descr = descr;
                        history.userId = uid;
                        
                        i = T_Task_HistoryDao.Add(history);

                    };
                    if (i > 0)
                    {
                        return Success("汇报成功！", null);
                    }
                    else
                    {
                        return Fail("添加失败，请重试。");
                    }
                }
            }
            else
            {
                return Fail("提交失败，请填写完各项再提交。");
            }
        }


        //---------------删除任务、删除任务记录--------------
        public ActionResult deleteC(String cid, String tid)
        {
            if (String.IsNullOrWhiteSpace(cid))
            {
                return Fail("error");
            }

            String uid = Convert.ToString(Session["uid"]);
            T_Task_HistoryDAO deledao = new T_Task_HistoryDAO();
            T_Task_History deleC = deledao.GetById(Convert.ToInt32(cid));
            deleC.id = Convert.ToInt32(cid);
            if(deleC.descr == "发布任务"){
                int roleid = Convert.ToInt32(Session["roleid"]);
                if(roleid == 2){
                    List<T_Task_History> l =  deledao.GetAllByModel(new T_Task_History() {taskId = Convert.ToInt32(tid)});
                    //--------  一方管理员删除任务时，先删除所有的历史记录，再删除任务
                    foreach(T_Task_History t in l){
                        deledao.Del(Convert.ToInt32(t.id));
                    }

                    T_TaskDAO tdao = new T_TaskDAO();
                    T_Task deleT = tdao.GetById(Convert.ToInt32(tid));
                    deleT.id = Convert.ToInt32(Session["tid"]);
                    Session["fid"] = tid;
                    int result = tdao.Del(Convert.ToInt32(Session["tid"]));
                    if (result > 0)
                    {
                        return Success("已删除！", null);
                    }
                    else
                    {
                        return Success("未通过！", null);
                    }
                }
                else
                {
                    deledao.Del(Convert.ToInt32(cid));
                    T_TaskDAO tdao = new T_TaskDAO();
                    T_Task deleT = tdao.GetById(Convert.ToInt32(tid));
                    deleT.id = Convert.ToInt32(Session["tid"]);
                    Session["fid"] = tid;
                    int result = tdao.Del(Convert.ToInt32(Session["tid"]));
                    if (result > 0)
                    {
                        return Success("已删除！", null);
                    }
                    else
                    {
                        return Success("未通过！", null);
                    }
                }
            }
            else
            {
                //删除任务汇报
                deledao.Del(Convert.ToInt32(cid));
            }
            return Success("已删除！", null);
        }


        //----修改已提交过的数据
        public ActionResult changeC(String cid, String cont)
        {
            if (String.IsNullOrWhiteSpace(cid))
            {
                return Fail("error");
            }

            String uid = Convert.ToString(Session["uid"]);
            T_Task_HistoryDAO hdao = new T_Task_HistoryDAO();
            T_Task_History hid = hdao.GetByModel(new T_Task_History() { userId = uid });

            if (uid != hid.userId)
            {
                return Fail("对不起，您没有权限修改...");
            }

            T_Task_History changeC = hdao.GetById(Convert.ToInt32(cid));
            changeC.id = Convert.ToInt32(cid);

            changeC.content = cont;

            int result = hdao.Update(changeC);
            if (result > 0)
            {
                return Success("修改成功！", null);
            }
            else
            {
                return Success("已通过，但未记录", null);
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
