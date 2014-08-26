using Facebook;
using FacebookReadMVC.App_Start;
using FacebookReadMVC.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FacebookReadMVC.Controllers
{
    public class PostBackupController : Controller
    {
        public readonly GroupReadContext context = new GroupReadContext();
        // GET: PostBackup
        public ActionResult Index()
        {
            var posts = context.Posts.FindAll();
            return View(posts);
        }
        public ActionResult TakeBackup()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult TakeBackup(BackupPost post)
        {
            FacebookClient fb = GetFacebookClaim();
            ReadWrite.PostRetrieval(fb, post); //"791222680917253"
            return RedirectToAction("Index");
        }

        private FacebookClient GetFacebookClaim()
        {
            var owinContext = HttpContext.GetOwinContext();
            var authentication = owinContext.Authentication;
            var user = authentication.User;
            var claim = (user.Identity as ClaimsIdentity).FindFirst("urn:facebook:access_token");
            var fb = new FacebookClient(claim.Value);
            return fb;
        }

        public ActionResult Details(string Id)
        {
            var post = context.Posts.FindOneById(ObjectId.Parse(Id));
            return View(post);
        }
    }
}