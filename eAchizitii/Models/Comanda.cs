using eAchizitii.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Models
{
    public class Comanda
    {
        public int Id { get; set; }

        [Display(Name = "Data comenzii")]
        public DateTime DataComanda { get; set; }

        public string Observatii { get; set; }

        public int Activ { get; set; } //1 - Comanda Activa, 0 - Comanda inactiva

        [Display(Name ="Valoare estimativa")]
        [Required(ErrorMessage = "Valoarea este obligatorie")]
        [Range(1,1000000,ErrorMessage ="Introduceti o valoare valida. Folositi punctul (.) pentru zecimale!")]
        public decimal Valoare { get; set; }

        [Display(Name ="Sucursala")]
        //Legatura cu Sucursala
        public int? SucursalaId { get; set; }
        public Sucursala? sucursala { get; set; }

        [Display(Name = "Adresa livrare")]
        //Legatura cu Adresele de livrare
        public int? AdresaLivrareId { get; set; }
        public AdresaLivrare? adresaLivrare { get; set; }

        [Display(Name ="Status")]
        public int StatusComandaId { get; set; }
        public StatusComanda statusComanda { get; set; }

        //Legatura cu tabela Produs_comanda
        public List<Produs_comanda>? produse_comenzi { get; set; }

        //Relatia cu tabela angajati-comenzi
        public List<Angajat_comanda>? angajati_comenzi { get; set; }

        //Legatura cu InfoComanda
        public List<InfoComanda>? InfoComenzi { get; set; }

        //Legatura cu MesajeComanda
        public List<MesajComanda>? MesajeComanda { get; set; }


    }
}
