namespace eAchizitii.Models
{
    public class AdresaLivrare
    {

        public int Id { get; set; }
        public string? Adresa { get; set; }

        //legatura cu Sucursala
        public int? SucursalaId { get; set; }
        public Sucursala? sucursala { get; set; }

        //Legatura cu comenzile
        public List<Comanda>? comenzi { get; set; }

    }
}
