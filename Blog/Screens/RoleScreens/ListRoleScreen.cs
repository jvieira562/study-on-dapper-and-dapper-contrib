using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.RoleScreens
{
    public class ListRoleScreen
    {
        protected ListRoleScreen()
        {
            
        }
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Lista de roles");
            Console.WriteLine("-------------------");
            List();
            Console.ReadKey();
            MenuRoleScreen.Load();
        }
        private static void List()
        {
            var repository = new Repository<Role>(Database.GetConnection());
            var roles = repository.Get();

            Console.WriteLine("ID | NOME + SLUG\n-------------------");
            foreach (var item in roles)
            {
                var zeroLeft = string.Empty;
                if (item.Id < 10)
                    zeroLeft = "0";
                Console.WriteLine($"{zeroLeft}{item.Id} | {item.Name} - ({item.Slug})");
            }
        }
    }
}
