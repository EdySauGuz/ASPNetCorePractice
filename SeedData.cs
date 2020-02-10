using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AspNetCoreTodo
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }
        // Verifica si existe rol de admin.
        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }
        // Crea Usuario admin, si no existe.
        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            /*var u = await userManager.FindByNameAsync("drukker135@gmail.com");
                await userManager.AddToRoleAsync(u,Constants.AdministratorRole);

                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                        .AddConsole()
                        .AddEventLog();
                });
                ILogger logger = loggerFactory.CreateLogger<SeedData>();
                logger.LogInformation("Agregando rol a usuario");*/
            
            if (testAdmin != null){
                return;
            }else{
                //await userManager.RemovePasswordAsync(testAdmin);
                //await userManager.AddPasswordAsync(testAdmin,"Admin.123");
                //var ec = await userManager.IsEmailConfirmedAsync(testAdmin);
                //if(ec == false){
                //    var token = await userManager.GenerateEmailConfirmationTokenAsync(testAdmin);
                //    await userManager.ConfirmEmailAsync(testAdmin, token);
                //}
            }

            testAdmin = new IdentityUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, "Admin.123");
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
    }
}