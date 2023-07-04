using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class ResetareParolaVM
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Introduceti o parola")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirmarea parolei este obligatorie")]
        [Display(Name ="Confirmare parola")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage =("Parolele nu corespund"))]
        public string ConfirmPassword { get; set; }

    }
}
