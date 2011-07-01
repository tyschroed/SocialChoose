using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SocialChoose.Domain.Entities
{
    public static class DatabaseConnection
    {
        public static SqlConnection Current;

        public static SqlConnection GetOpenConnection() 
        {
            var conn = Current;
            if(conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            return conn;
        }
    }
}
