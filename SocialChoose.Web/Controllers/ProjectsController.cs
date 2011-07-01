using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Calais;
using System.Net;
using System.IO;

namespace SocialChoose.Controllers
{
    public class ProjectsController : BaseController
    {
        private string DonorsChooseApiUrl
        {
            get
            {
                return string.Format("http://api.donorschoose.org/common/json_feed.html?APIKey={0}", Settings.DonorsChooseApiKey);
            }
        }
        public ActionResult Index()
        {
            if (CurrentUser == null)
                return RedirectToAction("Index", "Link");

            return View();
        }
        public override bool IsLoginRequired
        {
            get
            {
                return false;
            }
        }

        public ActionResult Retrieve()
        {
            var User = CurrentUser;

            var client = new FacebookClient(CurrentUser.FacebookToken);
            dynamic me;
            try
            {
                me = client.Get("me");
            }
            catch (FacebookOAuthException ex)
            {
                var url = Url.Action("FBLogOn", "Auth", new { returnUrl = Url.Action("Index","Projects") });
                return Json(new { authRequired = url },JsonRequestBehavior.AllowGet);
            }

            return Json(me, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string value)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("{0}&keywords={1}", DonorsChooseApiUrl, value));

            var resp = req.GetResponse();
            using (var stream = resp.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    var content = sr.ReadToEnd();
                    
                    return Json(content.Trim(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        //public ActionResult Analyze()
        //{
        //    var User = CurrentUser;

        //    var client = new FacebookClient(CurrentUser.FacebookToken);
        //    dynamic me;
        //    try
        //    {
        //        me = client.Get("me");
        //    }
        //    catch (FacebookOAuthException ex)
        //    {
        //        return RedirectToAction("FBLogOn", "Auth", new { returnUrl = HttpContext.Request.Url.ToString() });
        //    }

        //    CalaisDotNet calaisAPI = new CalaisDotNet(Settings.CalaisApiKey, me.ToString(), CalaisInputFormat.RawText);

        //    var document = calaisAPI.Call<CalaisSimpleDocument>();



        //    return Json(new { output = document.RawOutput }, JsonRequestBehavior.AllowGet);
        //}

    }
}
