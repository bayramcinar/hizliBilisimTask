using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceAPI.Data
{
    public static class DbInitializer
    {
        public static void Seed(DataContext context)
        {
            context.Database.Migrate();

            if (context.Users.Any() || context.Customers.Any() || context.Invoices.Any() || context.InvoiceLines.Any())
                return;

            var adminUser = new User
            {
                UserName = "admin",
                Password = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9", // admin123 şifre ilk giriş için dummy veriler eklemek için kullandım
                RecordDate = DateTime.Now
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
            int defaultUserId = adminUser.UserId;

            var customers = new List<Customer>
            {
                new Customer { TaxNumber = "1234567890", Title = "ABC İnşaat A.Ş.", Address = "İstanbul", EMail = "abc@insaat.com", UserId = defaultUserId, RecordDate = DateTime.Now },
                new Customer { TaxNumber = "9876543210", Title = "XYZ Bilişim Ltd.", Address = "Ankara", EMail = "xyz@bilisim.com", UserId = defaultUserId, RecordDate = DateTime.Now },
                new Customer { TaxNumber = "4567891230", Title = "Köroğlu Gıda", Address = "Bursa", EMail = "info@koroglu.com", UserId = defaultUserId, RecordDate = DateTime.Now },
                new Customer { TaxNumber = "7891234560", Title = "Beta Tekstil", Address = "İzmir", EMail = "beta@tekstil.com", UserId = defaultUserId, RecordDate = DateTime.Now },
                new Customer { TaxNumber = "3216549870", Title = "Delta Enerji", Address = "Adana", EMail = "delta@enerji.com", UserId = defaultUserId, RecordDate = DateTime.Now }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            var invoices = new List<Invoice>
            {
                new Invoice { CustomerId = customers[0].CustomerId, InvoiceNumber = "FTR001", InvoiceDate = DateTime.Today, TotalAmount = 1500, UserId = defaultUserId, RecordDate = DateTime.Now },
                new Invoice { CustomerId = customers[1].CustomerId, InvoiceNumber = "FTR002", InvoiceDate = DateTime.Today, TotalAmount = 2300, UserId = defaultUserId, RecordDate = DateTime.Now },
                new Invoice { CustomerId = customers[2].CustomerId, InvoiceNumber = "FTR003", InvoiceDate = DateTime.Today, TotalAmount = 3100, UserId = defaultUserId, RecordDate = DateTime.Now },
                new Invoice { CustomerId = customers[3].CustomerId, InvoiceNumber = "FTR004", InvoiceDate = DateTime.Today, TotalAmount = 1200, UserId = defaultUserId, RecordDate = DateTime.Now },
                new Invoice { CustomerId = customers[4].CustomerId, InvoiceNumber = "FTR005", InvoiceDate = DateTime.Today, TotalAmount = 800,  UserId = defaultUserId, RecordDate = DateTime.Now }
            };

            context.Invoices.AddRange(invoices);
            context.SaveChanges();

            var invoiceLines = new List<InvoiceLine>
            {
                new InvoiceLine { InvoiceId = invoices[0].InvoiceId, ItemName = "Çimento", Quantity = 10, Price = 100, UserId = defaultUserId, RecordDate = DateTime.Now },
                new InvoiceLine { InvoiceId = invoices[1].InvoiceId, ItemName = "Bilgisayar", Quantity = 2, Price = 1150, UserId = defaultUserId, RecordDate = DateTime.Now },
                new InvoiceLine { InvoiceId = invoices[2].InvoiceId, ItemName = "Zeytinyağı", Quantity = 20, Price = 155, UserId = defaultUserId, RecordDate = DateTime.Now },
                new InvoiceLine { InvoiceId = invoices[3].InvoiceId, ItemName = "Tişört", Quantity = 30, Price = 40, UserId = defaultUserId, RecordDate = DateTime.Now },
                new InvoiceLine { InvoiceId = invoices[4].InvoiceId, ItemName = "Güneş Paneli", Quantity = 4, Price = 200, UserId = defaultUserId, RecordDate = DateTime.Now }
            };

            context.InvoiceLines.AddRange(invoiceLines);
            context.SaveChanges();
        }
    }
}
