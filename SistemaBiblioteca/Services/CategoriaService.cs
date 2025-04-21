using SistemaBiblioteca.Context;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Services
{
    internal class CategoriaService
    {
        public void AdicionarCategoria()
        {
            while (true)
            {
                Console.WriteLine("Nome da categoria:");
                string nomeCategoria = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(nomeCategoria) || nomeCategoria.Length > 60)
                {
                    Console.WriteLine("Nome inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Categoria categoria = new Categoria()
                {
                    Nome = nomeCategoria,
                };

                using (var db = new AppDbContext())
                {
                    var consultaCategoria = db.Categorias.FirstOrDefault(c => c.Nome.ToLower() == nomeCategoria.ToLower());

                    if (consultaCategoria == null)
                    {
                        try
                        {
                            db.Add(categoria);
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
                        Console.WriteLine("Essa categoria já existe. Tente novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                }

                Console.WriteLine("Categoria adicionada com sucesso! [Enter]");
                Console.ReadKey();
                break;
            }
        }
        public void ConsultarCategorias()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var categorias = db.Categorias.OrderBy(c => c.Id).ToList();

                    if (categorias.Count == 0)
                    {
                        Console.WriteLine("Nenhuma categoria cadastrada.");
                    }
                    else
                    {
                        foreach (var categoria in categorias)
                        {
                            Console.WriteLine($"ID: {categoria.Id}");
                            Console.WriteLine($"Nome: {categoria.Nome}");
                            Console.WriteLine("------------------------------------------------------");
                        }
                    }

                    Console.WriteLine("\nConsulta finalizada. [Enter]");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar categorias.");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
        public void AtualizarCategoria()
        {
            while (true)
            {
                Console.WriteLine("ID da Categoria que deseja atualizar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var categoria = db.Categorias.Find(id);

                    if (categoria == null)
                    {
                        Console.WriteLine($"Não existe uma categoria com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Novo nome da categoria:");
                        string nomeCategoria = Console.ReadLine()?.Trim();

                        var consultaCategoria = db.Categorias.FirstOrDefault(c => c.Nome.ToLower() == nomeCategoria.ToLower());

                        if (consultaCategoria == null)
                        {
                            if (string.IsNullOrWhiteSpace(nomeCategoria) || nomeCategoria.Length > 60)
                            {
                                Console.WriteLine("Nome inválido. [Enter]");
                                Console.ReadKey();
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    categoria.Nome = nomeCategoria;
                                    db.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Erro ao atualizar categoria.");
                                    Console.WriteLine(ex.ToString());
                                    Console.ReadKey();
                                    continue;
                                }
                            }

                            Console.WriteLine("\nCategoria atualizada. [Enter]");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Essa categoria já existe. Tente novamente. [Enter]");
                            Console.ReadKey();
                            continue;
                        }
                    }
                }
            }
        }
        public void RemoverCategoria()
        {
            while (true)
            {
                Console.WriteLine("ID da Categoria que deseja remover:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var categoria = db.Categorias.Find(id);

                    if (categoria == null)
                    {
                        Console.WriteLine($"Não existe uma categoria com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        try
                        {
                            db.Categorias.Remove(categoria);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao remover categoria.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("\nCategoria removida. [Enter]");
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }
    }
}