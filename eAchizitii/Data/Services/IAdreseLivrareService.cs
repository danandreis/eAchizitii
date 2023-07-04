using eAchizitii.Data.ViewModels;
using eAchizitii.Models;

namespace eAchizitii.Data.Services
{
    public interface IAdreseLivrareService
    {

        Task<IEnumerable<AdresaLivrare>> GetAllAsync(int idSucursala);

        Task<AdresaLivrare> GetByIdAsync(int id);

        string GetNumeSucursala(int idSucursala);

        Task AddAsync(AdresaLivrareVM adresaLivrare);

        Task UpdateAsync(int id, AdresaLivrareVM adresaLivrare);

        Task DeleteAsync(int id);
    }
}
