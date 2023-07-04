using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace eAchizitii.Controllers
{

    [Authorize(Roles = "Management")]
    public class ManagementController : Controller
    {

        private readonly IComenziService _serviceComenzi;
        private readonly IPrincipal _principal;
        private readonly INotyfService _notyfService;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public ManagementController(IComenziService serviceComenzi, UserManager<AppUser> userManager, 
                IPrincipal principal, INotyfService notyfService, IEmailService emailService)    
        {

            _serviceComenzi = serviceComenzi;
            _userManager = userManager;
            _principal = principal;
            _notyfService = notyfService;
            _emailService = emailService;

        }

        public async Task<IActionResult> Index()
        {

            var lista_comenzi_trimise_aprobare = await _serviceComenzi.GetAllUnapprovedOrdersAsync();

            return View(lista_comenzi_trimise_aprobare);
        }

        public string GetEmailEmitentComanda(List<Angajat_comanda> listaAngajatiComanda)
        {

            string emailEmitent = "";

            foreach (var angajat in listaAngajatiComanda!)
            {

                if (angajat.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                {

                    emailEmitent = _userManager.FindByIdAsync(angajat.angajatId).Result.Email;

                }

            }

            return emailEmitent;

        }

        public void NotificareAprobareComanda(Comanda comandaBD)
        {

            var emailToAddress = GetEmailEmitentComanda(comandaBD.angajati_comenzi!);
            var emailFromAddress = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Email;
            var emailSubject = "Comanda a fost aprobata";
            string mesajMail = "Comanda nr." + comandaBD.Id + " din data de " + comandaBD.DataComanda.ToString("dd.MM.yyyy") +
               "  a fost aprobata de un reprezentant al departamentului de Management ";

            bool isEmaiSent = _emailService.TrimiteMail(emailToAddress, emailFromAddress, emailSubject, mesajMail);

            if (isEmaiSent)
            {
                _notyfService.Success("A fost trimis mailul catre emitentul comenzii!");

            }
            else
            {
                _notyfService.Error("A intervenit o eroare la trimiterea mailului");

            }

        }

        public async Task<IActionResult> Aproba(int id) //id = id comanda
        {

            //Schimbare status comanda din Trimisa in Aprobata

            await _serviceComenzi.ChangeStatusAsync(id, ApplicationConstantValues.Aprobata);

            _notyfService.Success("Comanda a fost aprobata");

            var comandaBD = await _serviceComenzi.GetByIdAsync(id);

            NotificareAprobareComanda(comandaBD);

            return RedirectToAction("Index");

        }


    }
}
