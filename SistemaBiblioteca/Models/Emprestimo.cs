using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Models
{
    internal class Emprestimo
    {
        public Emprestimo()
        {
            DataRetirada = DateOnly.FromDateTime(DateTime.Now);
            DataDevolucao = DataRetirada.AddDays(14);
            Devolvido = false;
        }

        public int Id { get; set; }
        public DateOnly DataRetirada { get; set; }
        public DateOnly DataDevolucao { get; set; }
        public bool Devolvido { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public int IdLivro { get; set; }
        public Livro Livro { get; set; }
    }
}
