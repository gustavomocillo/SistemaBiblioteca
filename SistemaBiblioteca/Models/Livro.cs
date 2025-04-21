using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Models
{
    internal class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int QuantidadePaginas { get; set; }
        public int? Ano { get; set; }
        public string ISBN13 { get; set; }

        public int IdAutor { get; set; }
        public Autor Autor { get; set; }

        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; }

        public IList<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}
