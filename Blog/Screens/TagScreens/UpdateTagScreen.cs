using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.TagScreens
{
    public static class UpdateTagScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Atualizar tag");
            Console.WriteLine("---------------");

            Console.Write("Id: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Nome: ");
            var name = Console.ReadLine()!;

            Console.Write("Slug: ");
            var slug = Console.ReadLine()!;

            Update(new Tag
            {
                Id = id,
                Name = name,
                Slug = slug
            });

            Console.ReadKey();
            MenuTagScreen.Load();
        }

        private static void Update(Tag tag)
        {
            try
            {
                var repository = new Repository<Tag>(Database.GetConnection());
                repository.Update(tag);
                Console.WriteLine($"Tag updated success.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possível atualizar a tag");
                Console.WriteLine($"\n\n{ex.Message}\n{tag.ToString()}");
            }
        }
    }
}
