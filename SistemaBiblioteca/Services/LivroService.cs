using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.Context;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Services
{
    internal class LivroService
    {
        public void AdicionarLivro()
        {
            while (true)
            {
                Console.WriteLine("Título do livro:");
                string titulo = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    Console.WriteLine("O título é obrigatório. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Ano de publicação (digite 0 se não souber):");
                int? ano = null;

                if (!int.TryParse(Console.ReadLine(), out int anoConvertido))
                {
                    Console.WriteLine("Insira um ano válido. [Enter]");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    ano = anoConvertido;
                }

                if (ano > DateTime.Now.Year)
                {
                    Console.WriteLine("Insira um ano válido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                if (ano == 0)
                    ano = null;

                Console.WriteLine("Quantidade de páginas:");

                if (!int.TryParse(Console.ReadLine(), out int quantidadePaginas))
                {
                    Console.WriteLine("Insira uma quantidade válida. [Enter]");
                    Console.ReadKey();
                    continue;
                }
                else if (quantidadePaginas == 0)
                {
                    Console.WriteLine("A quantidade de páginas é obrigatória. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("ISBN13:");
                string isbn = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(isbn))
                {
                    isbn = null;
                }

                if (isbn.Length != 13 && isbn != null)
                {
                    Console.WriteLine("Insira um ISBN13 válido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("ID do autor:");
                if (!int.TryParse(Console.ReadLine(), out int idAutor))
                {
                    Console.WriteLine("Insira um ID válido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var autor = db.Autores.FirstOrDefault(a => a.Id == idAutor);

                    if (autor == null)
                    {
                        Console.WriteLine("Esse autor não existe, insira um ID válido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("ID da categoria:");
                    if (!int.TryParse(Console.ReadLine(), out int idCategoria))
                    {
                        Console.WriteLine("Insira um ID válido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var categoria = db.Categorias.FirstOrDefault(a => a.Id == idCategoria);

                    if (categoria == null)
                    {
                        Console.WriteLine("Essa categoria não existe, insira um ID válido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Livro livro = new Livro()
                    {
                        Titulo = titulo,
                        Ano = ano,
                        QuantidadePaginas = quantidadePaginas,
                        ISBN13 = isbn,
                        IdAutor = idAutor,
                        Autor = autor,
                        IdCategoria = idCategoria,
                        Categoria = categoria
                    };

                    try
                    {
                        db.Add(livro);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao inserir no banco de dados.");
                        Console.WriteLine(ex.ToString());
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Livro adicionado com sucesso! [Enter]");
                    Console.ReadKey();
                    break;
                }
            }
        }
    }
}