using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse müşteri silinmesin

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Müşteri silinirse fatura silinmesin

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse fatura silinmesin

            modelBuilder.Entity<InvoiceLine>()
                .HasOne(il => il.Invoice)
                .WithMany(i => i.InvoiceLines)
                .HasForeignKey(il => il.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade); // Fatura silinince satırlar silinsin

            modelBuilder.Entity<InvoiceLine>()
                .HasOne(il => il.User)
                .WithMany(u => u.InvoiceLines)
                .HasForeignKey(il => il.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse satırlar silinmesin
        }

    }
}
