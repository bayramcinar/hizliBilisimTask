using System;
using System.ComponentModel.DataAnnotations;

public class InvoiceLineDto
{
    public int InvoiceLineId { get; set; } 

    [Required(ErrorMessage = "Fatura ID zorunludur.")]
    public int InvoiceId { get; set; }

    [Required(ErrorMessage = "Ürün adı boş olamaz.")]
    [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
    public string ItemName { get; set; }

    [Required(ErrorMessage = "Adet bilgisi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "en az 1 adet olmalıdır.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Fiyat bilgisi zorunludur.")]
    [Range(0, double.MaxValue, ErrorMessage = "Fiyat sıfır veya daha büyük olmalıdır.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Kullanıcı ID zorunludur.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Kayıt tarihi zorunludur.")]
    public DateTime RecordDate { get; set; }
}
