using Microsoft.EntityFrameworkCore;
using quiz_fatemehabolhasani.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: @"Server=LAPTOP-LCL3BM4M\SQLEXPRESS;Database=Bank;Integrated Security=true;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);
   
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SourceCard)
                .WithMany(c => c.TransactionsAsSource)
                .HasForeignKey(t => t.SourceCardNumber)
                .HasPrincipalKey(c => c.CardNumber)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DestinationCard)
                .WithMany(c => c.TransactionsAsDestination)
                .HasForeignKey(t => t.DestinationCardNumber)
                .HasPrincipalKey(c => c.CardNumber)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
