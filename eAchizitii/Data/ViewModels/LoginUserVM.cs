using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class LoginUserVM
    {

        [Required (ErrorMessage ="Introduceti adresa de e-mail")]
        [Display(Name ="Adresa de e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Introduceti parola")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }
    }
}
