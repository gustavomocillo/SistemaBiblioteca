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
    internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nome).HasMaxLength(120).IsRequired();
            builder.Property(u => u.Senha).HasMaxLength(10).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.CPF).HasColumnType("CHAR(11)").IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.CPF).IsUnique();
        }
    }
}
