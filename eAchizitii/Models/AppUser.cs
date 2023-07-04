using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Models
{
    public class AppUser:IdentityUser 
    {
        [Display(Name ="Nume utilizator")]
        [Required]
        public string Nume { get; set; }

        //Relatia cu Sucursala
        public int? SucursalaId { get; set; }
        public Sucursala? sucursala { get; set; }

        //Relatia cu tabela angajati-comenzi
        public List<Angajat_comanda>? angajati_comenzi { get; set; }

        //Relatia cu InfoComanda
        public List<InfoComanda>? InfoComanda { get; set; }

        //Relatia cu MesajeComanda
        public List<MesajComanda>? MesajeComanda { get; set; }

    }
}
