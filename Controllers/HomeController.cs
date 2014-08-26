using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using FacebookReadMVC.Properties;
using FacebookReadMVC.App_Start;

namespace FacebookReadMVC.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public GroupReadContext context = new GroupReadContext();
        
        public ActionResult Index()
        {
            
            //FacebookClient fb = new FacebookClient("1430700287164125|L_mlhS4IgOHVRmp-oI3DrV2__OA");
            

             //ReadWrite.PostRetrieval(fb);
            //PostRetrieval(fb, "792998980739623");

            //dynamic me = fb.Post("/v2.1/256859081119610/feed", parameters);
            //var id = me.post;
            //var name = me.name;

            //parameters["message"] = "";
            //dynamic post = fb.Post("/v2.1/695439857195272/comments", parameters);//TFT Post 695439857195272
            //dynamic post = fb.Post("/v2.1/412444812227702/comments", parameters);//ShanilTest Post 412444812227702


            //int counter = 1;


            //Console.WriteLine("ID: " + id);
            //Console.WriteLine("Name: " + name);

            return View();

        //    context.db.GetStats();

        //    return Json(context.db.Server.BuildInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            //ApplicationUserManager abc = HttpContext.GetOwinContext().
            
            var owinContext = HttpContext.GetOwinContext();
            var authentication = owinContext.Authentication;
            var user = authentication.User;
            var claim = (user.Identity as ClaimsIdentity).FindFirst("urn:facebook:access_token");

            //var access_token = claim.FindAll("FacebookAccessToken").First().Value;
            var fb = new FacebookClient(claim.Value);
            //ReadWrite.PostRetrieval(fb);
            //ReadWrite.PostRetrieval(fb, "791222680917253");
            //HttpContext.User. 
            //dynamic token = fb.Get("oauth/access_token", new
            //{
            //    client_id = "1430700287164125",
            //    client_secret = "ee985ca3401facc9bb1c719807a13c22",
            //    redirect_uri = "https://localhost:44300/",
            //    code = Request.QueryString["code"]
            //});
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}