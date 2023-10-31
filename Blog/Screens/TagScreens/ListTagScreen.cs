using Blog.Models;
using Blog.Repositories;
using Blog.Screens.RoleScreens;

namespace Blog.Screens.TagScreens
{
    public static class ListTagScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Lista de tags");
            Console.WriteLine("-------------------");
            List();
            Console.ReadKey();
            MenuRoleScreen.Load();
        }
        private static void List()
        {
            var repository = new Repository<Tag>(Database.GetConnection());
            var tags = repository.Get();

            Console.WriteLine("ID | NOME + SLUG\n-------------------");
            foreach (var item in tags)
            {
                var zeroLeft = string.Empty;
                if (item.Id < 10)
                    zeroLeft = "0";
                Console.WriteLine($"{zeroLeft}{item.Id} | {item.Name} - ({item.Slug})");
            }
        }
    }
}
