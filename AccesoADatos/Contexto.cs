using LogicaDeNegocios.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoADatos
{
    public class Contexto: DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<EventoEconomico> EventosEconomicos { get; set; }

        public Contexto(DbContextOptions options): base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(usuario => usuario.HasKey(u => u.Id));
            modelBuilder.Entity<EventoEconomico>(evento => evento.HasKey(e => e.Id));

            base.OnModelCreating(modelBuilder);
        }
    }
}
