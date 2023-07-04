using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data.Services;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace eAchizitii.Controllers
{

    [Authorize]
    public class AdreseLivrareController : Controller
    {

        private readonly IAdreseLivrareService _services;
        private readonly INotyfService _notyfService;
        private readonly UserManager<AppUser> _userManager;
        private readonly int idSucursala;
        private readonly string sucursala;

        private readonly IPrincipal _principal;

        public AdreseLivrareController(IAdreseLivrareService services, INotyfService notyfService, IPrincipal principal,
            UserManager<AppUser> userManager)
        {
            _services = services;
            _notyfService = notyfService;
            _principal = principal;
            _userManager = userManager;

            idSucursala = (int)_userManager.FindByNameAsync(_principal.Identity!.Name).Result.SucursalaId!;
            sucursala = _services.GetNumeSucursala(idSucursala);

        }

        public async Task<IActionResult> Index()
        {

            var listaAdreese = await _services.GetAllAsync(idSucursala);
            ViewBag.sucursala = sucursala;

            return View(listaAdreese);

        }

        //GET - AdreseLIvrare/Create
        public async Task<IActionResult> Create()
        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult>Create(AdresaLivrareVM adresaLivrare)
        {

            if (!ModelState.IsValid)
            {

               return View();

            }

            adresaLivrare.SucursalaId = idSucursala;

            await _services.AddAsync(adresaLivrare);

            _notyfService.Success("Adresa a fost adaugate cu succes in baza de date!");

            return RedirectToAction("Index");

        }

        //GET - AdreseLivrare/Editare/{id}
        public async Task<IActionResult> Editare(int id)
        {

            var adresaLivrare = await _services.GetByIdAsync(id);

            if(adresaLivrare == null)
            {

                _notyfService.Error("Adresa de livrare cu ID-ul " + id + " nu exista!");

                return RedirectToAction("Index");

            }

            var adresalivrareVM = new AdresaLivrareVM()
            {

                Id = adresaLivrare.Id,
                Adresa = adresaLivrare.Adresa,
                SucursalaId = idSucursala

            };

            ViewBag.Sucursala = sucursala;

            return View(adresalivrareVM);
        }

        [HttpPost]
        public async Task<IActionResult> Editare(int id, AdresaLivrareVM adresaLivrareNoua)
        {

            if (!ModelState.IsValid)
            {

              
                ViewBag.Sucursala = sucursala;

                return View(adresaLivrareNoua);

            }

            adresaLivrareNoua.SucursalaId = idSucursala;

            await _services.UpdateAsync(id, adresaLivrareNoua);

            _notyfService.Success("Adresa a fost actualizate cu succes in nbaza de date!");

            return RedirectToAction("Index");
        }

        //GET - AdreseLivrare/Stergere/{id}
        public async Task<IActionResult> Stergere(int id)
        {

            var adresaDB = await _services.GetByIdAsync(id);

            if(adresaDB == null)
            {

                _notyfService.Error("Adresa cu ID-ul " + id + " nu exista in baza de date!");
                return RedirectToAction("Index");

            }

            var adresalivare = new AdresaLivrareVM()
            {

                Id = adresaDB.Id,
                Adresa = adresaDB.Adresa!,
                SucursalaId = idSucursala

            };

           ViewBag.Sucursala = sucursala;


            return View(adresalivare);

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmaStergere(int id)
        {

            var adresaDb = await _services.GetByIdAsync(id);

            if(adresaDb == null)
            {

                _notyfService.Error("Adresa cu ID-ul " + id + " nu exista in baza de date!");
                return RedirectToAction("Index");

            }

            await _services.DeleteAsync(id);

            _notyfService.Success("Adresa a fost stearsa din baza de date!");

            return RedirectToAction("Index");
        }

    }
}
