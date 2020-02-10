using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Controllers
{
    [Authorize(Roles = Constants.AdministratorRole)]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ManageUsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager
                .GetUsersInRoleAsync(Constants.AdministratorRole))
                .ToArray();
            var everyone = await _userManager.Users
                .ToArrayAsync();
            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone
            };
            return View(model);
        }
        // Acción para eliminar usuario.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(String Email){
            if(Email == null){
                return BadRequest("No fue posible obtener email de usuario.");
            }

            var user = await _userManager.FindByEmailAsync(Email);
            var result = await _userManager.DeleteAsync(user);

            if(result.Succeeded){
                return RedirectToAction("Index");
            }else{
                return BadRequest("No fue posible eliminar el usuario.");
            }
        }
    }
}