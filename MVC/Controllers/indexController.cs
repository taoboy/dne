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

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using com.gdce_task.Model;

namespace MVC.Controllers
{
    public class indexController : ControllerBase
    {
        //
        // GET: /index/

        public ActionResult Login()
        {
            return View("Login");
        }


        public ActionResult Logout()
        {
            Session["User"] = null;

            Response.Cookies["uname"].Expires = DateTime.Now;
            Response.Cookies["upass"].Expires = DateTime.Now;

            return RedirectToRoute(new { Controller = "index", Action = "Login", refer = Request.Url.AbsolutePath });
        }


        public ActionResult Home()
        {
            //PostBLL pbll = new PostBLL();
            //Post p = new Post();
            //p.categoryId = 3;
            //p.state = true;

            //List<Post> list = pbll.GetAllByModel(p);

            //return View("home", list);
            return View("home");
        }

        public JsonResult LoginChk(String username, String hid_pass, String code)
        {

            //String chkCode = Session["chkCode"].ToString();

            //if (code.ToLower() != chkCode.ToLower())//检查验证码
            //{
            //    return Fail("验证码错误");
            //}
            //else
            //{
            //    UsersBLL uBll = new UsersBLL();

            //    Users u = new Users();
            //    u.userName = username;
            //    u.password = BLLHelper.GetMD5(hid_pass);

            //    List<Users> us = uBll.GetAllByModel(u);

            //    if (us.Count > 0)
            //    {
            //        u = us[0];
            //        Session["User"] = u;
            //        String privious = Request.QueryString["refer"];

            //        //写入cookie以方便下次自动登录
            //        Response.Cookies["uname"].Value = username;
            //        Response.Cookies["uname"].Expires = DateTime.MaxValue;
            //        Response.Cookies["upass"].Value = hid_pass;
            //        Response.Cookies["upass"].Expires = DateTime.MaxValue;
                    
            //        return Success("登录成功！");
            //    }
            //    else
            //    {
            //        return Fail("用户名或密码错误");
            //    }
            //}

            return Success("登录成功！", null);
        }

        public void ImgCode()
        {
            int Width = 100;
            int Height = 30;

            string chkCode = string.Empty;

            //颜色列表，用于验证码、噪线、噪点
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkBlue };

            //字体列表，用于验证码
            string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };

            //验证码的字符集，去掉了一些容易混淆的字符
            char[] character ={ '2', '3', '4', '5', '6', '8', '9', 
                                'A', 'B', 'C', 'D', 'E','F', 'G', 
                                'H', 'J', 'K', 'L', 'M', 'N', 
                                'P', 'R', 'S', 'T', 'W', 'X', 'Y' };

            Random rnd = new Random();

            //生成验证码字符串
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }

            //保存验证码的Cookie
            //HttpCookie anycookie = new HttpCookie("validateCookie");
            //anycookie.Values.Add("ChkCode", chkCode);
            //HttpContext.Current.Response.Cookies["validateCookie"].Values["ChkCode"] = chkCode;

            Session["chkCode"] = chkCode;

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            //画噪线
            for (int i = 0; i < 5; i++)
            {
                int x1 = rnd.Next(Width);
                int y1 = rnd.Next(Height);
                int x2 = rnd.Next(Width);
                int y2 = rnd.Next(Height);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }

            //画验证码字符串
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 16);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(
                    chkCode[i].ToString(),
                    ft,
                    new SolidBrush(clr),
                    (float)i * 20 + 20,
                    (float)6
                    );
            }

            //画噪点
            for (int i = 0; i < 100; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }

            //清除该页输出缓存，设置该页无缓存
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "No-Cache");

            //将验证码图片写入内存流，并将其以"image/Png" 格式输出
            MemoryStream ms = new MemoryStream();

            try
            {
                bmp.Save(ms, ImageFormat.Png);
                Response.ClearContent();
                Response.ContentType = "image/Png";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                //显式释放资源
                bmp.Dispose();
                g.Dispose();
            }
        }

    }
}
