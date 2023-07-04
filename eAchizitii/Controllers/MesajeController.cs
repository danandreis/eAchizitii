using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Principal;

namespace eAchizitii.Controllers
{

    [Authorize(Roles = "Achizitii,  Management, Director Sucursala")]
    public class MesajeController : Controller
    {

        private readonly IMesajeService _serviceMesaje;
        private readonly IComenziService _serviceComenzi;
        private readonly INotyfService _notyfService;
        private readonly IPrincipal _principal;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private MesajComandaVM mesajComandaVM = new MesajComandaVM();

        public MesajeController(IMesajeService service, UserManager<AppUser> userManger, INotyfService notyfService, 
            IPrincipal principal, IComenziService serviceComenzi, IEmailService emailService)
        {

            _serviceMesaje = service;
            _userManager = userManger;
            _notyfService = notyfService;
            _principal = principal;
            _serviceComenzi = serviceComenzi;
            _emailService = emailService;
        }

        public async Task<List<MesajComanda>> IstoricMesajeComanda(int idComanda)
        {

            var mesajeComanda = await _serviceMesaje.GetMesajePeComandaAsync(idComanda);
            
            List<MesajComanda> listaMesajeComanda = new List<MesajComanda>();

            foreach (var mesaj in mesajeComanda)
            {

                var mesajNou = new MesajComanda()
                {

                    Id = mesaj.Id,
                    Data = mesaj.Data,
                    Mesaj = mesaj.Mesaj,
                    ComandaId = mesaj.ComandaId,
                    utilizatorId = mesaj.utilizatorId,
                    utilizator = await _userManager.FindByNameAsync(mesaj.utilizator!.UserName)

                };

                listaMesajeComanda.Add(mesajNou);

            }

            return listaMesajeComanda;

        }

        public IActionResult TrimiteMesaj(int id) //id=idComanda
        {

            mesajComandaVM.listaMesaje = new List<MesajComanda>();

            mesajComandaVM.listaMesaje = IstoricMesajeComanda(id).Result;
            mesajComandaVM.mesajComanda = new MesajComanda();
            mesajComandaVM.mesajComanda.Data = DateTime.Now;
            mesajComandaVM.mesajComanda.ComandaId = id;

            return View(mesajComandaVM);

        }


        public void NotificareTrimitereMesajComanda(string destinationEmail, int idComanda, DateTime dataComanda)
        {

            var emailToAddress = destinationEmail;
            var emailFromAddress = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Email;
            var emailSubject = "Ati primit un mesaj pentru comanda " + idComanda;

            string mesajMail = "Ati primit un mesaj pentru comanda nr." + idComanda + " din data de " + 
                                dataComanda.ToString("dd.MM.yyyy") +"</br></br> Va rog sa verificati in aplicatie mesajul trimis!" ;

            bool isEmaiSent = _emailService.TrimiteMail(emailToAddress, emailFromAddress, emailSubject, mesajMail);

            if (isEmaiSent)
            {
                _notyfService.Success("A fost trimis mailul catre destinatar!");

            }
            else
            {
                _notyfService.Error("A intervenit o eroare la trimiterea mailului catre destintar");

            }

        }


        [HttpPost]
        public async Task<IActionResult> TrimiteMesaj(MesajComanda mesajNouComanda)
        {

            var utilizator = await _userManager.FindByNameAsync(_principal.Identity!.Name);

            mesajComandaVM.mesajComanda = new MesajComanda()
            {

                Data = mesajNouComanda.Data,
                ComandaId = mesajNouComanda.ComandaId,
                Mesaj = mesajNouComanda.Mesaj,
                utilizatorId = utilizator.Id
            
            };

            mesajComandaVM.listaMesaje = new List<MesajComanda>();

            mesajComandaVM.listaMesaje = IstoricMesajeComanda((int)mesajNouComanda.ComandaId!).Result;

            ModelState["utilizatorId"]!.ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {

                return View(mesajComandaVM);

            }

            await _serviceMesaje.AddAsync(mesajComandaVM.mesajComanda);

            _notyfService.Success("Mesajul a fost transmis cu succes");

            var emailDestinatar = "";
            var comandaDB = await _serviceComenzi.GetByIdAsync((int)mesajNouComanda.ComandaId);
            var dataComanda = comandaDB.DataComanda;

            if (mesajComandaVM.listaMesaje.Count > 0)
            {

                emailDestinatar = mesajComandaVM.listaMesaje[0].utilizator!.Email;

            }
            else
            {
                
                if (comandaDB != null)
                {

                    foreach (var angajat in comandaDB.angajati_comenzi!)
                    {

                        if (angajat.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                        {

                            emailDestinatar = angajat.angajat!.Email;

                        }

                    }

                }

            }

            NotificareTrimitereMesajComanda(emailDestinatar, (int)mesajNouComanda.ComandaId,dataComanda);

            var rolUtilizator  = await _userManager.GetRolesAsync(utilizator);

            //Daca se trimite mesaj de la Management sau Achizitii
            if (rolUtilizator[0].Equals("Management") || rolUtilizator[0].Equals("Achizitii"))
            {

                await _serviceComenzi.ChangeStatusAsync((int)mesajNouComanda.ComandaId,
                    ApplicationConstantValues.Solicitare_info_supl);

                return RedirectToAction("Index", rolUtilizator[0]);

            }

            //Daca se trimite mesaj de la Directorul de Sucursala
        
            //Se setabileste rolul utilizatorului caruia i se raspunde
            var utilizatorRaspuns = await _userManager.FindByNameAsync(mesajComandaVM.listaMesaje[0].utilizator!.UserName);

            var rolUtilizatorRaspuns = await _userManager.GetRolesAsync(utilizatorRaspuns);

            if (rolUtilizatorRaspuns[0].Equals("Management"))
            {

                await _serviceComenzi.ChangeStatusAsync((int)mesajNouComanda.ComandaId,
                        ApplicationConstantValues.Trimisa);

            }
            else
            {
             
                await _serviceComenzi.ChangeStatusAsync((int)mesajNouComanda.ComandaId,
                        ApplicationConstantValues.Preluata);
            
            }

           

            return RedirectToAction("Index", "Comenzi");

        }

        public async Task<IActionResult> Inapoi()
        {

            var utilizator = await _userManager.FindByNameAsync(_principal.Identity!.Name);

            var rolUtilizator = await _userManager.GetRolesAsync(utilizator);

            if (rolUtilizator[0].Equals("Management") || rolUtilizator[0].Equals("Achizitii"))
            {

                return RedirectToAction("Index", rolUtilizator[0]);

            }
            else
            {

                return RedirectToAction("Index", "Comenzi");

            }


        }

        public async Task<IActionResult> Editare (int id)
        {

            var mesaj = await _serviceMesaje.GetMesajByIdAsync(id);

            mesajComandaVM.listaMesaje = new List<MesajComanda>();

            mesajComandaVM.listaMesaje = IstoricMesajeComanda((int)mesaj.ComandaId!).Result;

            mesajComandaVM.mesajComanda = new MesajComanda();
            mesajComandaVM.mesajComanda.Mesaj = mesaj.Mesaj;
            mesajComandaVM.mesajComanda.Data = mesaj.Data;
            mesajComandaVM.mesajComanda.ComandaId = mesaj.ComandaId;
            mesajComandaVM.mesajComanda.Id = mesaj.Id;

            return View("TrimiteMesaj",mesajComandaVM);

        }

        [HttpPost]
        public async Task<IActionResult> ActualizeazaMesaj(int id, MesajComanda mesajComanda)
        {

            var mesajNou = await _serviceMesaje.UpdateAsync(id, mesajComanda);

            if (mesajNou != null)
            {

                _notyfService.Success("Mesajul a fost actualizat cu succes!");

            }
            else
            {

                _notyfService.Error("A intervenit o eroare la actualizarea mesajului");

            }

            return RedirectToAction("TrimiteMesaj",new { id = mesajComanda.ComandaId });


        }

    }
}
