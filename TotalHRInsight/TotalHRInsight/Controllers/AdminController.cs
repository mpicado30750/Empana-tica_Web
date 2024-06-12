using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TotalHRInsight.DAL;
using TotalHRInsight.Models;

namespace TotalHRInsight.Controllers
{
    public class AdminController : Controller
    {
        private readonly AuthDbContext _authDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AuthDbContext authDbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _authDbContext = authDbContext;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userRoleViewModelList = new List<UserRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userRoleViewModel = new UserRoleViewModel
                {
                    User = user,
                    Roles = userRoles
                };
                userRoleViewModelList.Add(userRoleViewModel);
            }

            return View(userRoleViewModelList);
        }

        public IActionResult CrearUsuario()
        {
            var listaRol = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(listaRol, "Id", "Name");
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearUsuario(AdminCrearUsuarioViewModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                await _userStore.SetUserNameAsync(user, usuarioModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, usuarioModel.Email, CancellationToken.None);
                user.Nombre = usuarioModel.Nombre;
                user.PrimerApellido = usuarioModel.PrimerApellido;
                user.SegundoApellido = usuarioModel.SegundoApellido;
                user.Salario = usuarioModel.Salario;
                user.FechaNacimiento = usuarioModel.FechaNacimiento;
                user.FechaRegistro = DateTime.Now;
                user.NumeroTelefono = usuarioModel.NumeroTelefono;
                user.Estado = true;
                var result = await _userManager.CreateAsync(user, usuarioModel.Password);
                if (result.Succeeded)
                { 
                    string normalizeRoleName = _roleManager.Roles.FirstOrDefault(r => r.Id == usuarioModel.IdRol).NormalizedName;
                    var resultRole = await _userManager.AddToRoleAsync(user, normalizeRoleName);
                    return RedirectToAction("Index", "Home");
                }
            }
            var listaRol = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(listaRol, "Id", "Name");
            return View(usuarioModel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.ToListAsync();
            var listaRol = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(listaRol, "Id", "Name");
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Nombre = user.Nombre,
                PrimerApellido = user.PrimerApellido,
                SegundoApellido = user.SegundoApellido,
                Salario = user.Salario,
                FechaNacimiento = user.FechaNacimiento,
                NumeroTelefono = user.NumeroTelefono,
                Email = user.Email,
                SelectedRoleId = roles.FirstOrDefault(r => userRoles.Contains(r.Name))?.Id
        };

            return View(model);
        }


        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.SelectedRoleId);
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades del usuario
            user.Nombre = model.Nombre;
            user.PrimerApellido = model.PrimerApellido;
            user.SegundoApellido = model.SegundoApellido;
            user.Salario = model.Salario;
            user.FechaNacimiento = model.FechaNacimiento;
            user.NumeroTelefono = model.NumeroTelefono;
            user.Email = model.Email;

            // Guardar los cambios en el usuario
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                // Si no se pudo actualizar, agrega los errores al ModelState y muestra la vista de nuevo
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.SelectedRoleId);
                return View(model);
            }

            // Fetch the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Find the new role by ID from the model
            var newRole = await _roleManager.FindByIdAsync(model.SelectedRoleId);
            if (newRole == null)
            {
                ModelState.AddModelError("", "The selected role is invalid.");
                return View(model);
            }

            // Update the user role if it has changed
            if (!currentRoles.Contains(newRole.Name))
            {
                // Remove all roles currently assigned to the user and add the new role
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole.Name);
            }

            return RedirectToAction(nameof(Index));
        }





        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Estado = false; // Establecer el usuario como inactivo
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Ativar/5
        public async Task<IActionResult> Activar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Ativar/5
        [HttpPost, ActionName("Activar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Estado = true; // Establecer el usuario como Ativar
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
