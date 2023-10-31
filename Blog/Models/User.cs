using Dapper.Contrib.Extensions;

namespace Blog.Models
{
    [Table("[User]")]
    public class User
    {
        public User()
        {
            Name = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            Bio = string.Empty;
            Image = string.Empty;
            Slug = string.Empty;
            Roles = new List<Role>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        
        [Write(false)]
        public IList<Role> Roles { get; private set; }
    }
}
