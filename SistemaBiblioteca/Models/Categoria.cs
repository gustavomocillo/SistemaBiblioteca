using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Models
{
    internal class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        IList<Livro> Livros { get; set; } = new List<Livro>();
    }
}
