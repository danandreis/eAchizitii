namespace eAchizitii.Models
{
    public class RolComanda
    {
        public int Id { get; set; }

        public string? Rol { get; set; }

        public List<Angajat_comanda>? angajati_comenzi { get; set; }
    }
}
