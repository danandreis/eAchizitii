using eAchizitii.Models;

namespace eAchizitii.Data.Services
{
    public interface IProduseService
    {

        Task<IEnumerable<Produs>> GetAll();
        Task<Produs> GetByIdAsync(int id);
        void Add(Produs produs);
        Task<Produs> UpdateAsync(int id, Produs produsNou);
        Task DeleteAsync(int id);
    }
}
