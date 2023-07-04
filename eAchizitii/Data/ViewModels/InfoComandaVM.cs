using eAchizitii.Models;
using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class InfoComandaVM
    {
       public List<InfoComanda> listaInfoComenzi { get; set; }

        public InfoComandaCurentaVM infoComandaCurentaVM { get; set; }

    }
}
