using Blog.Models;
using Blog.Repositories;
using Microsoft.Data.SqlClient;

namespace Blog.Screens.RoleScreens
{
    public static class UpdateRoleScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Atualizar role");
            Console.WriteLine("---------------");

            Console.Write("Id: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Nome: ");
            var name = Console.ReadLine()!;

            Console.Write("Slug: ");
            var slug = Console.ReadLine()!;

            Update(new Role
            {
                Id = id,
                Name = name,
                Slug = slug
            });

            Console.ReadKey();
            MenuRoleScreen.Load();
        }

        private static void Update(Role role)
        {
            try
            {
                var repository = new Repository<Role>(Database.GetConnection());
                repository.Update(role);
                Console.WriteLine($"Role updated success.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to update this role!");
                Console.WriteLine($"\n\n{ex.Message}\n{role.ToString()}");
            }
        }
    }
}
