using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Mvc;

namespace eAchizitii.Data.Services
{
    public interface IComenziService
    {

        Task<List<Comanda>> GetAllAsync(); //Se extrag toate comenzile din baza de date
        Task<List<Comanda>> GetAllAsync(int idSucursala); //Se extrag toate comenzile emise de o anumita sucursala
        Task<List<Comanda>> GetOrdersByAquisitorAsync(string idResposabilAchizitii); //Se extrag toate comenzile preluate de un anumit resposanbil
        Task<List<Comanda>> GetAllUnallocatedOrdersAsync(); //Se extrag toate comenzile nealocate
        Task<List<Comanda>> GetAllUnapprovedOrdersAsync(); //Se extrag comenzile trimise si neaprobate
        Task<Comanda> GetByIdAsync(int id); //Se extrage o comanda pe baza ID-ului
        Task<List<Produs>> GetProduse(); //Se extrag produsele aferente comenzii
        Task<Produs> GetProdusById(int id);
        Task<RolComanda> GetIdRolComanda(string nume);
        Task<List<AdresaLivrare>> GetAdreseLivrare(int idSucursala);
        Task<List<AppUser>> GetAngajati(int idSucursala);
        Task<int> GetIdStatusByNameAsync(string status); //Se extrage ID-ul statusului pe baza denumirii statusului
        Task<int> AddAsync(ComandaVM comandaVM);
        Task UpdateAsync(int id, ComandaVM comandaVM);
        Task ChangeStatusAsync(int id, string status);
        Task SchimbaStareComanda(int id);

    }
}
