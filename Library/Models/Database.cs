using System;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public static class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}