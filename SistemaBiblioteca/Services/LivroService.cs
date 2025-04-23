using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.Context;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public void ConsultarTodosLivros()
        {
            using (var db = new AppDbContext())
            {
                var livros = db.Livros
                    .AsNoTracking()
                    .OrderBy(l => l.Titulo)
                    .Include(l => l.Autor)
                    .Include(l => l.Categoria)
                    .Include(l => l.Emprestimos)
                    .ToList();

                if (livros.Count == 0)
                {
                    Console.WriteLine("Não existem livros registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                {
                    ExibirLivros(livros);
                }
            }
        }
        public void ConsultarLivrosEmprestados()
        {
            using (var db = new AppDbContext())
            {
                var livros = db.Livros
                    .AsNoTracking()
                    .OrderBy(l => l.Titulo)
                    .Include(l => l.Autor)
                    .Include(l => l.Categoria)
                    .Include(l => l.Emprestimos)
                    .Where(l => l.Emprestimos.Any(e => !e.Devolvido))
                    .ToList();

                if (livros.Count == 0)
                {
                    Console.WriteLine("Não existem livros emprestados. [Enter]");
                    Console.ReadKey();
                }
                else
                {
                    ExibirLivros(livros);
                }
            }
        }
        public void ConsultarLivrosDisponiveis()
        {
            using (var db = new AppDbContext())
            {
                var livros = db.Livros
                    .AsNoTracking()
                    .OrderBy(l => l.Titulo)
                    .Include(l => l.Autor)
                    .Include(l => l.Categoria)
                    .Include(l => l.Emprestimos)
                    .Where(l => l.Emprestimos.All(e => e.Devolvido))
                    .ToList();

                if (livros.Count == 0)
                {
                    Console.WriteLine("Não existem livros disponíveis. [Enter]");
                    Console.ReadKey();
                }
                else
                {
                    ExibirLivros(livros);
                }
            }
        }
        public void BuscarPorId()
        {
            while (true)
            {
                Console.WriteLine("ID do Livro que deseja buscar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var livro = db.Livros
                        .AsNoTracking()
                        .Include(l => l.Autor)
                        .Include(l => l.Categoria)
                        .Include(l => l.Emprestimos)
                        .Where(l => l.Id == id)
                        .ToList();

                    if (livro.Count == 0)
                    {
                        Console.WriteLine($"Não existe um livro com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        ExibirLivros(livro);
                    }
                }
            }
        }

        public void BuscarPorNome()
        {
            while (true)
            {
                Console.WriteLine("Título do livro que deseja buscar:");
                string titulo = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(titulo))
                {
                    Console.WriteLine("O título é obrigatório. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var livro = db.Livros
                        .AsNoTracking()
                        .Include(l => l.Autor)
                        .Include(l => l.Categoria)
                        .Include(l => l.Emprestimos)
                        .Where(l => l.Titulo.Contains(titulo))
                        .ToList();

                    if (livro.Count == 0)
                    {
                        Console.WriteLine($"Não existe um livro com o título {titulo}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        ExibirLivros(livro);
                    }
                }
            }
        }
        public void BuscarPorCategoria()
        {
            while (true)
            {
                Console.WriteLine("ID da Categoria que deseja buscar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var livro = db.Livros
                        .AsNoTracking()
                        .Include(l => l.Autor)
                        .Include(l => l.Categoria)
                        .Include(l => l.Emprestimos)
                        .Where(l => l.IdCategoria == id)
                        .ToList();

                    var categoria = db.Categorias.Find(id);
                    if (categoria == null)
                    {
                        Console.WriteLine($"Não existe categoria com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (livro.Count == 0)
                    {
                        Console.WriteLine($"Não existe um livro com a Categoria {categoria.Nome}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        ExibirLivros(livro);
                    }
                }
            }
        }
        public void AtualizarLivro()
        {
            while (true)
            {
                Console.WriteLine("ID do Livro que deseja atualizar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var livro = db.Livros.Find(id);

                    if (livro == null)
                    {
                        Console.WriteLine("Não existe um livro com esse ID. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Novo título do livro:");
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

                        try
                        {
                            livro.Titulo = titulo;
                            livro.Ano = ano;
                            livro.QuantidadePaginas = quantidadePaginas;
                            livro.ISBN13 = isbn;
                            livro.IdAutor = idAutor;
                            livro.Autor = autor;
                            livro.IdCategoria = idCategoria;
                            livro.Categoria = categoria;

                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao atualizar no banco de dados.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            break;
                        }

                        Console.WriteLine($"{titulo} foi atualizado com sucesso! [Enter]");
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }
        public void RemoverLivro()
        {
            while (true)
            {
                Console.WriteLine("ID do Livro que deseja remover:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var livro = db.Livros.Find(id);

                    if (livro == null)
                    {
                        Console.WriteLine($"Não existe um livro com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    Console.WriteLine($"Deseja remover este livro: {livro.Titulo}?\n");
                    Console.WriteLine("1 - Sim");
                    Console.WriteLine("2 - Não");

                    if (!int.TryParse(Console.ReadLine(), out int remover))
                    {
                        Console.WriteLine("Resposta inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (remover == 1)
                    {
                        try
                        {
                            db.Livros.Remove(livro);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao remover livro.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else if (remover == 2)
                    {
                        Console.WriteLine("Tente novamente.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Resposta Inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("\nLivro removido. [Enter]");
                    Console.ReadKey();
                    break;
                }
            }
        }


        private static void ExibirLivros(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                bool estaEmprestado = livro.Emprestimos.Any(e => !e.Devolvido);

                Console.WriteLine($"ID: {livro.Id}");
                Console.WriteLine($"Título: {livro.Titulo}");
                Console.WriteLine($"Autor: {livro.Autor.Nome}");
                Console.WriteLine($"Categoria: {livro.Categoria.Nome}");
                Console.WriteLine($"Ano: {livro.Ano}");
                Console.WriteLine($"Páginas: {livro.QuantidadePaginas}");
                Console.WriteLine($"ISBN13: {livro.ISBN13}");
                Console.WriteLine($"Status: {(estaEmprestado ? "Emprestado" : "Disponível")}");
                Console.WriteLine("----------------------------------------------------------");
            }
            Console.WriteLine("\n[Enter]");
            Console.ReadKey();
        }
    }
}