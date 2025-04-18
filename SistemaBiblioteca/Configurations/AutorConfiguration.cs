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
    internal class AutorConfiguration : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.ToTable("Autores");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Nome).HasMaxLength(120).IsRequired();
            builder.Property(a => a.DataNascimento).HasColumnType("DATE").IsRequired();

            builder.HasMany(a => a.Livros)
                .WithOne(l => l.Autor)
                .HasForeignKey(l => l.IdAutor)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
