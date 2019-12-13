using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BookingAppStore.Models;
using BookingAppStore.util;

namespace BookingAppStore.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();
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
        public ActionResult Index(int page = 1)
        {
            ViewBag.roleid = getUserRole();

            int pageSize = 100; // количество объектов на страницу
            IEnumerable<Book> booksPerPages = db.Books
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var books = booksPerPages;
            ViewBag.Books = books;
            int hour = DateTime.Now.Hour;
            ViewData["Head"] = "My shop";
            return View();
            
        }
        public ActionResult GetImage(int id)

        {

            ViewBag.BookId = id;
            string path = "../../Uploaded Files" + Convert.ToString(id) + ".jpg";
            return new ImageResult(path);
        }


		
		[HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.roleid = getUserRole();

            ViewBag.BookId = id;
            return View();
        }
		
		[HttpPost]
		public ActionResult Buy(Purchase purchase)

		{
			if (ModelState.IsValid)
			{
				purchase.Date = DateTime.Now;
				db.Purchase.Add(purchase);
				db.SaveChanges();
				return RedirectToAction("Thanks", purchase);

			}
			else
			{
				ViewBag.BookId = purchase.BookId;
				return View();
			}
		}
		
		[HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }
		
		[HttpPost]
        public ActionResult CreateBook(Book book, HttpPostedFileBase imageFile)
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
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.roleid = getUserRole();

            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book, HttpPostedFileBase imageFile)
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
        [HttpGet]
        public ActionResult Delete(int id)
        {
            ViewBag.roleid = getUserRole();

            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
		
		public ActionResult EditBook(int? id)
        {
            ViewBag.roleid = getUserRole();

            if (id == null)
            {
                return HttpNotFound();
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }
        [HttpPost]
		
		public ActionResult EditBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ViewResult Thanks(Purchase purchase)
        {
            ViewBag.roleid = getUserRole();

            foreach (Book b in db.Books)
                if (b.Id == purchase.BookId)
                {
                    ViewBag.BookName = b.Name;
                    ViewBag.Name = purchase.Person;
                    ViewBag.Address = purchase.Email;
                }






            return View();
        }


		
        public ActionResult About()
        {
            ViewBag.Message = "О магазине";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакт";

            return View();
        }


        public ActionResult GetVoid(int id)
        {
            if (id < 3)
            {
                return new HttpStatusCodeResult(404);
            }
            return View();
        }
        public FileResult GetFile(int id)
        {
            ViewBag.BookId = id;
            // Путь к файлу
            string file_path = Server.MapPath("../../Files/b" + (id) + ".pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            return File(file_path, file_type);
        }

		public ActionResult Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
				if (Request.Cookies["auth"] != null)
				{
					var c = new HttpCookie("auth");
					c.Expires = DateTime.Now.AddDays(-1);
					Response.Cookies.Add(c);
				}
				return RedirectToAction("Contact");

			}
			else return RedirectToAction("Contact");

		}

		//public FileResult GetBytes()
		//{
		//    string path = Server.MapPath("~/Files/shine.jpg");
		//    byte[] mas = System.IO.File.ReadAllBytes(path);
		//    string file_type = "application/jpg";
		//    string file_name = "shine.jpg";
		//    return File(mas, file_type, file_name);
		//}
		// GET: Home  
		//public ActionResult UploadFiles()
		//{
		//    return View();
		//}
		//[HttpPost]
		//public ActionResult UploadFiles(HttpPostedFileBase[] files)
		//{

		//    //Ensure model state is valid  
		//    if (ModelState.IsValid)
		//    {   //iterating through multiple file collection   
		//        foreach (HttpPostedFileBase file in files)
		//        {
		//            //Checking file is available to save.  
		//            if (file != null)
		//            {
		//                var InputFileName = Path.GetFileName(file.FileName);
		//                var ServerSavePath = Path.Combine(Server.MapPath("~/Uploaded Files/") + InputFileName);
		//                //Save file to server folder  
		//                file.SaveAs(ServerSavePath);
		//                //assigning file uploaded status to ViewBag for showing message to user.  
		//                ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
		//            }

		//        }
		//    }
		//    return View();

		//}



	}
}