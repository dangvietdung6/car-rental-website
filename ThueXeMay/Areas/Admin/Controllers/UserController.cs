using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThueXeMay.Common;
using ThueXeMay.Models;
using ThueXeMay.User;

namespace ThueXeMay.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private RENT_MOTOREntities db = new RENT_MOTOREntities();

        // GET: Admin/user
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(page, pageSize);
            return View(model);
        }

        /*public ActionResult Index()
        {
            return View(db.users.ToList());
        }*/

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);

            return View(user);
        }



        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]

        public ActionResult Create(user user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var encryptedMd5Pas = Encryptor.MD5Hash(user.password);
                user.password = encryptedMd5Pas;

                long id = dao.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm người dùng thành công");
                }
            }
            return View("Index");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,firstname,lastName,address,gender,phoneNumber")] user user)
        {

            if (ModelState.IsValid)
            {
                db.Set<user>().AddOrUpdate(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}