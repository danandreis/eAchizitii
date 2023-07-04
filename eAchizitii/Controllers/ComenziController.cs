using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.Services;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;
using System.Text.Json;

namespace eAchizitii.Controllers
{

    [Authorize (Roles = "Admin, User, Management, Director Sucursala")]
    public class ComenziController : Controller
    {

        private readonly IComenziService _service;
        private readonly INotyfService _notyfService;
        private readonly IPrincipal _principal;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly int idSucursala;
        ListaProduseVM produseVM = new ListaProduseVM();
        ComandaVM comandaVM = new ComandaVM();

        public ComenziController(IComenziService service, INotyfService notyfService,
                IPrincipal principal, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _service = service;
            _notyfService = notyfService;
            _principal = principal;
            _userManager = userManager;
            _emailService = emailService;

            idSucursala = (int)_userManager.FindByNameAsync(_principal.Identity!.Name).Result.SucursalaId!;
            
        }

        public async Task<IActionResult> Index()
        {

            await _service.GetOrdersByAquisitorAsync("01889b87-54f2-4c65-9fdb-e27f6d2171c6");

            HttpContext.Session.Remove("Comanda");
            HttpContext.Session.Remove("listaProduseDB");
            HttpContext.Session.Remove("Etapa");

            var lista_comenzi = await _service.GetAllAsync(idSucursala);

            if (_principal.IsInRole("Achizitii") || _principal.IsInRole("Admin"))
            {

                lista_comenzi = await _service.GetAllAsync();

            }

            return View(lista_comenzi);
        }

        //GET - Comenzi/Create
        public async Task<IActionResult> Create()
        {

            produseVM.listaProduseDB = new List<ProdusVM>();

            var etapa = HttpContext.Session.GetString("Etapa");

            if ((etapa == null) || (etapa == "1"))
            {

                if (etapa == null)
                {

                    HttpContext.Session.SetString("Etapa", "1");

                }

                if (HttpContext.Session.GetString("listaProduseDB") != null)
                {

                    produseVM = JsonSerializer.Deserialize<ListaProduseVM>(HttpContext.Session.GetString("listaProduseDB")!)!;

                    return View("SelectieProduse", produseVM);

                }
                else
                {

                    var produseDB = await _service.GetProduse();

                    if (produseDB != null)
                    {

                        foreach (var produs in produseDB)
                        {

                            var produsVM = new ProdusVM()
                            {

                                Id = produs.Id,
                                Denumire = produs.Denumire,
                                Um = produs.Um,
                                Cantitate = 1,
                                Selectat = false

                            };

                            produseVM.listaProduseDB.Add(produsVM);

                        }
                    }
                }

                HttpContext.Session.SetString("listaProduseDB", JsonSerializer.Serialize(produseVM));
                return View("SelectieProduse", produseVM);

            }

            //Deserializam comanda pentru completare cu celelalte date
            var comanda = HttpContext.Session.GetString("Comanda");

            if (comanda != null)
            {

                comandaVM = JsonSerializer.Deserialize<ComandaVM>(comanda)!;

            }

            ViewBag.AdreseLivrare = new SelectList(_service.GetAdreseLivrare(idSucursala).Result, "Id", "Adresa");

            ViewBag.PersoaneContact = new SelectList(_service.GetAngajati(idSucursala).Result, "Id", "Nume");

            if (HttpContext.Session.GetString("Actualizare") != null)
            {

                ViewBag.Actualizare = Int16.Parse(HttpContext.Session.GetString("Actualizare")!);

            }

            return View(comandaVM);


        }

        private string GenerareMesajPreluareComanda(Comanda comandaDB)
        {

            var emitent = _userManager.FindByNameAsync(_principal.Identity!.Name);

            string mesajMail = "Sucursala <b>" + comandaDB.sucursala!.Denumire.ToUpper() + "</b> a trimis o comanda spre aprobare! </br>" +
                "Informatii suplimentare : </br> </br> ";

            mesajMail += "<table>";

            mesajMail += "<tr><td>Data comanda : </td><td><b> " + DateTime.Now.ToString("dd.MM.yyyy hh:mm tt") + " </b></td></tr>";

            mesajMail += "<tr><td>Valoare estimativa : </td><td><b> " + comandaDB.Valoare.ToString("#.#,00") + " LEI</b></td></tr>";

            mesajMail += "<tr><td>Director sucursala : </td><td><b> " + emitent.Result.Nume + " </b></td></tr>";

            mesajMail += "<tr><td>Telefon : </td><td><b> " + emitent.Result.PhoneNumber + " </b></td></tr>";

            mesajMail += "</table>";

            return mesajMail;

        }

        private async Task<List<AppUser>> GetManagementMembersEmailAddresses(Comanda comanda)
        {

            var listaManagement = await _userManager.GetUsersInRoleAsync("Management");

            return listaManagement.ToList();

        }

        private async Task<bool> NotificareTrimitereComanda(int idComanda)
        {

            var comandaDB = await _service.GetByIdAsync(idComanda);

            List<AppUser> userList = GetManagementMembersEmailAddresses(comandaDB).Result;
            var emailFromAddress = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Email;
            var emailSubject = "Ati primit o comanda spre aprobare!";

           /* int indx = 0;

            Parallel.ForEach(userList, user =>
            {

                bool isEmaiSent = _emailService.TrimiteMail(user.Email, emailFromAddress, emailSubject,
                                   GenerareMesajPreluareComanda(comandaDB));

                 if (isEmaiSent)
                {
                
                    indx++;
                
                }

            });*/

            int indx = 0;
            foreach (var user in userList)
            {

                var emailToAddress = user.Email;

                bool isEmaiSent = _emailService.TrimiteMail(emailToAddress, emailFromAddress, emailSubject,
                                    GenerareMesajPreluareComanda(comandaDB));

               

            }

            return (indx != 0) ? true : false;

        }


        [HttpPost]
        public async Task<IActionResult> Create(ComandaVM comandaVM)
        {

            if (!ModelState.IsValid)
            {

                ViewBag.AdreseLivrare = new SelectList(_service.GetAdreseLivrare(idSucursala).Result, "Id", "Adresa");
                ViewBag.PersoaneContact = new SelectList(_service.GetAngajati(idSucursala).Result, "Id", "Nume");

                return View(comandaVM);

            }

            comandaVM.SucursalaId = idSucursala;
            comandaVM.StatusId = await _service.GetIdStatusByNameAsync(ApplicationConstantValues.Trimisa);
            comandaVM.Activ = 1;
            comandaVM.DataComanda = DateTime.Now;

            //Adaugam persoanele implicate in comanda
            comandaVM.angajati_comanda = new List<Angajat_comanda>();

            //1. Emitentul comenzii
            var angajat_comanda = new Angajat_comanda()
            {
                angajatId = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Id,
                RolComandaId = _service.GetIdRolComanda(ApplicationConstantValues.Emitent).Result.Id,

            };

            comandaVM.angajati_comanda.Add(angajat_comanda);

            //2. persoana de contact comenzii
            angajat_comanda = new Angajat_comanda()
            {
                angajatId = comandaVM.PersoanaContactId,
                RolComandaId = _service.GetIdRolComanda(ApplicationConstantValues.Persoana_contact).Result.Id,

            };

            comandaVM.angajati_comanda.Add(angajat_comanda);

            var idComanda = await _service.AddAsync(comandaVM);

            _notyfService.Success("Comanda a fost adaugata cu succes!");

            //Se trimite mail la management pentru aprobare comanda
            var emailTrimis = await NotificareTrimitereComanda(idComanda); 

            if (emailTrimis)
            {

                _notyfService.Success("Au fost trimise mailurile catre management!");

            }
            else
            {

                _notyfService.Error("A intervenit o eroare la trimiterea mailurilor");

            }

            return RedirectToAction("Index");

        }

        public IActionResult ModificareSelectieProdus(int id)
        {

            var sessionValue = HttpContext.Session.GetString("listaProduseDB");

            if (sessionValue != null)
            {

                produseVM = JsonSerializer.Deserialize<ListaProduseVM>(sessionValue)!;

            }

            foreach (var produs in produseVM.listaProduseDB)
            {

                if (produs.Id == id)
                {

                    produs.Selectat = !produs.Selectat;

                }

            }

            HttpContext.Session.SetString("listaProduseDB", JsonSerializer.Serialize(produseVM));

            return RedirectToAction("Create");

        }
        public async Task<IActionResult> Inapoi()
        {

            var actualizare = HttpContext.Session.GetString("Actualizare");

            if (actualizare != null && HttpContext.Session.GetString("Etapa") != "1")
            {

                var comanda = HttpContext.Session.GetString("Comanda");
                if (comanda != null)
                {

                    comandaVM = JsonSerializer.Deserialize<ComandaVM>(comanda)!;

                }

                produseVM.listaProduseDB = new List<ProdusVM>();

                var produseDB = await _service.GetProduse();

                if (produseDB != null)
                {

                    foreach (var produs in produseDB)
                    {

                        var produsVM = new ProdusVM()
                        {

                            Id = produs.Id,
                            Denumire = produs.Denumire,
                            Um = produs.Um,
                            Cantitate = 1,
                            Selectat = (comandaVM.listaProduse!.FirstOrDefault(p => p.Denumire == produs.Denumire) != null) ? true : false

                        };

                        produseVM.listaProduseDB.Add(produsVM);

                    }

                    HttpContext.Session.SetString("listaProduseDB", JsonSerializer.Serialize(produseVM));
                    HttpContext.Session.SetString("Etapa", "1");
                    return View("SelectieProduse", produseVM);
                }

            }
            else
            {
                var etapa = HttpContext.Session.GetString("Etapa");

                if (etapa != null && etapa == "2")
                {

                    HttpContext.Session.SetString("Etapa", "1");

                    return RedirectToAction("Create");

                }

            }

            return RedirectToAction("Index");
        }

        public IActionResult Continuare()
        {

            var produse = HttpContext.Session.GetString("listaProduseDB");

            if (produse != null)
            {

                produseVM = JsonSerializer.Deserialize<ListaProduseVM>(produse)!;

            }

            //Verificare daca exista deja produse in comanda
            var comanda = HttpContext.Session.GetString("Comanda");

            if (comanda != null)
            {

                comandaVM = JsonSerializer.Deserialize<ComandaVM>(comanda)!;

                //Stergem produsele existente
                comandaVM.listaProduse!.Clear();
            }
            else
            {

                comandaVM.listaProduse = new List<ProdusVM>();

            }

            //Adaugam in comanda produsele selectate
            foreach (ProdusVM produs in produseVM.listaProduseDB)
            {
                if (produs.Selectat == true)
                {

                    comandaVM.listaProduse!.Add(produs);

                }

            }

            if (comandaVM.listaProduse.Count == 0)
            {

                _notyfService.Error("Nu ati selectat nici un produs!");

                return View("SelectieProduse", produseVM);

            }

            HttpContext.Session.SetString("Comanda", JsonSerializer.Serialize(comandaVM));
            HttpContext.Session.SetString("Etapa", "2");


            return RedirectToAction("Create");

        }

        //GET - comenzi/Editare => Create/Etapa = 2
        public IActionResult Editare(int id)
        {

            comandaVM.listaProduse = new List<ProdusVM>();

            var comanda = _service.GetByIdAsync(id).Result;

            if(comanda.Activ == 0)
            {

                return View("Index");

            }

            comandaVM.Id = id;
            comandaVM.DataComanda = comanda.DataComanda;
            comandaVM.Valoare = comanda.Valoare;

            foreach (var persoana in comanda.angajati_comenzi!)
            {

                if (persoana.rolComanda!.Rol!.Equals(ApplicationConstantValues.Persoana_contact))
                {

                    comandaVM.PersoanaContactId = persoana.angajatId;
                }

                if (persoana.rolComanda!.Rol!.Equals(ApplicationConstantValues.Emitent))
                {

                    comandaVM.EmitentId = persoana.angajatId;
                }

                if (persoana.rolComanda!.Rol.Equals(ApplicationConstantValues.Responstabil_achizitii))
                {

                    comandaVM.ResponsabilAchizitiiId = persoana.angajatId;
                }

            }

            comandaVM.AdresaLivrareId = (int)comanda.AdresaLivrareId!;
            comandaVM.SucursalaId = comanda.SucursalaId;
            comandaVM.StatusId = comanda.StatusComandaId;
            comandaVM.Observatii = comanda.Observatii;

            foreach (var p in comanda.produse_comenzi!)
            {

                var produsVM = new ProdusVM()
                {

                    Id = p.produs.Id,
                    Denumire = p.produs.Denumire,
                    Um = p.produs.Um,
                    Cantitate = p.Cantitate,
                    Selectat = true

                };

                comandaVM.listaProduse.Add(produsVM);
            }


            HttpContext.Session.SetString("Comanda", JsonSerializer.Serialize(comandaVM));
            HttpContext.Session.SetString("Etapa", "2");
            HttpContext.Session.SetString("Actualizare", "1");

            ViewBag.AdreseLivrare = new SelectList(_service.GetAdreseLivrare(idSucursala).Result, "Id", "Adresa");
            ViewBag.PersoaneContact = new SelectList(_service.GetAngajati(idSucursala).Result, "Id", "Nume");
            ViewBag.Etapa = 2;
            ViewBag.Actualizare = 1;

            return View("Create", comandaVM);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizare(int id, ComandaVM comandaVM)
        {

            if (!ModelState.IsValid)
            {

                ViewBag.AdreseLivrare = new SelectList(_service.GetAdreseLivrare(idSucursala).Result, "Id", "Adresa");
                ViewBag.PersoaneContact = new SelectList(_service.GetAngajati(idSucursala).Result, "Id", "Nume");
                ViewBag.Actualizare = 1;

                return RedirectToAction("Create", comandaVM);

            }

            var comandaVeche = HttpContext.Session.GetString("Comanda");
            var infoComandaVM = new ComandaVM();

            if (comandaVeche != null)
            {

                infoComandaVM = JsonSerializer.Deserialize<ComandaVM>(comandaVeche);

            }

            comandaVM.SucursalaId = infoComandaVM!.SucursalaId;
            comandaVM.StatusId = infoComandaVM.StatusId;
            comandaVM.DataComanda = infoComandaVM.DataComanda;
            comandaVM.Id = infoComandaVM.Id;
            comandaVM.EmitentId = infoComandaVM.EmitentId;
            comandaVM.ResponsabilAchizitiiId = infoComandaVM.ResponsabilAchizitiiId;

            await _service.UpdateAsync((int)comandaVM.Id!, comandaVM);

            _notyfService.Success("Datele comenzii au fost actualizate in baza de date");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detalii(int id)
        {

            var comanda = await _service.GetByIdAsync(id);

            return View(comanda);
        }

        //Schimbare status din Activa in Inactiva si invers
        public async Task<IActionResult> SchimbaStareComanda(int id, string status)
        {

            await _service.SchimbaStareComanda(id);

            _notyfService.Success("Starea comenzii a fost actualizata cu succes!");

            return RedirectToAction("Index");
            
        }
    }
}
