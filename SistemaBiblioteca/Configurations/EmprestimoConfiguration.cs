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
    internal class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
    {
        public void Configure(EntityTypeBuilder<Emprestimo> builder)
        {
            builder.ToTable("Emprestimos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.DataRetirada).HasColumnType("DATE").IsRequired();
            builder.Property(e => e.DataDevolucao).HasColumnType("DATE").IsRequired();
            builder.Property(e => e.Devolvido).IsRequired();

            builder.HasOne(e => e.Usuario)
                .WithMany(u => u.Emprestimos)
                .HasForeignKey(e => e.IdUsuario);

            builder.HasOne(e => e.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.IdLivro);
        }
    }
}
