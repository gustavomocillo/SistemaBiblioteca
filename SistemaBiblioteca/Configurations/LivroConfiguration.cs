using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Configurations
{
    internal class LivroConfiguration : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.ToTable("Livros");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Titulo).IsRequired();
            builder.Property(l => l.QuantidadePaginas).HasColumnType("SMALLINT").IsRequired();
            builder.Property(l => l.Ano).HasColumnType("SMALLINT").IsRequired();
            builder.Property(l => l.ISBN13).HasColumnType("CHAR(13)");
        }
    }
}
