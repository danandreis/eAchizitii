using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class AdresaLivrareVM
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="Adresa este obligatorie")]
        public string Adresa { get; set; }

        //legatura cu Sucursala
        [Display(Name ="Lista sucursale")]
        public int SucursalaId { get; set; }

    }
}
