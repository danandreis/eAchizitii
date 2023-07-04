using AspNetCoreHero.ToastNotification.Abstractions;
using eAchizitii.Data;
using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eAchizitii.Controllers
{

    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notyfService;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, INotyfService notyfService, 
            RoleManager<IdentityRole> roleManager)
        {

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _notyfService = notyfService;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {

            var loginUser = new LoginUserVM();
            return View(loginUser);

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM loginUser)
        {

            if (!ModelState.IsValid)
            {

                return View(loginUser);

            }

            var utilizator = await _userManager.FindByEmailAsync(loginUser.Email);

            if(utilizator == null)
            {

                _notyfService.Error("Datele de autentificare sunt incorecte!");
                
                return RedirectToAction("Login");

            }

            if(utilizator!.LockoutEnd > DateTime.Now || utilizator.AccessFailedCount == 3)
            {

                await _userManager.SetLockoutEndDateAsync(utilizator, DateTime.Now.AddMonths(1));

                return View("UtilizatorBlocat");
            }

            if(utilizator != null)
            {

                var passwordCheck = await _userManager.CheckPasswordAsync(utilizator, loginUser.Password);

                if (passwordCheck)
                {

                    var result = await _signInManager.PasswordSignInAsync(utilizator, loginUser.Password, false, false);

                    if (result.Succeeded)
                    {

                        _notyfService.Success("Autentificare cu succes!");

                        if (_userManager.IsInRoleAsync(utilizator, "Management").Result)
                        {

                            return RedirectToAction("Index","Management");

                        }

                        if (_userManager.IsInRoleAsync(utilizator, "Admin").Result)
                        {

                            return RedirectToAction("ListaUtilizatori");

                        }
                       
                        if (_userManager.IsInRoleAsync(utilizator, "Achizitii").Result)
                        {

                            return RedirectToAction("ComenziAlocate", "Achizitii");

                        }

                        if (_userManager.IsInRoleAsync(utilizator, "User").Result 
                            || _userManager.IsInRoleAsync(utilizator, "Director Sucursala").Result)
                        {

                            return RedirectToAction("Index", "Comenzi");

                        }
                    }

                }
                else
                {

                    await _userManager.AccessFailedAsync(utilizator);


                }

            }

            _notyfService.Error("Datele de autentificare sunt incorecte");

            return View(loginUser);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create()
        {

            var listaSucursale = new SelectList(await _context.Sucursale.OrderBy(s => s.Denumire).ToListAsync(), "Id", "Denumire");
            var listaRoluri = new SelectList(await _roleManager.Roles.OrderBy(r=>r.NormalizedName).ToListAsync(),"Name", "NormalizedName");
            
            var utilizatorNou = new UtilizatorNouVM();

            ViewBag.listaSucursale = listaSucursale;
            ViewBag.listaRoluri = listaRoluri;

            return View(utilizatorNou);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UtilizatorNouVM utilizatorNou)
        {

            var listaSucursale = new SelectList(await _context.Sucursale.OrderBy(s => s.Denumire).ToListAsync(), "Id", "Denumire");
            var listaRoluri = new SelectList(await _roleManager.Roles.OrderBy(r => r.NormalizedName).ToListAsync(), "Name", "NormalizedName");

            ViewBag.listaSucursale = listaSucursale;
            ViewBag.listaRoluri = listaRoluri;

            if (!ModelState.IsValid)
            {
            
                return View(utilizatorNou);

            }

            var user = await _userManager.FindByEmailAsync(utilizatorNou.Email);

            if(user != null)
            {

             _notyfService.Error("Exista deja in baza de date un utilizator cu aceasta parola!");

                return View(utilizatorNou);

            }

            string numeUtilizator = utilizatorNou.Nume.Replace(" ", ".").ToLower();

            var utilizator = new AppUser()
            {

                Nume = utilizatorNou.Nume,
                Email = utilizatorNou.Email,
                UserName = numeUtilizator,
                SucursalaId = utilizatorNou.SucursalaId,
                PhoneNumber = utilizatorNou.Telefon,
                
            };

            var response = await _userManager.CreateAsync(utilizator, utilizatorNou.Password);

            if (response.Succeeded)
            {

                await _userManager.AddToRoleAsync(utilizator, utilizatorNou.Rol);
                
                _notyfService.Success("Utilizatorul a fost adaugat cu succes");

                return RedirectToAction("ListaUtilizatori", "Account");

            }

            _notyfService.Error("A intervenit o eroare la inregistrarea utilizatorului");
            
            return View(utilizatorNou);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();

            return RedirectToAction("Login","Account");
        }

        [Authorize]
        public IActionResult ResetareParola()
        {

            var idUser = _userManager.FindByNameAsync(User.Identity!.Name).Result.Id;

            var resetareParola = new ResetareParolaVM()
            {

                Id = idUser

        };

            return View(resetareParola);

        }

        [HttpPost]
        public async Task<IActionResult>ResetareParola(ResetareParolaVM resetareParola)
        {

            if(!ModelState.IsValid)
            {

                return View(resetareParola);

            }

            var user =  _userManager.FindByNameAsync(User.Identity!.Name).Result;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult parolaNoua = await _userManager.ResetPasswordAsync(user, token, resetareParola.Password);

            if(!parolaNoua.Succeeded)
            {
                _notyfService.Error("A intervenit o eroare la actualizarea parole");
                return View(resetareParola);

            }

            _notyfService.Success("Parola a fost actualizata");

            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {

                return RedirectToAction("ListaUtilizatori");

            }

            return RedirectToAction("Index", "Comenzi");



        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ListaUtilizatori(string rol="Toate")
        {

            if(rol != "Toate" && await _roleManager.FindByNameAsync(rol) == null)
            {

                _notyfService.Error("NU exista rolul :" + rol.ToUpper());
                return RedirectToAction("ListaUtilizatori");

            }

            var listaUtilizatoriDB = await _context.Users.Include(u=>u.sucursala).Where(u=>u.UserName != User.Identity!.Name).
                OrderBy(u=>u.Nume).ToListAsync();

            List<UtilizatorVM> listaUtilizatoriView = new List<UtilizatorVM>();

            foreach(var utilizator in listaUtilizatoriDB)
            {

                if (rol == "Toate" || _userManager.GetRolesAsync(utilizator).Result.IndexOf(rol) != -1)
                {

                    var utilizatorCurent = new UtilizatorVM()
                    {

                        Id = utilizator.Id,
                        Nume = utilizator.Nume,
                        UserName = utilizator.UserName,
                        Email = utilizator.Email,
                        Sucursala = utilizator.sucursala!.Denumire,
                        Activ = (utilizator.LockoutEnd == null || utilizator.LockoutEnd < DateTime.Now) ? 1 : 0

                    };

                    listaUtilizatoriView.Add(utilizatorCurent);
                }
            }

            //Se extrag rolurile pentru selectia pe roluri
            List<SelectListItem> listaRoluri = new SelectList(await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync(), 
                "Name", "NormalizedName").ToList();


            var toateRolurile = new SelectListItem()
            {
                Text = "Toate",
                Value = "Toate"
            };

            listaRoluri.Insert(0, toateRolurile);

            if (rol != "Toate")
            {
                foreach (var item in listaRoluri)
                {

                    if(item.Value == rol)
                    {
                        listaRoluri[listaRoluri.IndexOf(item)].Selected = true;

                    }
                }
            }

            ViewBag.Roluri = listaRoluri;

            return View(listaUtilizatoriView);

        }

        public IActionResult AccessDenied(string ReturnURL)
        {

            return View();

        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ResetareParolaUser(string name)
        {
            
            var utilizator =  _userManager.FindByNameAsync(name).Result;

            if (utilizator == null)
            {

                _notyfService.Error("Nu exista utilizatorul " + name.ToUpper() + " in baza de date!");

            }
            else
            { 

                var token = await _userManager.GeneratePasswordResetTokenAsync(utilizator);

                IdentityResult result = await _userManager.ResetPasswordAsync(utilizator, token, "Password_1234");

                if (result.Succeeded)
                {

                    _notyfService.Success("Parola a fost resetata cu succes!");


                }
                else
                {

                    _notyfService.Error("A intervenit o eroare la resetarea parolei!");
                
                }

            }

            return RedirectToAction("ListaUtilizatori");

        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DezactivareCont(string name)
        {

            var utilizator = _userManager.FindByNameAsync(name).Result;

            if(utilizator == null)
            {

                _notyfService.Error("Nu exista utilizatorul cu numele " + name.ToUpper() + " in baza de date!");

            }
            IdentityResult result = await _userManager.SetLockoutEndDateAsync(utilizator!, DateTime.Now.AddMonths(1));

            if (result.Succeeded)
            {

                _notyfService.Success("Utilizatorul a fost blocat pana la data de " + DateTime.Now.AddMonths(1));
            }

            return RedirectToAction("ListaUtilizatori");
            
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ActivareCont(string name)
        {

            var utilizator = _userManager.FindByNameAsync(name).Result;

            if(utilizator == null)
            {

                _notyfService.Error("Utilizatorul cu numele " + name.ToUpper() + " nu exista in baza de date!");

            }

            IdentityResult result = await _userManager.SetLockoutEndDateAsync(utilizator!, DateTime.Now.AddDays(-1));
            
            await _userManager.ResetAccessFailedCountAsync(utilizator!);

            if (result.Succeeded)
            {

                _notyfService.Success("Utilizatorul a fost deblocat!");

            }

            return RedirectToAction("ListaUtilizatori");

        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdaugaRol()
        {

            RolMV rolUtilizator = new RolMV();

            return View(rolUtilizator);

        }

        [HttpPost]
        public async Task<IActionResult> AdaugaRol(RolMV rolUtilizator)
        {

            if (!ModelState.IsValid)
            {

                return View(rolUtilizator);

            }

            var rol = _roleManager.Roles.FirstOrDefaultAsync(r => r.NormalizedName.Equals(rolUtilizator.Nume.ToUpper())).Result;

            if(rol != null)
            {

                _notyfService.Error("Acest rol exista deja in baza de date!");

                return View(rolUtilizator);

            }

            var rezultat = await _roleManager.CreateAsync(new IdentityRole(rolUtilizator.Nume));

            if (rezultat.Succeeded)
            {

                _notyfService.Success("Rolul a fost adaugat in baza de date!");

            }
            else
            {

                _notyfService.Error("A intervenit o eroare la adaugarea rolului in baza de date");

            }

            return RedirectToAction("ListaUtilizatori");
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Editare(string id)
        {

            //Cauatre utilizator dupa user.name
            var utilizatorDB = await _userManager.FindByIdAsync(id);

            if(utilizatorDB == null)
            {

                _notyfService.Error("Nu exista utilizatorul cu acest ID in baza de date!");

                return RedirectToAction("ListaUtilizatori");

            }

            var listaSucursale = new SelectList(await _context.Sucursale.OrderBy(s => s.Denumire).ToListAsync(), "Id", "Denumire");
            var listaRoluri = new SelectList(await _roleManager.Roles.OrderBy(r => r.NormalizedName).ToListAsync(), "Name", "NormalizedName");

            ViewBag.listaSucursale = listaSucursale;
            ViewBag.listaRoluri = listaRoluri;

            var utilizator = new UtilizatorNouVM()
            {

                Id = utilizatorDB.Id,
                Nume = utilizatorDB.Nume,
                Email = utilizatorDB.Email,
                SucursalaId = (int)utilizatorDB.SucursalaId!,
                Telefon = utilizatorDB.PhoneNumber,
                Rol = _userManager.GetRolesAsync(utilizatorDB).Result[0]
                //Password = _userManager.GetClaimsAsync(user).Result

            };

            return View(utilizator);
        }

        [HttpPost]
        public async Task<IActionResult> Editare(string id, UtilizatorNouVM utilizatorNou)
        {

            ModelState["Password"]!.ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {

               var listaSucursale = new SelectList(await _context.Sucursale.OrderBy(s => s.Denumire).ToListAsync(), "Id", "Denumire");
               var listaRoluri = new SelectList(await _roleManager.Roles.OrderBy(r => r.NormalizedName).ToListAsync(), "Name", "NormalizedName");

                ViewBag.listaSucursale = listaSucursale;
                ViewBag.listaRoluri = listaRoluri;

                return View(utilizatorNou);
            }

            var utilizatorBD = await _userManager.FindByIdAsync(id);

            if(utilizatorBD != null)
            {
                utilizatorBD.Nume = utilizatorNou.Nume;
                utilizatorBD.UserName = utilizatorNou.Nume.Replace(" ", ".").ToLower();
                utilizatorBD.Email = utilizatorNou.Email;
                utilizatorBD.SucursalaId = utilizatorNou.SucursalaId;
                utilizatorBD.PhoneNumber = utilizatorNou.Telefon;
                

                var result = await _userManager.UpdateAsync(utilizatorBD);

                if (!result.Succeeded)
                {

                    _notyfService.Error("A intervenit o eroare la actualizare datelor!");

                }
                else
                {

                    //Se actualizeaza si rolul in baza de date
                    var rol = await _userManager.GetRolesAsync(utilizatorBD);

                    foreach (var item in rol)
                    {

                        if (item != utilizatorNou.Rol)
                        {
                            await _userManager.RemoveFromRoleAsync(utilizatorBD, item);

                            await _userManager.AddToRoleAsync(utilizatorBD, utilizatorNou.Rol);

                        }

                    }

                    _notyfService.Success("Datele utilizatorului au fost actualizate cu succes!");

                }

            };

            return RedirectToAction("ListaUtilizatori");

        }
    }
}
