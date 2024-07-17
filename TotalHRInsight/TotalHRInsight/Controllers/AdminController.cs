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
        private readonly TotalHRInsightDbContext _context;

        public AdminController(TotalHRInsightDbContext context, AuthDbContext authDbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _authDbContext = authDbContext;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userRoleViewModelList = new List<UserRoleViewModel>();
            var users = await _userManager.Users.Include(u => u.Sucursal).ToListAsync();

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
			ViewData["Sucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
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
                user.idSucursal = usuarioModel.idSucursal;
                user.Estado = true;
                var result = await _userManager.CreateAsync(user, usuarioModel.Password);
                if (result.Succeeded)
                { 
                    string normalizeRoleName = _roleManager.Roles.FirstOrDefault(r => r.Id == usuarioModel.IdRol).NormalizedName;
                    var resultRole = await _userManager.AddToRoleAsync(user, normalizeRoleName);
                    return RedirectToAction("Index", "Admin");
                }
            }
            var listaRol = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(listaRol, "Id", "Name");
			ViewData["Sucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal");
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
            ViewData["Sucursal"] = new SelectList(_context.Sucursales, "IdSucursal", "NombreSucursal", user.idSucursal);
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
                //idSucursal = user.FirstOrDefault(r => userRoles.Contains(r.Name))?.Id,
                SelectedRoleId = roles.FirstOrDefault(r => userRoles.Contains(r.Name))?.Id
        };

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model) 
        {
            // Remove the validation for Password and ConfirmPassword if they are empty
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.Remove(nameof(model.Password));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }
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

            user.Nombre = model.Nombre;
            user.PrimerApellido = model.PrimerApellido;
            user.SegundoApellido = model.SegundoApellido;
            user.Salario = model.Salario;
            user.FechaNacimiento = model.FechaNacimiento;
            user.NumeroTelefono = model.NumeroTelefono;
            user.Email = model.Email;
            user.UserName = model.Email;

            if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    foreach (var error in removePasswordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.SelectedRoleId);
                    return View(model);
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.SelectedRoleId);
                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["Roles"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.SelectedRoleId);
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var newRole = await _roleManager.FindByIdAsync(model.SelectedRoleId);
            if (newRole == null)
            {
                ModelState.AddModelError("", "The selected role is invalid.");
                return View(model);
            }

            if (!currentRoles.Contains(newRole.Name))
            {
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
