using eAchizitii.Models;

namespace eAchizitii.Data.Services
{
    public interface IInfoComandaService
    {
        Task<List<InfoComanda>> GetInfoComenziByIdComanda(int idComanda); //Se extrag informatiile solicitare/date despre o comanda

        Task<InfoComanda> GetInfoComandaById(int idInfo); //Se extrage o informatie din baza de date

        Task<bool> AddInfoComandaAsync(InfoComandaCurentaVM infoComandaCurentaVM);

        Task<bool> UpdateInfoComanda(InfoComandaCurentaVM infoComandaCurentaVM);

    }
}
