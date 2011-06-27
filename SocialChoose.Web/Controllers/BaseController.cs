using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities=SocialChoose.Domain.Entities;
using SocialChoose.Library;

namespace SocialChoose.Controllers
{
    public class BaseController : Controller
    {
        public Entities.User _currentUser;
        public Entities.User CurrentUser
        {
            get
            {
                if (_currentUser != null)
                    return _currentUser;
                else
                    _currentUser = GetCurrentUser();

                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        private Entities.User GetCurrentUser() 
        {
            var authCookie = Request.Cookies["SocialChoose-Auth"];
            if (authCookie != null && AuthenticationCookie.Verify(authCookie.Value))
            {
                var components = authCookie.Value.Split(':');
                var accountId = int.Parse(components[1]);

                return Entities.User.Load(accountId);
            }

            return null;
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var isAuthenticated = false;
            if (CurrentUser != null)
            {
                isAuthenticated = true;
            }

            if (IsLoginRequired && !isAuthenticated)
            {
                filterContext.Result = RedirectToAction("Login", "Auth", new { redirectPath = Url.RouteUrl(filterContext.RouteData.Values) });
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.CurrentUser = CurrentUser;
            Response.AddHeader("X-UA-Compatible", "IE=edge");
        }

        /// <summary>
        /// Override this method in a controller to control whether or not authentication is 
        /// required by default to access its actions. The default is true.
        /// </summary>
        public virtual bool IsLoginRequired
        {
            get { return true; }
        }
    }
}
