using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.RoleScreens
{
    public static class DeleteRoleScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Delete role");
            Console.WriteLine("---------------");

            Console.Write("Id: ");
            var id = int.Parse(Console.ReadLine()!);


            Delete(id);

            Console.ReadKey();
            MenuRoleScreen.Load();
        }

        private static void Delete(int id)
        {
            try
            {
                var repository = new Repository<Role>(Database.GetConnection());
                repository.Delete(id);
                Console.WriteLine($"Role deleted success.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete this role!");
                Console.WriteLine($"\n{ex.Message}");
            }
        }
    }
}
