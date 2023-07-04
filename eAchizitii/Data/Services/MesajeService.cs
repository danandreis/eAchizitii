using eAchizitii.Models;
using Microsoft.EntityFrameworkCore;

namespace eAchizitii.Data.Services
{
    public class MesajeService : IMesajeService
    {

        private readonly AppDbContext _context;

        public MesajeService(AppDbContext context)
        {

            _context = context; 

        }

        public async Task<MesajComanda> GetMesajByIdAsync(int id)
        {


            var mesaj = await _context.MesajeComanda.FirstOrDefaultAsync(m => m.Id == id);

            return mesaj!;


        }

        //Se xetrag mesajele aferente unei comenzi
        public async Task<List<MesajComanda>> GetMesajePeComandaAsync(int idComanda)
        {

            var mesajeComanda = await _context.MesajeComanda.Include(m=>m.utilizator).Where(m => m.ComandaId == idComanda).
                                OrderByDescending(m=>m.Data).ToListAsync();

            return mesajeComanda;

        }

        public async Task AddAsync(MesajComanda mesajComanda)
        {
            
            if(mesajComanda != null)
            {

                await _context.AddAsync(mesajComanda);

                await _context.SaveChangesAsync();

            }


        }

        public async Task<MesajComanda> UpdateAsync(int id, MesajComanda mesajComanda)
        {

            var mesajBD = await _context.MesajeComanda.FirstOrDefaultAsync(m=>m.Id == id);
            
            if(mesajBD != null)
            {

                mesajBD.Mesaj = mesajComanda.Mesaj;
                mesajBD.Data = DateTime.Now;

                await _context.SaveChangesAsync();

            }

            return mesajBD!;

        }

        public Task DeleteAsync(int id, MesajComanda mesajComanda)
        {
            throw new NotImplementedException();
        }


       
    }
}
