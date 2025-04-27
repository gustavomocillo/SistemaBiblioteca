using SistemaBiblioteca.Context;
using SistemaBiblioteca.Menus;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Services
{
    internal class AutorService
    {
        public void AdicionarAutor()
        {
            while (true)
            {
                Console.WriteLine("Nome do autor:");
                string nomeAutor = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(nomeAutor) || nomeAutor.Length > 120)
                {
                    Console.WriteLine("Nome inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Data de nascimento (dd-mm-aa):");

                if (!DateOnly.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly data))
                {
                    Console.WriteLine("Data inválida. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Autor autor = new Autor()
                {
                    Nome = nomeAutor,
                    DataNascimento = data
                };

                using (var db = new AppDbContext())
                {
                    var consultaAutor = db.Autores.FirstOrDefault(a => a.Nome.ToLower() == nomeAutor.ToLower());

                    if (consultaAutor == null)
                    {
                        try
                        {
                            db.Add(autor);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao inserir no banco de dados.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Esse autor já existe. Tente novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                }

                Console.WriteLine("Autor adicionado com sucesso! [Enter]");
                Console.ReadKey();
                MenuAdmin.Exibir();
                break;
            }
        }
        public void ConsultarAutores()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var autores = db.Autores.OrderBy(a => a.Nome).ToList();

                    if (autores.Count == 0)
                    {
                        Console.WriteLine("Nenhum autor cadastrado.");
                    }
                    else
                    {
                        Console.Clear();
                        foreach (var autor in autores)
                        {
                            Console.WriteLine($"ID: {autor.Id}");
                            Console.WriteLine($"Nome: {autor.Nome}");
                            Console.WriteLine($"Nascimento: {autor.DataNascimento.ToString("dd-MM-yyyy")}");
                            Console.WriteLine("------------------------------------------------------");
                        }
                    }

                    Console.WriteLine("\nConsulta finalizada. [Enter]");
                    Console.ReadKey();
                    MenuAdmin.Exibir();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar autores.");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
        public void AtualizarAutor()
        {
            while (true)
            {
                Console.WriteLine("ID do Autor que deseja atualizar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var autor = db.Autores.Find(id);

                    if (autor == null)
                    {
                        Console.WriteLine($"Não existe um autor com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Novo nome do autor:");
                        string nomeAutor = Console.ReadLine()?.Trim();

                        Console.WriteLine("Data de nascimento (dd-mm-aa):");

                        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly data))
                        {
                            Console.WriteLine("Data inválida. [Enter]");
                            Console.ReadKey();
                            continue;
                        }

                        var consultaAutor = db.Autores.FirstOrDefault(a => a.Nome.ToLower() == nomeAutor.ToLower());

                        if (consultaAutor == null)
                        {
                            if (string.IsNullOrWhiteSpace(nomeAutor) || nomeAutor.Length > 120)
                            {
                                Console.WriteLine("Nome inválido. [Enter]");
                                Console.ReadKey();
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    autor.Nome = nomeAutor;
                                    autor.DataNascimento = data;
                                    db.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Erro ao atualizar autor.");
                                    Console.WriteLine(ex.ToString());
                                    Console.ReadKey();
                                    continue;
                                }
                            }

                            Console.WriteLine("\nAutor atualizado. [Enter]");
                            Console.ReadKey();
                            MenuAdmin.Exibir();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Esse autor já existe. Tente novamente. [Enter]");
                            Console.ReadKey();
                            continue;
                        }
                    }
                }
            }
        }
        public void RemoverAutor()
        {
            while (true)
            {
                Console.WriteLine("ID do Autor que deseja remover:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var autor = db.Autores.Find(id);

                    if (autor == null)
                    {
                        Console.WriteLine($"Não existe um autor com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        try
                        {
                            db.Autores.Remove(autor);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao remover autor.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("\nAutor removido. [Enter]");
                        Console.ReadKey();
                        MenuAdmin.Exibir();
                        break;
                    }
                }
            }
        }
    }
}
