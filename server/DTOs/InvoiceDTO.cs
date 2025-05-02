using System;
using System.ComponentModel.DataAnnotations;

public class InvoiceDto
{
    public int InvoiceId { get; set; } 
    
    [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Fatura numarası boş olamaz.")]
    [StringLength(50, ErrorMessage = "Fatura numarası en fazla 50 karakter olabilir.")]
    public string InvoiceNumber { get; set; }

    [Required(ErrorMessage = "Fatura tarihi zorunludur.")]
    public DateTime InvoiceDate { get; set; }

    [Required(ErrorMessage = "Toplam tutar boş olamaz.")]
    [Range(0, int.MaxValue, ErrorMessage = "Toplam tutar negatif olamaz.")]
    public int TotalAmount { get; set; }

    [Required(ErrorMessage = "Kullanıcı bilgisi zorunludur.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Kayıt tarihi zorunludur.")]
    public DateTime RecordDate { get; set; }
}
