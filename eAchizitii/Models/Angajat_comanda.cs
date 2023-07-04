using eAchizitii.Data.Enum;

namespace eAchizitii.Models
{
    public class Angajat_comanda
    {

        public string? angajatId { get; set; }
        public AppUser? angajat { get; set; }

        public int? ComandaId { get; set; }
        public Comanda? comanda { get; set; }

        public int RolComandaId { get; set; }

        public RolComanda? rolComanda { get; set; }


    }
}
