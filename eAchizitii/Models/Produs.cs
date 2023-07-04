using eAchizitii.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Models
{
    public class Produs
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="Denumirea produsului este obligatorie")]
        [StringLength(250,MinimumLength =3,ErrorMessage ="Intoduceti o denumire valida")]
        public string Denumire { get; set; }

        [Display(Name ="U.M.")]
        [Required(ErrorMessage ="Selectati o U.M. valida")]
        public ListaUM Um { get; set; }

        //Relatia cu tabela de produs_comanda
        public List<Produs_comanda>? produse_comenzi { get; set; }

    }
}
