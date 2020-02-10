using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener usuario.
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            // Obtener las tareas desde la base de datos
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            // Colocar los tareas en un modelo
            var model = new TodoViewModel()
            {
                Items = items
            };
            // Genera la vista usando el modelo
            return View(model);
        }
        // Acción para agregar tarea a lista.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            // Obtener usuario.
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);
            if (!successful)
            {
                return BadRequest("No fue posible agregar la tarea.");
            }
            return RedirectToAction("Index");
        }
        // Marcar tarea como completada.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            // Obtener usuario.
            var currentUser = await _userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();
            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);
            if (!successful)
            {
                return BadRequest("No fue posible marcar tarea como Completada.");
            }
            return RedirectToAction("Index");
        }
    }
}
