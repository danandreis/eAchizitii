namespace eAchizitii.Models
{
    public class Produs_comanda
    {

        public int? ComandaId { get; set; }
        public Comanda? comanda { get; set; }

        public int ProdusId { get; set; }
        public Produs produs { get; set; }

        public int Cantitate { get; set; }


    }
}
