using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAPI.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6, en fazla 100 karakter olmalıdır.")]
        public string Password { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        public ICollection<Invoice> Invoices { get; set; } 
        public ICollection<Customer> Customers { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; } 
    }
}
