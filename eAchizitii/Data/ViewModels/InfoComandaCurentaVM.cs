namespace eAchizitii.Models
{
    public class InfoComandaCurentaVM
    {

        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Descriere { get; set; }

        //Legatura cu comenzile
        public int? ComandaId { get; set; }

        public Comanda? comanda { get; set; }

        public string utilizatorId { get; set; }

        public AppUser? utilizator { get; set; }

        public List<IFormFile> listaFisiereUpload { get; set; }

        //Dictionar cu cheie = numeFisierSalvat (numele fiiserului de pe disc) => valoare = numeFisierOriginal (numele original al fisierului)
        public Dictionary<string,string> listaFisiereDB { get; set; } 

    }
}
