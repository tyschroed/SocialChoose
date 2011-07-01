using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialChoose.Library;
using Entities = SocialChoose.Domain.Entities;
using Facebook;

namespace SocialChoose.Controllers
{
    public class LinkController : BaseController
    {

        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                var user = Entities.User.Create();
                Response.SetCookie(AuthenticationCookie.Create(user));
                CurrentUser = user;
            }
            else
            {
                if (CurrentUser.FacebookToken != null)
                    ViewData["HasFacebook"] = true;
            }
            

            return View();
        }

        public override bool IsLoginRequired
        {
            get
            {
                return false;
            }
        }

    }
}
