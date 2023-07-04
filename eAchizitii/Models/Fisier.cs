namespace eAchizitii.Models
{
    public class Fisier
    {

        public int Id { get; set; }

        public string numeFisierDisc { get; set; }
        public string numeFisierBazaDate { get; set; }

        public string caleFisier { get; set; }

        public int InfoComandaId { get; set; }

        public InfoComanda infoComanda { get; set; }

    }
}
