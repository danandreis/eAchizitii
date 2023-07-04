using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace eAchizitii.Data.Services
{
    public class InfoComandaService :IInfoComandaService
    {

        public AppDbContext _context { get; set; }
        private readonly IWebHostEnvironment _environment;

        public InfoComandaService(AppDbContext context, IWebHostEnvironment environment)
        {

            _context = context;
            _environment = environment;
        }

        //Se extrag toate informatiile aferente unei comenzi
        public async Task<List<InfoComanda>> GetInfoComenziByIdComanda(int idComanda)
        {

            var lista_info_comanda = await _context.InfoComenzi.Include(ic => ic.utilizator).
                                    Include(ic => ic.fisiere).Where(info => info.ComandaId == idComanda).
                                    OrderByDescending(info => info.Data).ToListAsync();

            return lista_info_comanda;

        }

        //Se extrage o informatie din baza de date 
        public async Task<InfoComanda> GetInfoComandaById(int idInfo)
        {

            var info_comanda = await _context.InfoComenzi.Include(ic => ic.utilizator).Include(ic => ic.comanda).
                                    Include(ic => ic.fisiere).FirstOrDefaultAsync(info => info.Id == idInfo);

            return info_comanda!;

        }

        public async Task<bool> AddInfoComandaAsync(InfoComandaCurentaVM infoComandaCurentaVM)
        {

            if (infoComandaCurentaVM != null)
            {
                //Salvare informatii "stare" comanda
                var infoComanda = new InfoComanda()
                {

                    Data = infoComandaCurentaVM.Data,
                    Descriere = infoComandaCurentaVM.Descriere,
                    ComandaId = infoComandaCurentaVM.ComandaId,
                    utilizatorId = infoComandaCurentaVM.utilizatorId

                };

                await _context.InfoComenzi.AddAsync(infoComanda);

                var nrInregInformatii = await _context.SaveChangesAsync();

                //Salvare fisiere aferente informatiilor salvate mai sus
                var nrFisiereInregistrate = 0;

                if (infoComandaCurentaVM.listaFisiereDB != null)
                {

                    foreach (KeyValuePair<string,string> keyValuePair in infoComandaCurentaVM.listaFisiereDB)
                    {

                        var f = new Fisier()
                        {

                            numeFisierDisc = keyValuePair.Key, //Fisiere generate cu Guid()
                            numeFisierBazaDate = keyValuePair.Value, //Numele original al fisierelor de pe disx
                            InfoComandaId = infoComanda.Id,
                            caleFisier = "./Fisiere"  //Path.Combine(_environment.WebRootPath, "Fisiere")

                    };

                        await _context.Fisiere.AddAsync(f);

                    }

                    nrFisiereInregistrate = await _context.SaveChangesAsync();

                }


                if (nrInregInformatii != 1 && nrFisiereInregistrate == 0)
                {

                    return false;
                }

                return true;

            }

            return false;
        }

        public async Task<bool> UpdateInfoComanda(InfoComandaCurentaVM infoComandaCurentaVM)
        {

            var infoComandaDB = await _context.InfoComenzi.FirstOrDefaultAsync(info => info.Id == infoComandaCurentaVM.Id);

            if (infoComandaDB != null)
            {

                //Actualizare descriere informatie comanda din tabela InfoComenzi
                infoComandaDB.Descriere = infoComandaCurentaVM.Descriere;
                var result = await _context.SaveChangesAsync();

                //Se actualizeaza informatiile in tabela Fisiere

                //1. Se sterg din baza de date fisierele sterse din comanda
                var listaFisiereComanda = _context.Fisiere.Where(f => f.InfoComandaId == infoComandaCurentaVM.Id).ToList();

                foreach (KeyValuePair<string,string> keyValuePair in infoComandaCurentaVM.listaFisiereDB)
                {

                    if (keyValuePair.Value == null)
                    {

                         _context.Fisiere.Remove(listaFisiereComanda.Find(f => f.numeFisierDisc == keyValuePair.Key)!);

                    }

                }

                if(infoComandaCurentaVM.listaFisiereUpload != null)
                {

                    foreach (var fisier in infoComandaCurentaVM.listaFisiereUpload)
                    {

                        var cheie = infoComandaCurentaVM.listaFisiereDB.FirstOrDefault(f => f.Value == fisier.FileName).Key;
                        
                        var f = new Fisier()
                        {

                            numeFisierDisc = cheie, //Fisiere generate cu Guid()
                            numeFisierBazaDate = infoComandaCurentaVM.listaFisiereDB[cheie], //Numele original al fisierelor de pe disx
                            InfoComandaId = (int)infoComandaCurentaVM.Id!,
                            caleFisier = "./Fisiere"   //Path.Combine(_environment.WebRootPath, "Fisiere")

                        };

                        await _context.Fisiere.AddAsync(f);
                    }

                }

                await _context.SaveChangesAsync();

                return true;

            }

            return false;
        }
    }
}
