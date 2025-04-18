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
    internal class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nome).HasMaxLength(60).IsRequired();

            builder.HasMany(c => c.Livros)
                .WithOne(l => l.Categoria)
                .HasForeignKey(l => l.IdCategoria)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
