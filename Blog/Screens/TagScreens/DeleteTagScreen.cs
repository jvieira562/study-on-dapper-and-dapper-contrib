using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.TagScreens
{
    public static class DeleteTagScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Delete tag");
            Console.WriteLine("---------------");

            Console.Write("Id: ");
            var id = int.Parse(Console.ReadLine()!);


            Delete(id);

            Console.ReadKey();
            MenuTagScreen.Load();
        }

        private static void Delete(int id)
        {
            try
            {
                var repository = new Repository<Tag>(Database.GetConnection());
                repository.Delete(id);
                Console.WriteLine($"Tag deleted success.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possível excluir a tag");
                Console.WriteLine($"\n{ex.Message}");
            }
        }
    }
}
