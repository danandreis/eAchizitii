using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;

namespace eAchizitii.Controllers
{
    [Authorize (Roles = "Achizitii")]
    public class AchizitiiController : Controller
    {

        private readonly IComenziService _service;
        private readonly INotyfService _notyfService;
        private readonly IPrincipal _principal;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ComandaVM comandaVM = new ComandaVM();
        
        public AchizitiiController(IComenziService service, INotyfService notyfService, UserManager<AppUser> userManager, 
                IPrincipal principal, IEmailService emailService)
        {

            _service = service;
            _notyfService = notyfService;
            _userManager = userManager;
            _principal = principal;
            _emailService = emailService;
        
        }

        public async Task<IActionResult> Index()
        {

            var lista_comenzi = await _service.GetAllUnallocatedOrdersAsync();

            return View(lista_comenzi);
        }
        public async Task<IActionResult> ComenziAlocate()
        {

            var responsabilAchizitii = await _userManager.FindByNameAsync(_principal.Identity!.Name);

            var lista_comenzi = await _service.GetOrdersByAquisitorAsync(responsabilAchizitii.Id);

            return View(lista_comenzi);
        }

        public async Task<IActionResult> DetaliiComanda(int id)
        {

            var comanda = await _service.GetByIdAsync(id);

            if(comanda == null)
            {

                _notyfService.Error("Comanda cu ID-ul : " + id + " nu exista in baza de date!");

                return RedirectToAction("Index");
            }

            return View(comanda);

        }

        //Schimbare status comanda in Procesul de achizitie
        public async Task<IActionResult> SchimbareStatusComandaAchizitii(int id, string status)
        {

            var comanda = await _service.GetByIdAsync(id);

            if (comanda == null)
            {

                _notyfService.Error("Comanda cu ID-ul : " + id + " nu exista in baza de date!");

                return RedirectToAction("Index");

            }

            await _service.ChangeStatusAsync(id, status);

            if (status.Equals(ApplicationConstantValues.Preluata))
            {
                comandaVM.Id = comanda.Id;
                comandaVM.ResponsabilAchizitiiId = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Id;

                foreach (var item in comanda.angajati_comenzi!)
                {
                    if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                    {

                        comandaVM.EmitentId = item.angajatId;

                    }

                    comandaVM.PersoanaContactId = item.angajatId;

                }

                await _service.UpdateAsync((int)comandaVM.Id, comandaVM);


                _notyfService.Success("Preluarea comenzii a fost inregistrata in baza de date!");

            }

            if (status.Equals(ApplicationConstantValues.Trimisa))
            {
                comandaVM.Id = comanda.Id;
                
                foreach (var item in comanda.angajati_comenzi!)
                {
                    if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                    {

                        comandaVM.EmitentId = item.angajatId;

                    }

                    if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Responstabil_achizitii))
                    {

                        comandaVM.ResponsabilAchizitiiId = null;

                    }

                    comandaVM.PersoanaContactId = item.angajatId;

                }

                await _service.UpdateAsync((int)comandaVM.Id, comandaVM);


                _notyfService.Success("Au fost actualizate informatiile despre persoanele implicate in comanda ");

            }


            return RedirectToAction("Index");
        }

        private string GenerareMesajPreluareComanda(int idComanda, DateTime dataComanda, AppUser angajat)
        {

            string mesajMail = "Comanda nr." + idComanda + " din data de " + dataComanda.ToString("dd.MM.yyyy") +
                "  a fost preluata de catre un responsabil achizitii . </br></br>" +
                "Informatii suplimentare : </br> </br> ";

            mesajMail += "<table>";

            mesajMail += "<tr><td>Data preluare : </td><td><b> " + DateTime.Now.ToString("dd.MM.yyyy hh:mm tt") + " </b></td></tr>";

            mesajMail += "<tr><td>Resposanbil achizitii : </td><td><b> " + angajat.Nume + " </b></td></tr>";

            mesajMail += "<tr><td>Telefon : </td><td><b> " + angajat.PhoneNumber + " </b></td></tr>";

            mesajMail += "</table>";

            return mesajMail;

        }

        public async Task<bool> NotificarePreluareComanda(ComandaVM comandaVM)
        {

            var emailFromAddress = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Email;
            var emailToAddress = _userManager.FindByIdAsync(comandaVM.EmitentId).Result.Email;
            var emailSubject = "Comanda a fost preluata de un responsabil";
            var angajat = await _userManager.FindByNameAsync(User.Identity!.Name);
            var corpMesaj = GenerareMesajPreluareComanda((int)comandaVM.Id!, comandaVM.DataComanda, angajat);

            bool isEmaiSent = _emailService.TrimiteMail(emailToAddress, emailFromAddress, emailSubject, corpMesaj);

            if (!isEmaiSent)
            {
                return false;
            }

            return true;

        }

        public async Task<IActionResult> PreluareComanda(int id)
        {

            var comanda = await _service.GetByIdAsync(id);

            if (comanda == null)
            {


                _notyfService.Error("Comanda cu ID-ul : " + id + " nu exista in baza de date!");

                return RedirectToAction("Index");

            }

            await _service.ChangeStatusAsync(id, ApplicationConstantValues.Preluata);


            comandaVM.Id = comanda.Id;
            comandaVM.DataComanda = comanda.DataComanda;
            comandaVM.ResponsabilAchizitiiId = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Id;

            foreach (var item in comanda.angajati_comenzi!)
            {
                if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                {

                    comandaVM.EmitentId = item.angajatId;

                }


                comandaVM.PersoanaContactId = item.angajatId;

            }

            await _service.UpdateAsync((int)comandaVM.Id, comandaVM);

            _notyfService.Success("Preluarea comenzii a fost inregistrata in baza de date!");


            if (NotificarePreluareComanda(comandaVM).Result)
            {

                _notyfService.Success("A fost trimis mailul catre emitentul comenzii!");

            }
            else
            {
                _notyfService.Error("A intervenit o eroare la trimiterea mailului");

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RenuntareComanda(int id)
        {

            var comanda = await _service.GetByIdAsync(id);

            if (comanda == null)
            {

                _notyfService.Error("Comanda cu ID-ul : " + id + " nu exista in baza de date!");

                return RedirectToAction("Index");

            }

            await _service.ChangeStatusAsync(id, ApplicationConstantValues.Aprobata);

            comandaVM.Id = comanda.Id;
            foreach (var item in comanda.angajati_comenzi!)
            {
                if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                {

                    comandaVM.EmitentId = item.angajatId;

                }

                if (item.rolComanda!.Rol!.Equals(ApplicationConstantValues.Responstabil_achizitii))
                {

                    comandaVM.ResponsabilAchizitiiId = null;

                }

                comandaVM.PersoanaContactId = item.angajatId;

            }

            await _service.UpdateAsync((int)comandaVM.Id, comandaVM);

            _notyfService.Success("Renuntarea la comanda a fost inregistrata in baza de date!");

            return RedirectToAction("ComenziAlocate");
        }
    }
}
