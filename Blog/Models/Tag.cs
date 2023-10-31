using Dapper.Contrib.Extensions;

namespace Blog.Models
{
    [Table("[Tag]")]
    public class Tag
    {
        public Tag()
        {
            Name = string.Empty;
            Slug = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }


        public string Details()
        {
            return $"\nDetalhes do objeto:\nId: {Id}\nName: {Name}\nSlug: {Slug}\n";
        }
    }
}
