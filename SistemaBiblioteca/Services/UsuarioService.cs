using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.Context;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace SistemaBiblioteca.Services
{
    internal class UsuarioService()
    {
        public void CriarUsuario()
        {
            while (true)
            {
                using (var db = new AppDbContext()) {

                    Console.WriteLine("Email:");
                    string email = Console.ReadLine()?.Trim();

                    Console.WriteLine("Senha (até 10 caracteres):");
                    string senha = Console.ReadLine()?.Trim();

                    if (!EmailValido(email))
                    {
                        Console.WriteLine("Email inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (db.Usuarios.Any(u => u.Email == email))
                    {
                        Console.WriteLine("Usuário já existe. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (senha.Length > 10 || string.IsNullOrWhiteSpace(senha))
                    {
                        Console.WriteLine("Senha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("Nome: ");
                    string nome = Console.ReadLine()?.Trim();

                    if (nome.Length > 120 || string.IsNullOrWhiteSpace(nome))
                    {
                        Console.WriteLine("Nome inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("CPF (apenas números): ");
                    string cpf = Console.ReadLine()?.Trim();

                    if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                    {
                        Console.WriteLine("CPF inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    var usuario = new Usuario()
                    {
                        Nome = nome,
                        CPF = cpf,
                        Email = email,
                        Senha = senha
                    };

                    try
                    {
                        db.Add(usuario);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao inserir no banco de dados.");
                        Console.WriteLine(ex.ToString());
                        Console.ReadKey();
                        break;
                    }

                    Console.WriteLine("Usuario adicionado com sucesso! [Enter]");
                    Console.ReadKey();
                    break;
                }
            }
        }
        public Usuario? Login()
        {
            while (true)
            {
                using (var db = new AppDbContext())
                {

                    Console.WriteLine("Email:");
                    string email = Console.ReadLine()?.Trim();

                    Console.WriteLine("Senha:");
                    string senha = Console.ReadLine()?.Trim();

                    if (!EmailValido(email))
                    {
                        Console.WriteLine("Email inválido. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (senha.Length > 10 || string.IsNullOrWhiteSpace(senha))
                    {
                        Console.WriteLine("Senha inválida. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (email == "acesso@admin.com" && senha == "3009121514")
                    {
                        // pula pro menu admin
                    }

                    var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email);

                    if (usuario == null)
                    {
                        Console.WriteLine("Usuário não existente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    if (usuario.Senha == senha)
                    {
                        Console.WriteLine($"Seja bem-vindo(a) {usuario.Nome}! [Enter]");
                        Console.ReadKey();
                        return usuario;
                    }
                    else
                    {
                        Console.WriteLine("Senha incorreta. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                }
            }
        }
        public void ConsultarTodosUsuarios()
        {
            using (var db = new AppDbContext())
            {
                var usuarios = db.Usuarios
                    .AsNoTracking()
                    .Include(u => u.Emprestimos)
                    .ThenInclude(e => e.Livro)
                    .ToList();

                if (usuarios.Count == 0)
                {
                    Console.WriteLine("Não existem usuários registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                    ExibeUsuarios(usuarios);
            }
        }
        public void ConsultarUsuariosComEmprestimo()
        {
            using (var db = new AppDbContext())
            {
                var usuarios = db.Usuarios
                    .AsNoTracking()
                    .Include(u => u.Emprestimos)
                    .ThenInclude(e => e.Livro)
                    .Where(u => u.Emprestimos
                    .Any(e => !e.Devolvido))
                    .ToList();

                if (usuarios.Count == 0)
                {
                    Console.WriteLine("Não existem usuários devendo livros registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                    ExibeUsuarios(usuarios);
            }
        }
        public void AtualizarUsuario()
        {

        }
        public void ExcluirUsuario()
        {

        }



        private static void ExibeUsuarios(List<Usuario> usuarios)
        {
            foreach (var usuario in usuarios)
            {
                var livrosEmprestados = usuario.Emprestimos.Where(e => !e.Devolvido).ToList();

                string senhaOculta = new string('*', usuario.Senha.Length);
                string cpfOculto = "*********" + usuario.CPF.Substring(8);

                Console.WriteLine($"ID: {usuario.Id}");
                Console.WriteLine($"Nome: {usuario.Nome}");
                Console.WriteLine($"Email: {usuario.Email}");
                Console.WriteLine($"Senha: {senhaOculta}");
                Console.WriteLine($"CPF: {cpfOculto}");

                if (livrosEmprestados.Count > 0)
                {
                    Console.WriteLine("Livros Emprestados: " + string.Join(", ", livrosEmprestados
                        .Select(le => le.Livro.Titulo)));
                }
                Console.WriteLine($"--------------------------------------------------");
            }
            Console.WriteLine("\n[Enter]");
            Console.ReadKey();
        }

        private static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, padrao);
        }
    }
}
