using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eAchizitii.Data.Services
{
    public class ComenziService : IComenziService
    {
        public AppDbContext _context { get; set; }
        private readonly UserManager<AppUser> _userManager;

        public ComenziService(AppDbContext context, UserManager<AppUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        
        }

        private IQueryable<Comanda> GetOrders()
        {

            var comenzi = _context.Comenzi.Include(c => c.adresaLivrare).Include(c => c.sucursala).
                             Include(c => c.statusComanda).Include(c => c.angajati_comenzi)!.ThenInclude(u => u.angajat).
                             Include(c => c.angajati_comenzi)!.ThenInclude(rc => rc.rolComanda);

            return comenzi;

        }

        public async Task<List<Comanda>> GetAllAsync()
        {

            var comenzi = await GetOrders().ToListAsync();

            return comenzi;

        }

        public async Task<List<Comanda>> GetAllAsync(int idSucursala)
        {

            var comenzi = await GetOrders().Where(c => c.SucursalaId == idSucursala).ToListAsync();

            return comenzi;

        }

        public async Task<Comanda> GetByIdAsync(int id)
        {

            var comanda = await _context.Comenzi.Include(c => c.adresaLivrare).Include(c => c.sucursala).Include(c=>c.MesajeComanda).
                                Include(c => c.statusComanda).Include(c => c.produse_comenzi)!.ThenInclude(c => c.produs).
                                Include(c => c.angajati_comenzi)!.ThenInclude(u => u.angajat).
                                Include(c => c.angajati_comenzi)!.ThenInclude(rc => rc.rolComanda).
                                FirstOrDefaultAsync(c => c.Id == id);

            return comanda!;
        }

        public async Task<List<Comanda>> GetOrdersByAquisitorAsync(string idResposabilAchizitii)
        {

            var comenzi = await GetOrders().Where(c => c.angajati_comenzi!.Any(a => a.angajatId!.Equals(idResposabilAchizitii))).
                                ToListAsync();

            return comenzi;

        }

        //Comenzile aprobate si nealocate
        public async Task<List<Comanda>> GetAllUnallocatedOrdersAsync()
        {

            var statusComanda = await _context.StatusuriComanda.FirstAsync(s => s.Status == ApplicationConstantValues.Aprobata);

            var comenzi = await GetOrders().Where(c => c.StatusComandaId == statusComanda.Id).ToListAsync();

            return comenzi;
        }

        public async Task<List<Comanda>> GetAllUnapprovedOrdersAsync()
        {

            var statusComanda = await _context.StatusuriComanda.FirstAsync(s => s.Status == ApplicationConstantValues.Trimisa);

            var comenzi = await GetOrders().OrderBy(c => c.DataComanda).
                                Where(c => c.StatusComandaId == statusComanda.Id).ToListAsync();

            return comenzi;
        }
        public async Task<RolComanda> GetIdRolComanda(string nume)
        {


            return await _context.RoluriComenzi.FirstOrDefaultAsync(rc => rc.Rol == nume);
        }

        public async Task<int> GetIdStatusByNameAsync(string status)
        {

            var idStatus = await _context.StatusuriComanda.FirstOrDefaultAsync(s => s.Status.Equals(status));

            if (idStatus != null)
            {

                return idStatus.Id;

            }
            else
            {

                return -1;

            }

        }

        //Citeste toate produsele din BD
        public async Task<List<Produs>> GetProduse()
        {

            var lista_produse = await _context.Produse.ToListAsync();

            return lista_produse;

        }

        public async Task<Produs> GetProdusById(int id)
        {

            var produs = await _context.Produse.FirstOrDefaultAsync(p => p.Id == id);

            return produs!;

        }

        public async Task<List<AdresaLivrare>> GetAdreseLivrare(int idSucursala)
        {

            return await _context.AdreseLivrare.Where(adresa => adresa.SucursalaId == idSucursala).ToListAsync();

        }

        public async Task<List<AppUser>> GetAngajati(int idSucursala)
        {

            return await _context.Users.Include(u => u.sucursala).Where(u => u.SucursalaId == idSucursala).ToListAsync();

        }


        public async Task<int> AddAsync(ComandaVM comandaVM)
        {
            
            if(comandaVM != null)
            {

                var comanda = new Comanda()
                {
                    DataComanda = comandaVM.DataComanda,
                    StatusComandaId = comandaVM.StatusId,
                    Observatii = comandaVM.Observatii?? "-",
                    SucursalaId = comandaVM.SucursalaId,
                    Valoare = comandaVM.Valoare,
                    Activ = 1,
                    AdresaLivrareId = comandaVM.AdresaLivrareId,

                };

                await _context.Comenzi.AddAsync(comanda);
                await _context.SaveChangesAsync();

                //Adaugare id-uri comanda si Produse in tabela Produse_comenzi
                foreach (var produs in comandaVM.listaProduse!)
                {

                    var produs_comanda_item = new Produs_comanda()
                    {

                        ComandaId = comanda.Id,
                        ProdusId = produs.Id,
                        Cantitate = produs.Cantitate

                    };

                    await _context.Produse_comenzi.AddAsync(produs_comanda_item);

                }

                await _context.SaveChangesAsync();

                //Adaugare persoane implicate in comanda in tabela Angajati_comenzi

                foreach (var item in comandaVM.angajati_comanda!)
                {

                    var persoane_comanda = new Angajat_comanda()
                    {

                        angajatId = item.angajatId,
                        ComandaId = comanda.Id,
                        RolComandaId =item.RolComandaId

                    };

                    await _context.Angajati_comenzi.AddAsync(persoane_comanda);

                }

                await _context.SaveChangesAsync();

                return comanda.Id;
                
            }

            return -1;

        }

       


        public async Task UpdateAsync(int id, ComandaVM comandaVM)
        {

            var comandaDb = await _context.Comenzi.FirstOrDefaultAsync(c => c.Id == id);

            if (comandaDb != null)
            {

                if (comandaVM.Observatii != null && comandaVM.AdresaLivrareId > 0 && comandaVM.Valoare > 0)
                {

                    comandaDb.Observatii = comandaVM.Observatii;
                    comandaDb.AdresaLivrareId = comandaVM.AdresaLivrareId;
                    comandaDb.Valoare = comandaVM.Valoare;

                    await _context.SaveChangesAsync();

                }

                if (comandaVM.listaProduse != null)
                {

                    await ActualizareProduseComenziBD(comandaDb.Id, comandaVM);

                }

                await ActualizarePersoaneComandaDB(comandaDb.Id, comandaVM);


            }

        }

        public async Task ChangeStatusAsync(int id, string status)
        {

           var comanda = await _context.Comenzi.FirstOrDefaultAsync(c => c.Id == id);
            
            if (comanda != null)
            {

                comanda.StatusComandaId = _context.StatusuriComanda.FirstOrDefaultAsync(s=>s.Status.Equals(status)).Result!.Id;

            }

            await _context.SaveChangesAsync(); 
            
        }

        //Schimbare stare din Activa in Inactiva si invers
        public async Task SchimbaStareComanda(int id)
        {

            var comanda = await _context.Comenzi.FirstOrDefaultAsync(c => c.Id == id);

            if (comanda != null)
            {

                if (comanda.Activ == 0)
                {

                    comanda.DataComanda = DateTime.Now;

                }

                comanda.Activ = (comanda.Activ == 1)? 0 : 1;


            }

           await _context.SaveChangesAsync();

        }

       
        private async Task ActualizareProduseComenziBD(int idComanda, ComandaVM comandaVM)
        {

            //Stergem produsele deja existente din comanda modificata
            var listaProduseDB = _context.Produse_comenzi.Where(pc => pc.ComandaId == idComanda).ToList();
            _context.Produse_comenzi.RemoveRange(listaProduseDB);


            //adaugam produsele dupa actualizare comenzii (noi si/sau existente
            foreach (var produs in comandaVM.listaProduse!)
            {

                var produs_comanda_item = new Produs_comanda()
                {

                    ComandaId = idComanda,
                    ProdusId = produs.Id,
                    Cantitate = produs.Cantitate

                };

                await _context.Produse_comenzi.AddAsync(produs_comanda_item);

            }

            await _context.SaveChangesAsync();

        }

        public async Task ActualizarePersoaneComandaDB(int idComanda, ComandaVM comandaVM)
        {

            //Extragem persoana de contact aferenta comenzii
            var persoane_comanda = await _context.Angajati_comenzi.Where(p => p.ComandaId == idComanda).ToListAsync();

            if (persoane_comanda != null)
            {

                _context.Angajati_comenzi.RemoveRange(persoane_comanda);

                var angajat_comenzi = new Angajat_comanda()
                {

                    angajatId = comandaVM.EmitentId,
                    ComandaId = idComanda,
                    RolComandaId = _context.RoluriComenzi.FirstOrDefaultAsync(r => r.Rol!.Equals(ApplicationConstantValues.Emitent)).Result!.Id

                };

                await _context.Angajati_comenzi.AddAsync(angajat_comenzi);

                angajat_comenzi = new Angajat_comanda()
                {

                    angajatId = comandaVM.PersoanaContactId,
                    ComandaId = idComanda,
                    RolComandaId = _context.RoluriComenzi.FirstOrDefaultAsync(r => r.Rol!.Equals(ApplicationConstantValues.Persoana_contact)).Result!.Id

                };

                await _context.Angajati_comenzi.AddAsync(angajat_comenzi);

                if (comandaVM.ResponsabilAchizitiiId != null)
                {
                    angajat_comenzi = new Angajat_comanda()
                    {

                        angajatId = comandaVM.ResponsabilAchizitiiId,
                        ComandaId = idComanda,
                        RolComandaId = _context.RoluriComenzi.FirstOrDefaultAsync(r => r.Rol!.Equals(ApplicationConstantValues.Responstabil_achizitii)).Result!.Id

                    };

                    await _context.Angajati_comenzi.AddAsync(angajat_comenzi);
                }

                await _context.SaveChangesAsync();

            }
        }

      
    }
}
