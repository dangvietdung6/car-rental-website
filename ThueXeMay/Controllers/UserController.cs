using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ThueXeMay.Common;
using ThueXeMay.Models;
using ThueXeMay.User;

namespace ThueXeMay.Controllers
{
    public class UserController : Controller
    {
        private RENT_MOTOREntities db = new RENT_MOTOREntities();
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckEmail(model.email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new user();
                    user.email = model.email;
                    user.password = Encryptor.MD5Hash(model.password);
                    user.firstName = model.firstName;
                    user.lastName = model.lastName;
                    user.address = model.address;
                    user.gender = model.gender;
                    user.phoneNumber = model.phoneNumber;
                    /*user.avatar = model.avatar;
                    user.image1 = model.image1;
                    user.image2 = model.image2;*/
                    user.status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng Ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }

                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.email, Encryptor.MD5Hash(model.password));
                if (result == 1)
                {
                    var user = dao.GetById(model.email);
                    var userSession = new UserLogin();
                    userSession.email = user.email;
                    userSession.id_user = user.id_user;
                    userSession.firstName = user.firstName;
                    userSession.lastName = user.lastName;
                    userSession.phoneNumber = user.phoneNumber;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("~/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không dúng.");
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Rename(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user nv = db.users.Find(id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return View("Rename", nv);
        }
        [HttpPost]
        public ActionResult Rename(int? id, string fnnew, string lnnew, string adnew, string pnnew, string gdnew)
        {
            try
            {
                user nv = db.users.Find(id);
                nv.firstName = fnnew;
                nv.lastName = lnnew;
                nv.address = adnew;
                nv.phoneNumber = pnnew;
                nv.gender = gdnew;
                db.SaveChanges();
                ThongBao("Đổi tên thành công :v", "success");
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View("Error");
            }
        }
        public ActionResult ChangePsw(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user nv = db.users.Find(id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return View(nv);
        }

        [HttpPost]
        public ActionResult ChangePsw(int? id, string pswOld, string pswNew)
        {
            try
            {
                user nv = db.users.Find(id);
                if (nv.password == Encryptor.MD5Hash(pswOld))
                {
                    nv.password = Encryptor.MD5Hash(pswNew);
                    db.SaveChanges();
                    ThongBao("Đổi mật khẩu thành công :v", "success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ThongBao("Mật khẩu hiện tại chưa chính xác :v", "error");
                    return RedirectToAction("ChangePsw", new { id = id });
                }
            }
            catch
            {
                return View("Error");
            }

        }

        protected void ThongBao(string message, string type)
        {
            TempData["ThongBao"] = message;
            if (type == "success")
            {
                TempData["Type"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["Type"] = "alert-waring";
            }
            else if (type == "error")
            {
                TempData["Type"] = "alert-danger";
            }
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("~/");
        }



        [HttpGet]
        public ActionResult cmt(int id)
        {
            //var cmt = db.comments.Find(id);
            var query = (from b in db.blogs
                         join c in db.comments on b.id equals c.id
                         where c.id_user == id
                         orderby c.date
                         select new CMT()
                         {
                             Title = b.title,
                             Content = c.content,
                             Date = c.date,
                         }).ToList();
            //query.OrderByDescending(a => a.Date);
            return View(query);
        }

        [HttpGet]
        public ActionResult vehicle(int id)
        {
            var query = (from a in db.rents
                         join b in db.rentDetails
                         on a.id_rent equals b.id_rent
                         join c in db.bills
                         on a.id_rent equals c.id_rent
                         join d in db.bikes
                         on b.id_bike equals d.id_bike
                         where a.id_user == id
                         select new vehicle()
                         {
                             Name = d.name,
                             Note = a.note,
                             DateStart = c.date_start,
                             DateEnd = c.date_end,
                             Amount = b.amount,
                         }).ToList();
            //query.OrderByDescending(a => a.Date);
            return View(query);
        }







    }
}