using SistemaBiblioteca.Context;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Menus
{
    internal class MenuAdmin
    {
        public static void Exibir()
        {
            var autorService = new AutorService();
            var categoriaService = new CategoriaService();
            var usuarioService = new UsuarioService();
            var emprestimoService = new EmprestimoService();
            var livroService = new LivroService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== MENU DE ADMINISTRADOR ====");
                Console.WriteLine("1 - Menu Livro");
                Console.WriteLine("2 - Menu Emprestimo");
                Console.WriteLine("3 - Menu Usuário");
                Console.WriteLine("4 - Menu Autor");
                Console.WriteLine("5 - Menu Categoria");
                Console.WriteLine("0 - Sair");

                int opcao = ValidaNumero();

                switch (opcao)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("1 - Adicionar livro");
                        Console.WriteLine("2 - Consultar livros");
                        Console.WriteLine("3 - Buscar livros");
                        Console.WriteLine("4 - Atualizar livro");
                        Console.WriteLine("5 - Remover livro");
                        Console.WriteLine("0 - Sair");

                        int opcao1 = ValidaNumero();

                        switch (opcao1)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                Console.Clear();
                                livroService.AdicionarLivro();
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("1 - Consultar todos os livros");
                                Console.WriteLine("2 - Consultar apenas os livros emprestados");
                                Console.WriteLine("3 - Consultar apenas os livros disponíveis");
                                Console.WriteLine("0 - Sair");

                                int opcaoConsultarLivro = ValidaNumero();

                                switch (opcaoConsultarLivro)
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
                                        livroService.ConsultarLivrosEmprestados();
                                        break;
                                    case 3:
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
                            case 3:
                                Console.Clear();
                                Console.WriteLine("1 - Buscar por ID");
                                Console.WriteLine("2 - Buscar por Nome");
                                Console.WriteLine("3 - Buscar por Categoria");
                                Console.WriteLine("0 - Sair");

                                int opcaoBuscarLivro = ValidaNumero();

                                switch (opcaoBuscarLivro)
                                {
                                    case 0:
                                        Environment.Exit(0);
                                        break;
                                    case 1:
                                        livroService.BuscarPorId();
                                        break;
                                    case 2:
                                        livroService.BuscarPorNome();
                                        break;
                                    case 3:
                                        livroService.BuscarPorCategoria();
                                        break;
                                    default:
                                        Console.WriteLine("Opção inválida.");
                                        Console.ReadKey();
                                        continue;
                                }
                                break;
                            case 4:
                                livroService.AtualizarLivro();
                                break;
                            case 5:
                                livroService.RemoverLivro();
                                break;
                            default:
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("1 - Criar empréstimo");
                        Console.WriteLine("2 - Consultar empréstimos");
                        Console.WriteLine("3 - Registrar devolução");
                        Console.WriteLine("0 - Sair");

                        int opcao2 = ValidaNumero();

                        switch (opcao2)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                emprestimoService.CriarEmprestimoAdmin();
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("1 - Consultar todos os empréstimos");
                                Console.WriteLine("2 - Consultar empréstimos de um usuário");
                                Console.WriteLine("0 - Sair");

                                int opcaoConsultarEmprestimo = ValidaNumero();

                                switch (opcaoConsultarEmprestimo)
                                {
                                    case 0:
                                        Environment.Exit(0);
                                        break;
                                    case 1:
                                        emprestimoService.ConsultarTodosEmprestimos();
                                        break;
                                    case 2:
                                        Console.WriteLine("ID do Usuário que deseja consultar:");

                                        int idUsuario = ValidaNumero();

                                        using (var db = new AppDbContext())
                                        {
                                            var usuario = db.Usuarios.Find(idUsuario);

                                            if (usuario == null)
                                            {
                                                Console.WriteLine($"Não existe um Usuário com o ID {idUsuario}. Tente Novamente. [Enter]");
                                                Console.ReadKey();
                                                continue;
                                            }

                                            emprestimoService.ConsultarEmprestimosDoUsuario(usuario);
                                        }
                                            break;
                                    default:
                                        Console.WriteLine("Opção inválida.");
                                        Console.ReadKey();
                                        continue;
                                }
                                break;
                            case 3:
                                emprestimoService.RegistrarDevolucao();
                                break;
                            default:
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("1 - Criar usuário");
                        Console.WriteLine("2 - Consultar usúarios");
                        Console.WriteLine("3 - Atualizar usuário");
                        Console.WriteLine("4 - Remover usuário");
                        Console.WriteLine("0 - Sair");

                        int opcao3 = ValidaNumero();

                        switch (opcao3)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                usuarioService.CriarUsuario();
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("1 - Consultar todos os usuários");
                                Console.WriteLine("2 - Consultar usuários com empréstimo em andamento");
                                Console.WriteLine("0 - Sair");

                                int opcaoConsultarLivro = ValidaNumero();

                                switch (opcaoConsultarLivro)
                                {
                                    case 0:
                                        Environment.Exit(0);
                                        break;
                                    case 1:
                                        Console.Clear();
                                        usuarioService.ConsultarTodosUsuarios();
                                        break;
                                    case 2:
                                        Console.Clear();
                                        usuarioService.ConsultarUsuariosComEmprestimo();
                                        break;
                                    default:
                                        Console.Clear();
                                        Console.WriteLine("Opção inválida.");
                                        Console.ReadKey();
                                        continue;
                                }
                                break;
                            case 3:
                                Console.Clear();
                                usuarioService.AtualizarUsuario();
                                break;
                            case 4:
                                Console.Clear();
                                usuarioService.ExcluirUsuario();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("1 - Adicionar autor");
                        Console.WriteLine("2 - Consultar autores");
                        Console.WriteLine("3 - Atualizar autor");
                        Console.WriteLine("4 - Remover autor");
                        Console.WriteLine("0 - Sair");

                        int opcao4 = ValidaNumero();

                        switch (opcao4)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                Console.Clear();
                                autorService.AdicionarAutor();
                                break;
                            case 2:
                                Console.Clear();
                                autorService.ConsultarAutores();
                                break;
                            case 3:
                                Console.Clear();
                                autorService.AtualizarAutor();
                                break;
                            case 4:
                                Console.Clear();
                                autorService.RemoverAutor();
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
                        Console.WriteLine("1 - Adicionar categoria");
                        Console.WriteLine("2 - Consultar categorias");
                        Console.WriteLine("3 - Atualizar categoria");
                        Console.WriteLine("4 - Remover categoria");
                        Console.WriteLine("0 - Sair"); 

                        int opcao5 = ValidaNumero();

                        switch (opcao5)
                        {
                            case 0:
                                Environment.Exit(0);
                                break;
                            case 1:
                                Console.Clear();
                                categoriaService.AdicionarCategoria();
                                break;
                            case 2:
                                Console.Clear();
                                categoriaService.ConsultarCategorias();
                                break;
                            case 3:
                                Console.Clear();
                                categoriaService.AtualizarCategoria();
                                break;
                            case 4:
                                Console.Clear();
                                categoriaService.RemoverCategoria();
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Opção inválida.");
                                Console.ReadKey();
                                continue;
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        continue;
                }
            }
        }

        private static int ValidaNumero()
        {
            if (!int.TryParse(Console.ReadLine(), out int opcao3))
            {
                Console.WriteLine("Opção inválida. [Enter]");
                Console.ReadKey();
            }

            return opcao3;
        }
    }
}
