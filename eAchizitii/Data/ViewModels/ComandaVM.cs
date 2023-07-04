using eAchizitii.Data.Enum;
using eAchizitii.Models;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class ComandaVM
    {
        public int? Id { get; set; }

        [Display(Name = "Data comenzii")]
        public DateTime DataComanda { get; set; }

        public string? Observatii { get; set; }

        public int Activ{ get; set; }

        [Display(Name = "Valoare estimativa")]
        [Required (ErrorMessage ="Valoarea este obligatorie")]
        [Range(1, 1000000, ErrorMessage = "Introduceti o valoare valida. Folositi punctul (.) pentru zecimale!")]
        public decimal Valoare { get; set; }

        //Legatura cu Sucursala
        public int? SucursalaId { get; set; }

        //Legatura cu Adresele de livrare
        [Display(Name = "Selectati o adresa de livrare")]
        public int AdresaLivrareId { get; set; }

        public int StatusId { get; set; }

        [Display(Name = "Selectati o persoana de contact")]
        //Legatura cu persoana de contact
        public string? PersoanaContactId { get; set; }

        [Display(Name = "Selectati o persoana de contact")]
        //Legatura cu respponsabilul de la achizitii
        public string? ResponsabilAchizitiiId { get; set; }

        public string? EmitentId { get; set; }

        //Dictionar ce contine informatii de tip Cheie = id produs => Valoare = cantitate produs
        public List<ProdusVM>? listaProduse { get; set; }

        public List<Angajat_comanda>? angajati_comanda { get; set; }

    }
}
