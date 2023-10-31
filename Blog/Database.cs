using Microsoft.Data.SqlClient;

namespace Blog
{
    public static class Database
    {
        private static readonly SqlConnection Connection 
            = new SqlConnection("");

        public static SqlConnection GetConnection()
        {
            return Connection;
        }
    }

}
