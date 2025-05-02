using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceAPI.Models
{
    [Table("InvoiceLine")]
    public class InvoiceLine
    {
        [Key]
        public int InvoiceLineId { get; set; }
        
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(255)]
        public string ItemName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Miktar 0'dan küçük olamaz.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Fiyat 0'dan küçük olamaz.")]
        public decimal Price { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime RecordDate { get; set; }

        public Invoice Invoice { get; set; }
        public User User { get; set; }
    }
}
