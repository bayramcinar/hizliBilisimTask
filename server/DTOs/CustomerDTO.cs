using System.ComponentModel.DataAnnotations;

public class CustomerDTO
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Vergi numarası alanı zorunludur.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Vergi numarası 20 karakterden fazla 5 karakterden az olamaz")]
    public string TaxNumber { get; set; }

    [Required(ErrorMessage = "İsim alanı zorunludur.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "İsim 100 karakterden fazla 5 karakterden az olamaz")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Adres alanı zorunludur.")]
    [StringLength(200, ErrorMessage = "Adres 200 karakterden fazla olamaz")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Email alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email formatı geçersiz.")]
    public string EMail { get; set; }


    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime RecordDate { get; set; }
}