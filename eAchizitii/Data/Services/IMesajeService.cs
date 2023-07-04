using eAchizitii.Models;

namespace eAchizitii.Data.Services
{
    public interface IMesajeService
    {

        Task<List<MesajComanda>> GetMesajePeComandaAsync(int idComanda);

        Task<MesajComanda> GetMesajByIdAsync(int id);

        Task AddAsync(MesajComanda mesajComanda);

        Task<MesajComanda> UpdateAsync(int id, MesajComanda mesajComanda);

        Task DeleteAsync(int id, MesajComanda mesajComanda);

    }
}
