using Blog.Screens.RoleScreens;
using Blog.Screens.TagScreens;

namespace Blog
{
    public class Program
    {
        private Program() { }

        static void Main(string[] args)
        {
            Database.GetConnection().Open();

            Load();

            Console.ReadKey();
            Database.GetConnection().Close();
        }
        private static void Load()
        {
            Console.Clear();
            Console.WriteLine("\tMeu Blog");
            Console.WriteLine("------------------------");
            Console.WriteLine("O que deseja fazer?");
            Console.WriteLine();
            Console.WriteLine("1 - Gestão de usuário");
            Console.WriteLine("2 - Gestão de perfil");
            Console.WriteLine("3 - Gestão de categoria");
            Console.WriteLine("4 - Gestão de tag");
            Console.WriteLine("5 - Vincular perfil/usuário");
            Console.WriteLine("6 - Vincular post/tag");
            Console.WriteLine("7 - Relatórios");
            Console.WriteLine("\n");

            var option = short.Parse(Console.ReadLine()!);

            switch (option)
            {
                case 2:
                    MenuRoleScreen.Load();
                    break;
                case 4:
                    MenuTagScreen.Load();
                    break;
                default: Load(); break;
            }
        }

    }
}