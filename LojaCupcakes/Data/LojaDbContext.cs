using LojaCupcakes.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LojaCupcakes.Data
{
    public class LojaDbContext : DbContext
    {
        public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options)
        {
        }

        // Mapeia seus Models para as Tabelas do Banco
        public DbSet<Cupcake> Cupcakes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        // Adiciona os que faltavam
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Renomeia as tabelas para o plural (boa prática)
            modelBuilder.Entity<Cupcake>().ToTable("Cupcakes");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Pedido>().ToTable("Pedidos");
            modelBuilder.Entity<ItemPedido>().ToTable("ItensPedido");

            // Mapeamento completo
            modelBuilder.Entity<Administrador>().ToTable("Administradores");
            modelBuilder.Entity<Pagamento>().ToTable("Pagamentos");

            // Configura o valor padrão para o status do pedido
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasDefaultValue("Processando");
        }
    }
}