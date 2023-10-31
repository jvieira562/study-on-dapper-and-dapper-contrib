using Dapper;

using Blog.Models;

using Microsoft.Data.SqlClient;

namespace Blog.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly SqlConnection _connection;
        
        public UserRepository(SqlConnection connection) 
            : base(connection)
                => _connection = connection;

        public List<User> GetWithRoles()
        {
            string query = 
                @"SELECT 
	                [User].*,
	                [Role].*
                FROM [User]
	                LEFT JOIN [UserRole]
		                ON [UserRole].[UserId] = [User].[Id]
	                LEFT JOIN [Role] 
                        ON [UserRole].[RoleId] = [Role].[Id];";
            
            var users = new List<User>();

            users = _connection.Query<User, Role, User>(
                sql : query,
                map : (user, role) =>
                {
                    var usr = users.Find(x => x.Id == user.Id);

                    if(usr == null)
                    {
                        usr = user;
                        if(role != null)
                            usr.Roles.Add(role);
                        users.Add(usr);
                    } else
                        usr.Roles.Add(role);

                    return user;
                },
                splitOn: "Id").ToList();


            return users;
        }
    }
}
