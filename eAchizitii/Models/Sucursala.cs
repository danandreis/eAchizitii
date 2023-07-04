using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Models
{
    public class Sucursala
    {

        public int Id { get; set; }

        [Display(Name = "Sucursala")]
        public string Denumire { get; set; }
        public List<AdresaLivrare>? AdreseLivrare { get; set; }
        public List<Comanda>? Comenzi { get; set; }
        public List<AppUser>? appUsers { get; set; }
    }
}
