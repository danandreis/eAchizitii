using eAchizitii.Models;
using Microsoft.AspNetCore.Identity;

namespace eAchizitii.Data
{
    public class CreateAdminAccount
    {

        public static async Task AddAdminAsync(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync("Admin"))
                {

                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var admin = await userManager.FindByEmailAsync("admin@achizitii.com");

                if(admin == null)
                {

                    var utilizatorAdmin = new AppUser()
                    {

                        Nume = "Administrator",
                        UserName = "Administrator",
                        Email = "admin@achizitii.com",
                        EmailConfirmed = true,
                        SucursalaId = 16
                    
                    };

                    await userManager.CreateAsync(utilizatorAdmin, "Admin_1234");

                    await userManager.AddToRoleAsync(utilizatorAdmin, "Admin");
                }
            }
        }
    }
}
