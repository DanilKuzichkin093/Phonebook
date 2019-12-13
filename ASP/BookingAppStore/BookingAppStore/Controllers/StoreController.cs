using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingAppStore.Models;

namespace BookingAppStore.Controllers
{
    public class StoreController : Controller
    {
        private BookContext db = new BookContext();

        UserContext dbU = new UserContext();

        public int getUserId()
        {
            int userId = 0;
            foreach (var u in dbU.Users)
            {
                if (u.Email == User.Identity.Name)
                {
                    userId = u.Id;
                    break;
                }
            }
            return userId;
        }
        public int getUserRole()
        {
            int roleid = 0;
            foreach (var u in dbU.Users)
            {
                if (u.Email == User.Identity.Name)
                {
                    roleid = u.RoleId;
                    break;
                }
            }
            return roleid;
        }

        public ActionResult Index()
        {
            ViewBag.roleid = getUserRole();

            return View(db.Books.ToList());
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.roleid = getUserRole();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            ViewBag.roleid = getUserRole();

            return View();
        }
		public ActionResult UploadFiles()
		{
			return View();
		}
		[HttpPost]
		public ActionResult UploadFiles(HttpPostedFileBase[] files)
		{

			//Ensure model state is valid  
			if (ModelState.IsValid)
			{   //iterating through multiple file collection   
				foreach (HttpPostedFileBase file in files)
				{
					//Checking file is available to save.  
					if (file != null)
					{
						var InputFileName = Path.GetFileName(file.FileName);
						var ServerSavePath = Path.Combine(Server.MapPath("~/Uploaded Files") + InputFileName);
						//Save file to server folder  
						file.SaveAs(ServerSavePath);
						//assigning file uploaded status to ViewBag for showing message to user.  
						ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
					}

				}
			}
			return View();

		}

		// POST: Store/Create

		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Phone,Email,Image")] Book book, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    book.Image = "../../Photo/" + imageFile.FileName;
                    imageFile.SaveAs(Server.MapPath(@"~/Photo/") + imageFile.FileName);
                }

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.roleid = getUserRole();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Phone,Email")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.roleid = getUserRole();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
