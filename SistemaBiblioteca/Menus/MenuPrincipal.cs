using SistemaBiblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Menus
{
    internal class MenuPrincipal
    {
        public static void Exibir()
        {
            var usuarioService = new UsuarioService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== BEM-VINDO À BIBLIOTECA ====");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Cadastrar-se");
                Console.WriteLine("0 - Sair");

                if (!int.TryParse(Console.ReadLine(), out int opcao))
                {
                    Console.WriteLine("Opção inválida. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                switch (opcao)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.Clear();
                        usuarioService.Login();
                        break;
                    case 2:
                        Console.Clear();
                        usuarioService.CriarUsuario();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        continue;
                }
            }
        }
    }
}