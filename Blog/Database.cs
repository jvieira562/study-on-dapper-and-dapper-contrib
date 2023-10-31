using Microsoft.Data.SqlClient;

namespace Blog
{
    public static class Database
    {
        private static readonly SqlConnection Connection 
            = new SqlConnection("Server=jvieiradev.database.windows.net;Database=blog;User ID=balta_course;Password=@1q2w!Q@W; TrustServerCertificate=True");

        public static SqlConnection GetConnection()
        {
            return Connection;
        }
    }

}
