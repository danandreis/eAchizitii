using eAchizitii.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class ProdusVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Denumirea produsului este obligatorie")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Intoduceti o denumire valida")]
        public string Denumire { get; set; }

        [Display(Name = "U.M.")]
        [Required(ErrorMessage = "Selectati o U.M. valida")]
        public ListaUM Um { get; set; }

        [Range(1,9999)]
        public int Cantitate { get; set; }

        public bool Selectat { get; set; }

    }
}
