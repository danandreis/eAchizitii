using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class UtilizatorVM
    {

        public string Id{ get; set; }

        public string Nume { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int Activ { get; set; }

        public string Sucursala { get; set; }

        public string Rol { get; set; }

    }
}
