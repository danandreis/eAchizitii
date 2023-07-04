using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAchizitii.Controllers
{

    [Authorize(Roles ="Admin")]
    public class ProduseController : Controller
    {

        private readonly IProduseService _service;
        private readonly INotyfService _notyf;

        public ProduseController(IProduseService service, INotyfService notyf)
        {
            _service = service;
            _notyf = notyf;

        }

        public async Task<IActionResult> Index()
        {

            var data = await _service.GetAll();
            return View(data);
        }

        //Create new product
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Denumire, Um")] Produs produsNou)
        {

            if (!ModelState.IsValid)
            {

                return View(produsNou);

            }

            _service.Add(produsNou);
            _notyf.Success("Produsul a fost adaugat in baza de date!");
            return RedirectToAction("Index");

        }

        //GET Produse/editare/{id}
        public async Task<IActionResult> Editare(int id)
        {

            var produs = await _service.GetByIdAsync(id);

            if (produs == null)
            {

                _notyf.Error("Eroare editare produs.</br> Nu exista produsul cu Id-ul " + id);
                return RedirectToAction("Index");

            }

            return View(produs);

        }

        [HttpPost]
        public async Task<IActionResult> Editare(int id, Produs produs)
        {

            if (!ModelState.IsValid)
            {
                _notyf.Error("A intervenit o eroare la actualizarea datelor produsului!");
                return View(produs);
            }

            await _service.UpdateAsync(id,produs);
            _notyf.Success("Datele produsul au fost actualizate in baza de date!");
            return RedirectToAction("Index");
        }

        //GET Produse/Stergere/{id}
        public async Task<IActionResult> Sterge(int id)
        {

            var produs = await _service.GetByIdAsync(id);

            if(produs == null)
            {

                _notyf.Error("Eroare stergere produs.</br> Nu exista produsul cu Id-ul " + id);
                return RedirectToAction("Index");
            }
            
            return View(produs);

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmaStergere(int id)
        {

            var produs = await _service.GetByIdAsync(id);

            if (produs == null)
            {

                _notyf.Error("Eroare stergere produs.</br> Nu exista produsul cu Id-ul " + id);
                return RedirectToAction("Index");
            }

           await _service.DeleteAsync(id);

            _notyf.Success("Produsul a fost sters cu succes </br> din baza de date!");
            return RedirectToAction("Index");

        }
    }
}