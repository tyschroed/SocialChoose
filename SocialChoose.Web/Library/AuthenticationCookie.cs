using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nen.Security.Cryptography;
using SocialChoose.Domain;
using SocialChoose.Domain.Entities;

namespace SocialChoose.Library
{
    public static class AuthenticationCookie
    {
        /// <summary>
        /// Create a new authentication cookie for the specified user.
        /// </summary>
        /// <param name="user">The user for which the cookie will be created.</param>
        /// <returns></returns>
        public static HttpCookie Create(User user)
        {
            var authString = user.Id.ToString() + ":" + DateTime.UtcNow.Ticks;
            return new HttpCookie("SocialChoose-Auth", Hash.HmacSha1(Settings.AuthenticationSecretKey, authString) + ":" + authString);
        }

        /// <summary>
        /// Returns true if the specified value represents a valid authentication cookie. In order for the cookie to be considered valid,
        /// the following must be true:
        /// 
        /// - The account ID is an integer
        /// - The timestamp is valid
        /// - The content of the cookie matches its HMAC-SHA1 hash
        /// 
        /// The method does not perform any further processing of the account ID to ensure its validity.
        /// </summary>
        /// <param name="value">The cookie value.</param>
        /// <returns>True if the cookie is valid, false otherwise.</returns>
        public static bool Verify(string value)
        {
            var components = value.Split(':');

            if (components.Length != 3)
                return false;

            var hashCode = components[0];
            int accountId;
            long timeStampTicks;

            if (!int.TryParse(components[1], out accountId))
                return false;

            if (!long.TryParse(components[2], out timeStampTicks))
                return false;

            if (Hash.HmacSha1(Settings.AuthenticationSecretKey, value.Substring(components[0].Length + 1)) != hashCode)
                return false;

            return true;
        }
    }
}