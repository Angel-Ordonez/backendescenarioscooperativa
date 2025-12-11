using Cooperativa.App.Domain.Model.Caja;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooperativa.App.Domain.Data.Config
{
    public class TransaccionConfig : IEntityTypeConfiguration<Transaccion>
    {
        public void Configure(EntityTypeBuilder<Transaccion> modelBuilder)
        {

            //// Relación N:1 (CuentaBancaria → Transacciones)
            //modelBuilder.HasOne(p => p.CuentaBancaria)
            //    .WithMany() // No es bidireccional (CuentaBancaria no necesita una lista de Préstamos)
            //    .HasForeignKey(p => p.CuentaBancariaId)
            //    .OnDelete(DeleteBehavior.Restrict); // Evita que se eliminen préstamos al borrar una cuenta


            modelBuilder.Property(c => c.Monto).HasColumnType("decimal(9,2)");


        }
    }
}
