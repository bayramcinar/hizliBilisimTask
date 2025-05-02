using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InvoiceAPI.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string TaxNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string EMail { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public DateTime RecordDate { get; set; }
        public User User { get; set; }
        public ICollection<Invoice> Invoices { get; set; } 
    }
}
