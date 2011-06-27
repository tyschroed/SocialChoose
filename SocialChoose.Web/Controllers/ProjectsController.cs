using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Calais;

namespace SocialChoose.Controllers
{
    public class ProjectsController : BaseController
    {
        public ActionResult Index()
        {
            if (CurrentUser == null)
                RedirectToAction("Index", "Find");

            return View();
        }

        public ActionResult Analyze()
        {
            var User = CurrentUser;

            var client = new FacebookClient(CurrentUser.FacebookToken);
            dynamic me = client.Get("me");

            CalaisDotNet calaisAPI = new CalaisDotNet(Settings.CalaisApiKey, me.ToString(), CalaisInputFormat.RawText);

            var document = calaisAPI.Call<CalaisSimpleDocument>();



            return Json(new { output = document.RawOutput }, JsonRequestBehavior.AllowGet);
        }

    }
}
