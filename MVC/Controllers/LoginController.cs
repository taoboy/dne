using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.gdce_task.Model;
using Newtonsoft.Json;


namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        T_UserDAO entityDao = new T_UserDAO();
        // POST: /Login/
        protected JsonResult Fail(String msg)
        {
            JsonModel json = new JsonModel(false, msg, null);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult Success(String msg, Object data)
        {
            JsonModel json = new JsonModel(true, msg, data);

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoginCheck()
        {
            String account = Request["account"];
            String pass = Request["password"];
            if (String.IsNullOrWhiteSpace(account) || String.IsNullOrWhiteSpace(pass))
            {
                return Fail("没有足够的参数");
            }
            T_User entity = new T_User() { account = account, password = pass };
            entity = entityDao.GetByModel(entity);
             if (entity == null)
             {
                 return Fail("用户名和密码不匹配");
             }
             else {
                 pass = "";
                 entity.password = "";
                 Session["uid"] = entity.id;
                 Session["username"] = entity.name;
                 Session["descr"] = entity.descr;
                 Session["create"] = entity.createTime;
                 Session["phone"] = entity.phone;
                 Session["dept"] = entity.roleIdEntity.descr;
                 Session["roleid"] = entity.roleId;

                 //登录成功后，将用户信息用Session存起来，方便调用。

                 
              //   var tempEntity = new { roleid=entity.roleId };
             //    string json5 = JsonConvert.SerializeObject(tempEntity);

                 pass = "";
                 //登录后密码清空。
                 T_TaskDAO tdao = new T_TaskDAO();
                 tdao.updateStatus();
                 





                 return Success("登录成功", entity);
                // 
             }
        }
        //----------个人中心视图-----------------------
        public ActionResult UserCenter()
        {
            return View();
        }
        //----------修改密码-----------------------
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


        //----------修改联系方式-----------------------
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

        public ActionResult result(T_User user)
        {
            return View();
        }
        //----------登录界面视图-----------------------
        public ActionResult Login()
        {
            return View();
        }
    }
}
