using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Configurations.SessaoLogin
{
    internal static class Sessao
    {
        public static Usuario? Usuario { get; set; } = null;
        public static bool AdminLogado { get; set; } = false;
    }
}
