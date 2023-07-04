using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data.Services;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Security.Principal;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace eAchizitii.Controllers
{

    [Authorize(Roles = "Achizitii, Management, Director Sucursala")]
    public class InfoComandaController : Controller
    {

        private readonly IInfoComandaService _serviceInfoComanda;
        private readonly IComenziService _serviceComenzi;
        private readonly IPrincipal _principal;
        private readonly INotyfService _notyfService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private InfoComandaVM infoComandaVM = new InfoComandaVM();

        public InfoComandaController(IInfoComandaService serviceInfoComanda, IPrincipal principal, 
            INotyfService notyfService, UserManager<AppUser> userManager, IComenziService serviceComenzi, 
            IWebHostEnvironment webHostEnvironment)
        {

            _serviceInfoComanda = serviceInfoComanda;
            _principal = principal;
            _notyfService = notyfService;
            _userManager = userManager;
            _serviceComenzi = serviceComenzi;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<InfoComandaVM> SetareInfoComandaVM(int idComanda)
        {


            var comandaDB = await _serviceComenzi.GetByIdAsync(idComanda);

            if (comandaDB == null)
            {

                return null!;

            }

            var infoComenzi = await _serviceInfoComanda.GetInfoComenziByIdComanda(idComanda);

            infoComandaVM.infoComandaCurentaVM = new InfoComandaCurentaVM();
            infoComandaVM.infoComandaCurentaVM.comanda = new Comanda();
            infoComandaVM.infoComandaCurentaVM.comanda = comandaDB;
            infoComandaVM.infoComandaCurentaVM.ComandaId = comandaDB.Id;
            infoComandaVM.infoComandaCurentaVM.Data = DateTime.Now;

            infoComandaVM.listaInfoComenzi = new List<InfoComanda>();

            foreach (var info in infoComenzi)
            {

                infoComandaVM.listaInfoComenzi.Add(info);

            }

            return infoComandaVM;

        }

        public async Task<IActionResult> Index(int idComanda)
        {

            var infoComanda = await SetareInfoComandaVM(idComanda);

            if (infoComanda == null)
            {

                return RedirectToAction("DetaliiComanda", "Achizitii");

            }

            return View(infoComanda);

        }

        //Setare nume fisiere cu Guid()
        private string SetareNumeNoiFisiereIncarcate(IFormFile Fisier)
        {

            var numeNouFisier = Guid.NewGuid() + Fisier.FileName.Substring(Fisier.FileName.IndexOf("."));

            return numeNouFisier;

        }

        //Salavare fisiere cu noile nume
        private bool SalvareFisiere(List<IFormFile> listaFisiereUpload, Dictionary<string,string> listaFisiereNumeNoi)
        {

            bool isFileSaved = false;
            //var caleDirector = Path.Combine(_webHostEnvironment.WebRootPath, "Fisiere");
           
            if (!Directory.Exists("./Fisiere"))
            {

                Directory.CreateDirectory("./Fisiere");

            }
            else
            {

                foreach (var fisier in listaFisiereUpload)
                {

                    var caleFisier = Path.Combine("./Fisiere", listaFisiereNumeNoi.FirstOrDefault(f=>f.Value == fisier.FileName).Key);

                    using (FileStream f = System.IO.File.Create(caleFisier))
                    {

                        fisier.CopyTo(f);
                        isFileSaved = true;
                        continue;

                    }

                }

            }

            return isFileSaved;

        }


        [HttpPost]
        public async Task<IActionResult> SalvareInfoComanda(InfoComandaCurentaVM infoComandaCurentaVM)
        {

            var infoComandaVM = await SetareInfoComandaVM((int)infoComandaCurentaVM.ComandaId!);

            ModelState["utilizatorId"]!.ValidationState = ModelValidationState.Valid;
            ModelState["listaFisiereDB"]!.ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {

                return View("Index",infoComandaVM);

            }

            infoComandaVM.infoComandaCurentaVM.Descriere = infoComandaCurentaVM.Descriere;
            infoComandaVM.infoComandaCurentaVM.utilizatorId = _userManager.FindByNameAsync(_principal.Identity!.Name).Result.Id;
            
            //Salvare fisiere pe disc
            if (infoComandaCurentaVM.listaFisiereUpload != null)
            {
                infoComandaVM.infoComandaCurentaVM.listaFisiereUpload = new List<IFormFile>();
                infoComandaVM.infoComandaCurentaVM.listaFisiereUpload.AddRange(infoComandaCurentaVM.listaFisiereUpload);

                infoComandaVM.infoComandaCurentaVM.listaFisiereDB = new Dictionary<string, string>();

                foreach (var fisier in infoComandaCurentaVM.listaFisiereUpload)
                {

                    var numeNouFisier = SetareNumeNoiFisiereIncarcate(fisier);
                    infoComandaVM.infoComandaCurentaVM.listaFisiereDB[numeNouFisier] = fisier.FileName;

                }
             
                SalvareFisiere(infoComandaCurentaVM.listaFisiereUpload, infoComandaVM.infoComandaCurentaVM.listaFisiereDB);

            }

            //Adaugare informatii in BD (atat informatia din comanda cat si fisierele aferente)
            var isInfoSaved = await _serviceInfoComanda.AddInfoComandaAsync(infoComandaVM.infoComandaCurentaVM);
          
            if (isInfoSaved)
            {

                _notyfService.Success("Informatia a fost salvata in baza de date!");


            }
            else
            {

                _notyfService.Error("A intervenit o eroare la salvarea informatiei in baza de date!");

            }

            return RedirectToAction("Index", new {idComanda = infoComandaVM.infoComandaCurentaVM.ComandaId});

        }

        public async Task<IActionResult> Editare(int id)
        {

            //Verific daca vine din pagina de stergere fisiere
            var stergereFisiere = HttpContext.Session.GetString("stergereFisiere");

            //Daca nu vine
            if(stergereFisiere == null)
            {


                HttpContext.Session.Remove("idComanda");
                HttpContext.Session.Remove("listaFisiereDB");

            }

            var infoComandaDB = await _serviceInfoComanda.GetInfoComandaById(id);

            if(infoComandaDB != null)
            {

                var infoComandaView = await SetareInfoComandaVM(infoComandaDB.comanda!.Id);
                infoComandaView.infoComandaCurentaVM.Id = infoComandaDB.Id;
                infoComandaView.infoComandaCurentaVM.Descriere = infoComandaDB.Descriere;

                infoComandaView.infoComandaCurentaVM.listaFisiereDB = new Dictionary<string, string>();

                var listaFisiere = HttpContext.Session.GetString("listaFisiereDB");

                if (listaFisiere != null)
                {

                    infoComandaView.infoComandaCurentaVM.listaFisiereDB = 
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(listaFisiere);
                        
                }
                else
                {
                    if (infoComandaDB.fisiere != null)
                    {
                        foreach (var fisier in infoComandaDB.fisiere)
                        {

                            infoComandaView.infoComandaCurentaVM.listaFisiereDB[fisier.numeFisierDisc] = fisier.numeFisierBazaDate;

                        }

                        //Serializare lista fisiere 
                        HttpContext.Session.SetString("listaFisiereDB", JsonConvert.SerializeObject(infoComandaView.infoComandaCurentaVM.listaFisiereDB));
                      
                        //Serializam si id-ul de comanda
                        HttpContext.Session.SetString("idComanda", JsonSerializer.Serialize(id));
                    }
                }

                return View("Index", infoComandaView);

            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> ActualizeazaInformatia(InfoComandaCurentaVM infoComandaCurentaVM)
        {

            infoComandaCurentaVM.listaFisiereDB = new Dictionary<string, string>();

            var listaFisiere = HttpContext.Session.GetString("listaFisiereDB");

            if (listaFisiere != null)
            {

                infoComandaCurentaVM.listaFisiereDB = JsonConvert.DeserializeObject<Dictionary<string, string>>(listaFisiere);
            
            }

            //Se verifica daca s-au adaugat fisiere noi
            if (infoComandaCurentaVM.listaFisiereUpload != null)
            {

                //Daca da se genereaza noild nume cu functis Guid() si se salveaza fisierele

                //Setare nume folosind Guid()
                foreach (var fisier in infoComandaCurentaVM.listaFisiereUpload)
                {

                    var numeNouFisier = SetareNumeNoiFisiereIncarcate(fisier);
                    infoComandaCurentaVM.listaFisiereDB[numeNouFisier] = fisier.FileName;

                }

                //Salvare fisiere cu noul nume
                SalvareFisiere(infoComandaCurentaVM.listaFisiereUpload, infoComandaCurentaVM.listaFisiereDB);

            }

            //Se sterg de pe disc fisierele care s-au eliminat din comanda 
            foreach (KeyValuePair<string,string> keyValuePair in infoComandaCurentaVM.listaFisiereDB)
            {

                if (keyValuePair.Value == null)
                {

                    //Se sterge si fisierul de pe disk
                    var caleFisier = Path.Combine("./Fisiere/", keyValuePair.Key);

                    System.IO.File.Delete(caleFisier);

                }

            }

            var isSaved = await _serviceInfoComanda.UpdateInfoComanda(infoComandaCurentaVM);

            if (isSaved)
            {

                _notyfService.Success("Informatiile au fost actulizate in baza de date!");

            }
            else
            {

                _notyfService.Error("A intervenit o eroare la actulizarea bazei de date!");

            }

            HttpContext.Session.Remove("idComanda");
            HttpContext.Session.Remove("listaFisiereDB");
            HttpContext.Session.Remove("stergereFisiere");

            return RedirectToAction("Index", new { idComanda = infoComandaCurentaVM.ComandaId });

        }

        //Metoda pentru stergere fisier aferent comenzii care se modifica
        public IActionResult StergeFisierComanda(string numeOriginalFisier)
        {


            //Se verifica daca exista lista de fisiere serializate
            var listaFisiereDb = HttpContext.Session.GetString("listaFisiereDB");
            
            if ( listaFisiereDb!= null)
            {

                //Se deserializeaza dictionarul
                Dictionary<string, string> listaFisiere = JsonConvert.DeserializeObject<Dictionary<string,string>>(listaFisiereDb);

                if (numeOriginalFisier != null)
                {

                    string cheie = listaFisiere.FirstOrDefault(f => f.Value == numeOriginalFisier).Key;

                    listaFisiere[cheie] = null!;

                }

                //Serializare lista fisiere din baza de date
                HttpContext.Session.SetString("listaFisiereDB", JsonConvert.SerializeObject(listaFisiere));

            }

            HttpContext.Session.SetString("stergereFisiere", "1");

            var idComanda = JsonSerializer.Deserialize<int>(HttpContext.Session.GetString("idComanda")!);

            return RedirectToAction("Editare", new { id = idComanda });

        }

    }

}
