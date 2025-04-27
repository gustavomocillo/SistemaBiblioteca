using Microsoft.Win32;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Menus
{
    internal class MenuUsuario()
    {
        public static void Exibir(Usuario? usuario)
        {
            var categoriaService = new CategoriaService();
            var emprestimoService = new EmprestimoService();
            var livroService = new LivroService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"==== BEM-VINDO {usuario.Nome.ToUpper()} ====");
                Console.WriteLine("1 - Criar Emprestimo");
                Console.WriteLine("2 - Consultar meus empréstimos");
                Console.WriteLine("3 - Registrar devolução");
                Console.WriteLine("4 - Consultar livros");
                Console.WriteLine("5 - Buscar livros");
                Console.WriteLine("6 - Consultar categorias de livros");
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
                        emprestimoService.CriarEmprestimoUsuario(usuario);
                        break;
                    case 2:
                        Console.Clear();
                        emprestimoService.ConsultarEmprestimosDoUsuario(usuario);
                        break;
                    case 3:
                        Console.Clear();
                        emprestimoService.RegistrarDevolucao();
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("1 - Consultar todos os livros");
                        Console.WriteLine("2 - Consultar apenas os livros disponíveis");
                        Console.WriteLine("0 - Sair");

                        if (!int.TryParse(Console.ReadLine(), out int opcao2))
                        {
                            Console.WriteLine("Opção inválida. [Enter]");
                            Console.ReadKey();
                            continue;
                        }

                        switch (opcao2)
                        {
                            case 0: 
                                Environment.Exit(0);
                                break;
                            case 1:
                                Console.Clear();
                                livroService.ConsultarTodosLivros();
                                break;
                            case 2:
                                Console.Clear();
                                livroService.ConsultarLivrosDisponiveis();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("1 - Buscar livro por ID");
                        Console.WriteLine("2 - Buscar livro por Nome");
                        Console.WriteLine("2 - Buscar livro por Categoria");
                        Console.WriteLine("0 - Sair");

                        if (!int.TryParse(Console.ReadLine(), out int opcao3))
                        {
                            Console.WriteLine("Opção inválida. [Enter]");
                            Console.ReadKey();
                            continue;
                        }

                        switch (opcao3)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                Console.Clear();
                                livroService.BuscarPorId();
                                break;
                            case 2:
                                Console.Clear();
                                livroService.BuscarPorNome();
                                break;
                            case 3:
                                Console.Clear();
                                livroService.BuscarPorCategoria();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    case 6:
                        Console.Clear();
                        categoriaService.ConsultarCategorias();
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
