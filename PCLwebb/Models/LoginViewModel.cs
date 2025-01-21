using System.ComponentModel.DataAnnotations;

namespace PCLwebb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv en epost.")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vänligen skriv lösenord.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
