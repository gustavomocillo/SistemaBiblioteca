using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaBiblioteca.Configurations.SessaoLogin;
using SistemaBiblioteca.Context;
using SistemaBiblioteca.Menus;
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
    internal class UsuarioService
    {
        public void CriarUsuario()
        {
            while (true)
            {
                using (var db = new AppDbContext())
                {

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

                    Console.WriteLine("Usuario criado com sucesso! [Enter]");
                    Console.ReadKey();
                    if (Sessao.AdminLogado)
                        MenuAdmin.Exibir();
                    else
                    {
                        Sessao.Usuario = usuario;
                        MenuUsuario.Exibir(Sessao.Usuario);
                    }
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
                        Sessao.AdminLogado = true;
                        Console.Clear();
                        MenuAdmin.Exibir();
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
                        Sessao.Usuario = usuario;
                        Console.WriteLine($"Seja bem-vindo(a) {usuario.Nome}! [Enter]");
                        Console.ReadKey();
                        MenuUsuario.Exibir(usuario);
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
                    .OrderBy(u => u.Nome)
                    .ToList();

                if (usuarios.Count == 0)
                {
                    Console.WriteLine("Não existem usuários registrados. [Enter]");
                    Console.ReadKey();
                }
                else
                    ExibeUsuarios(usuarios);

                MenuAdmin.Exibir();
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

                MenuAdmin.Exibir();
            }
        }
        public void AtualizarUsuario()
        {
            while (true)
            {
                Console.WriteLine("ID do Usuário que deseja atualizar:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var usuario = db.Usuarios.Find(id);

                    if (usuario == null)
                    {
                        Console.WriteLine($"Não existe um Usuário com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("Novo email:");
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

                    try
                    {
                        usuario.Nome = nome;
                        usuario.Email = email;
                        usuario.Senha = senha;
                        usuario.CPF = cpf;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao atualizar usuário.");
                        Console.WriteLine(ex.ToString());
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("\nUsuário atualizado. [Enter]");
                    Console.ReadKey();
                    MenuAdmin.Exibir();
                    break;
                }
            }
        }
        public void ExcluirUsuario()
        {
            while (true)
            {
                Console.WriteLine("ID do Usuário que deseja remover:");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("ID inválido. [Enter]");
                    Console.ReadKey();
                    continue;
                }

                using (var db = new AppDbContext())
                {
                    var usuario = db.Usuarios.Find(id);

                    if (usuario == null)
                    {
                        Console.WriteLine($"Não existe um Usuário com o ID {id}. Tente Novamente. [Enter]");
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        try
                        {
                            db.Usuarios.Remove(usuario);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erro ao remover Usuário.");
                            Console.WriteLine(ex.ToString());
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("\nUsuário removido. [Enter]");
                        Console.ReadKey();
                        MenuAdmin.Exibir();
                        break;
                    }
                }
            }
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
                else
                {
                    Console.WriteLine("Livros Emprestados: Não");
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
