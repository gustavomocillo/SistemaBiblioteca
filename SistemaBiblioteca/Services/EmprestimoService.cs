using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Configurations.SessaoLogin;
using SistemaBiblioteca.Context;
using SistemaBiblioteca.Menus;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Services
{
    internal class EmprestimoService
    {
        public void CriarEmprestimoAdmin()
        {
            while (true)
            {

                using (var db = new AppDbContext())
                {

                    Console.WriteLine("ID do Usuário do empréstimo:");

                    if (!int.TryParse(Console.ReadLine(), out int idUsuario))
                    {
                        Console.WriteLine("ID inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var usuario = db.Usuarios.Find(idUsuario);

                    if (usuario == null)
                    {
                        Console.WriteLine($"Não existe um Usuário com o ID {idUsuario}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("ID do Livro emprestado:");

                    if (!int.TryParse(Console.ReadLine(), out int idLivro))
                    {
                        Console.WriteLine("ID inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var emprestimoExistente = db.Emprestimos
                        .FirstOrDefault(e => e.IdLivro == idLivro && !e.Devolvido);

                    if (emprestimoExistente != null)
                    {
                        Console.WriteLine("Esse livro já está emprestado no momento. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var livro = db.Livros.Find(idLivro);

                    if (livro == null)
                    {
                        Console.WriteLine("Não existe um livro com esse ID. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine($"{usuario.Nome} deseja o livro {livro.Titulo}?\n");
                    Console.WriteLine("1 - Sim");
                    Console.WriteLine("2 - Não");

                    if (!int.TryParse(Console.ReadLine(), out int escolha))
                    {
                        Console.WriteLine("Escolha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (escolha == 1)
                    {
                        Emprestimo emprestimo = new Emprestimo()
                        {
                            IdLivro = idLivro,
                            IdUsuario = idUsuario,
                        };

                        try
                        {
                            db.Add(emprestimo);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao criar empréstimo.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else if (escolha == 2)
                    {
                        Console.WriteLine("[Enter] para voltar.");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Escolha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("\nEmpréstimo concluido. [Enter]");
                    Console.ReadKey();
                    MenuAdmin.Exibir();
                    break;
                }
            }
        }
        public void CriarEmprestimoUsuario(Usuario? usuario)
        {
            while (true)
            {

                using (var db = new AppDbContext())
                {
                    Console.WriteLine("ID do Livro que deseja pegar emprestado:");

                    if (!int.TryParse(Console.ReadLine(), out int idLivro))
                    {
                        Console.WriteLine("ID inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var emprestimoExistente = db.Emprestimos
                        .FirstOrDefault(e => e.IdLivro == idLivro && !e.Devolvido);

                    if (emprestimoExistente != null)
                    {
                        Console.WriteLine("Esse livro já está emprestado no momento. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var livro = db.Livros.Find(idLivro);

                    if (livro == null)
                    {
                        Console.WriteLine("Não existe um livro com esse ID. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine($"{usuario.Nome} deseja o livro {livro.Titulo}?\n");
                    Console.WriteLine("1 - Sim");
                    Console.WriteLine("2 - Não");

                    if (!int.TryParse(Console.ReadLine(), out int escolha))
                    {
                        Console.WriteLine("Escolha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (escolha == 1)
                    {
                        Emprestimo emprestimo = new Emprestimo()
                        {
                            IdLivro = idLivro,
                            IdUsuario = usuario.Id,
                        };

                        try
                        {
                            db.Add(emprestimo);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao criar empréstimo.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else if (escolha == 2)
                    {
                        Console.WriteLine("[Enter] para voltar.");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Escolha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("\nEmpréstimo concluido. [Enter]");
                    Console.ReadKey();
                    MenuUsuario.Exibir(Sessao.Usuario);
                    break;
                }
            }
        }
        public void ConsultarTodosEmprestimos()
        {
            using (var db = new AppDbContext())
            {
                var emprestimos = db.Emprestimos
                    .AsNoTracking()
                    .Include(e => e.Livro)
                    .Include(e => e.Usuario)
                    .ToList();

                if (emprestimos.Count == 0)
                {
                    Console.WriteLine("Não existem empréstimos registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                {
                    ExibirEmprestimos(emprestimos);
                }

                MenuAdmin.Exibir();
            }
        }
        public void ConsultarEmprestimosDoUsuario(Usuario? usuario)
        {
            using (var db = new AppDbContext())
            {
                var emprestimos = db.Emprestimos
                    .AsNoTracking()
                    .Include(e => e.Livro)
                    .Include(e => e.Usuario)
                    .Where(e => e.IdUsuario == usuario.Id)
                    .ToList();

                if (emprestimos.Count == 0)
                {
                    Console.WriteLine("Não existem empréstimos registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                {
                    ExibirEmprestimos(emprestimos);
                }

                if (Sessao.AdminLogado)
                    MenuAdmin.Exibir();
                else
                    MenuUsuario.Exibir(Sessao.Usuario);
            }
        }
        public void RegistrarDevolucao()
        {
            Console.WriteLine("ID do Empréstimo a ser devolvido:");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido. [Enter]");
                Console.ReadKey();
                return;
            }

            using (var db = new AppDbContext())
            {
                var emprestimo = db.Emprestimos.Find(id);

                if (emprestimo == null)
                {
                    Console.WriteLine("Empréstimo não encontrado. [Enter]");
                    Console.ReadKey();
                    return;
                }

                if (emprestimo.Devolvido)
                {
                    Console.WriteLine("Esse empréstimo já foi devolvido. [Enter]");
                    Console.ReadKey();
                    return;
                }

                emprestimo.Devolvido = true;
                db.SaveChanges();

                Console.WriteLine("Devolução registrada com sucesso. [Enter]");
                Console.ReadKey();
                if (Sessao.AdminLogado)
                    MenuAdmin.Exibir();
                else
                    MenuUsuario.Exibir(Sessao.Usuario);
            }
        }




        private static void ExibirEmprestimos(List<Emprestimo> emprestimos)
        {
            Console.Clear();
            foreach (var emprestimo in emprestimos)
            {
                Console.WriteLine($"ID: {emprestimo.Id}");
                Console.WriteLine($"Usuario: {emprestimo.Usuario.Nome}");
                Console.WriteLine($"Livro: {emprestimo.Livro.Titulo}");
                Console.WriteLine($"Retirada: {emprestimo.DataRetirada.ToString("dd-MM-yyyy")}");
                Console.WriteLine($"Devolução: {emprestimo.DataDevolucao.ToString("dd-MM-yyyy")}");
                Console.WriteLine($"Devolvido: {(emprestimo.Devolvido ? "Sim" : "Não")}");
                Console.WriteLine($"---------------------------------------------------------");
            }
            Console.WriteLine("\n[Enter]");
            Console.ReadKey();
        }
    }
}