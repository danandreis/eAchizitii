using eAchizitii.Models;
using Microsoft.EntityFrameworkCore;

namespace eAchizitii.Data.Services
{
    public class ProduseService : IProduseService
    {

        private readonly AppDbContext _context;
        
        public ProduseService(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Produs produs)
        {

            _context.Produse.Add(produs);
            _context.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {

            var produs = await _context.Produse.FirstOrDefaultAsync(p => p.Id == id);

            _context.Produse.Remove(produs!);

            await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<Produs>> GetAll()
        {

            var lista_produse = await _context.Produse.OrderBy(p=>p.Denumire).ToListAsync();
            return lista_produse;

        }

        public async Task<Produs> GetByIdAsync(int id)
        {
            var produs = await _context.Produse.FirstOrDefaultAsync(p => p.Id == id);

            return produs!;
        }

        public async Task<Produs> UpdateAsync(int id, Produs produs)
        {
            _context.Update(produs);
            await _context.SaveChangesAsync();

            return produs;
        }
    }
}
