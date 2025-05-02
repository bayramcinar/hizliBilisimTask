using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAPI.Models
{
    [Table("Invoice")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public int TotalAmount { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        public User User { get; set; }

        public Customer Customer { get; set; }

        public ICollection<InvoiceLine> InvoiceLines { get; set; } 
    }
}
