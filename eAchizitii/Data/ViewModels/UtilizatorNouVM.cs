using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class UtilizatorNouVM
    {

        public string? Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [Display(Name = "Nume utilizator")]
        [RegularExpression(@"^[A-Z][a-z]{2,}([ ][A-Z][a-z]{2,})+$",
            ErrorMessage = "Introduceti un nume valid!")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Adresa de e-mail este obligatorie")]
        [Display(Name = "Adresa e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Introduceti o parola")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Numarul de telefon este obligatoriu")]
        [RegularExpression(@"^[0]\d{9}$",
           ErrorMessage = "Introduceti un numar de telefon valid")]
        public string Telefon { get; set; }

        [Display(Name = "Sucursala")]
        //Legatura cu Sucursala
        public int SucursalaId { get; set; }

        [Display(Name ="Rol")]
        public string Rol { get; set; }
    }
}
