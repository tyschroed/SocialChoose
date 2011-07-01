using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialChoose.Models;
using Facebook;

namespace SocialChoose.Controllers
{
    public class AuthController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            return View();
        }

        public ActionResult FBLogOn(string returnUrl)
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(HttpContext.Request.Url,Url.Action("FBOAuth","Auth"));
            var loginUri = oAuthClient.GetLoginUrl(new Dictionary<string, object> { { "state", returnUrl }, { "scope", "user_about_me,user_activities,user_education_history,user_hometown,user_interests,user_location,user_likes,user_religion_politics,user_work_history" } });
            return Redirect(loginUri.AbsoluteUri);
        }

        public ActionResult FBOAuth(string code, string state)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(Request.Url, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
                    oAuthClient.RedirectUri = new Uri(HttpContext.Request.Url, Url.Action("FBOAuth", "Auth"));
                    dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
                    string accessToken = tokenResult.access_token;

                    DateTime expiresOn = DateTime.MaxValue;

                    if (tokenResult.ContainsKey("expires"))
                    {
                        expiresOn = DateTimeConvertor.FromUnixTime(tokenResult.expires);
                    }

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me?fields=id,name");
                    long facebookId = Convert.ToInt64(me.id);


                    CurrentUser.FacebookToken = accessToken;
                    CurrentUser.FacebookExpiresDateTime = expiresOn;
                    CurrentUser.FacebookId = facebookId;
                    if (string.IsNullOrEmpty(CurrentUser.Name))
                        CurrentUser.Name = (string)me.name;

                    CurrentUser.Update();

                    // prevent open redirection attack by checking if the url is local.
                    if (Url.IsLocalUrl(state))
                    {
                        return Redirect(state);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
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
