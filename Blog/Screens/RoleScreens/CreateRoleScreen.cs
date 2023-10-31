using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.RoleScreens
{
    public static class CreateRoleScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Nova role");
            Console.WriteLine("---------------");

            Console.Write("Nome: ");
            var name = Console.ReadLine()!;

            Console.Write("Slug: ");
            var slug = Console.ReadLine()!;

            Create(new Role { 
                Name = name,
                Slug = slug }) ;

            Console.ReadKey();
            MenuRoleScreen.Load();
        }

        private static void Create(Role role)
        {
            try
            {
                var repository = new Repository<Role>(Database.GetConnection());
                repository.Create(role);
                Console.WriteLine($"Role created success.{role.Details()}");

            } catch (Exception ex) {
                Console.WriteLine("Unable to create this role!");
                Console.WriteLine($"\n\n{ex.Message}\n{role.Details()}");
            }
        }
    }
}
