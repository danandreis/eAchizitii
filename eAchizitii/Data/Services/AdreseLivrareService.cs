using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.EntityFrameworkCore;

namespace eAchizitii.Data.Services
{
    public class AdreseLivrareService : IAdreseLivrareService
    {

        private readonly AppDbContext _context;

        public AdreseLivrareService(AppDbContext context)
        {

            _context = context;

        }

        public async Task<IEnumerable<AdresaLivrare>> GetAllAsync(int IdSucursala)
        {

            IQueryable<AdresaLivrare> listaAdrese = _context.AdreseLivrare.OrderBy(a => a.Adresa).Include(a => a.sucursala);

            if (IdSucursala != 16)
            {

                listaAdrese = listaAdrese.Where(s => s.SucursalaId == IdSucursala);

            }

            return await listaAdrese.ToListAsync();

        }

        public async Task<AdresaLivrare> GetByIdAsync(int id)
        {

            var adresa = await _context.AdreseLivrare.FirstOrDefaultAsync(a => a.Id == id);

            return adresa!;

        }

        public async Task AddAsync(AdresaLivrareVM adresaLivrareNoua)
        {

            var adresaLivrare = new AdresaLivrare()
            {

                Adresa = adresaLivrareNoua.Adresa,
                SucursalaId = adresaLivrareNoua.SucursalaId

            };


            await _context.AdreseLivrare.AddAsync(adresaLivrare);

            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            
            var adresa = await _context.AdreseLivrare.FirstOrDefaultAsync(a=>a.Id==id);

            if(adresa != null)
            {

                _context.AdreseLivrare.Remove(adresa);

                await _context.SaveChangesAsync();

            }


        }

        public async Task UpdateAsync(int id, AdresaLivrareVM adresaLivrareNoua)
        {

            var adresaLivrare = await _context.AdreseLivrare.FirstOrDefaultAsync(a => a.Id == id);

            if(adresaLivrare != null)
            {

                adresaLivrare.Id = adresaLivrareNoua.Id;
                adresaLivrare.Adresa = adresaLivrareNoua.Adresa;
                adresaLivrare.SucursalaId = adresaLivrareNoua.SucursalaId;

                await _context.SaveChangesAsync();

            }

        }

        public string GetNumeSucursala(int idSucursala)
        {

            return   _context.Sucursale.FirstOrDefaultAsync(s=>s.Id == idSucursala).Result!.Denumire;

        }
    }
}
