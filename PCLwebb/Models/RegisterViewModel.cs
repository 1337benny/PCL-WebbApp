using System.ComponentModel.DataAnnotations;

namespace PCLwebb.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv en epost.")]
        [StringLength(255)]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Vänligen ange en giltig epost.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vänligen skriv lösenord.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Vänlingen bekräfta lösenordet")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekrafta losenordet")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt förnamn.")]
        [StringLength(50)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt efternamn.")]
        [StringLength(50)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt födelsedatum.")]
        //[RegularExpression("^\\d{4}-\\d{2}-\\d{2}$\r\n", ErrorMessage = "Vänligen fyll i ditt födelsedatum.")]
        public DateOnly BirthDay { get; set; }
    }
}
