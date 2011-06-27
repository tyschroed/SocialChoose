using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SocialChoose
{
    public static class Settings
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                var db = ConfigurationManager.ConnectionStrings["Database"];
                return _connectionString ?? (db != null ? db.ConnectionString : null);
            }

            set
            {
                _connectionString = value;
            }
        }

        public static string AuthenticationSecretKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthenticationSecretKey"];
            }
        }

        public static string CalaisApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["CalaisApiKey"];
            }
        }
    }
}
