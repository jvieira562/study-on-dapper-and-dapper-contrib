using Dapper.Contrib.Extensions;

namespace Blog.Models
{
    [Table("[Category]")]
    public class Category
    {
        public Category()
        {
            Name = string.Empty;
            Slug = string.Empty;
            Posts = new List<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public List<Post> Posts { get; set; }
    }
}
