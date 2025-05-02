
using System.ComponentModel.DataAnnotations;
public class InvoiceResponseDto
{
    [Required]
    public List<InvoiceDto> Invoices { get; set; } = new();

    [Required]
    public int TotalCount { get; set; }
}