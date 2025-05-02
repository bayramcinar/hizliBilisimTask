using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
    [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 ve en fazla 100 karakter olmalıdır.")]
    public string Password { get; set; }
}
