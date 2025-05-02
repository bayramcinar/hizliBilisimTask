using System.ComponentModel.DataAnnotations;
public class UserDto
{
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; }

}