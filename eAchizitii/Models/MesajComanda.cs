namespace eAchizitii.Models
{
    public class MesajComanda
    {

        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Mesaj { get; set; }

        //Legatura cu comenzile
        public int? ComandaId { get; set; }

        public Comanda? comanda { get; set; }

        public string utilizatorId { get; set; }

        public AppUser? utilizator { get; set; }
    }
}
