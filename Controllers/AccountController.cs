using InventarioVentasMVC.Models;
using InventarioVentasMVC.ViewModels;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace InventarioVentasMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly InventarioContext _context;

        public AccountController(InventarioContext context)
        {
            _context = context;
        }

        // =====================================================
        // LOGIN GET
        // =====================================================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =====================================================
        // LOGIN POST
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Username == vm.Username &&
                    u.PasswordHash == vm.Password &&
                    u.Activo
                );

            if (usuario == null)
            {
                ModelState.AddModelError(
                    "",
                    "Usuario o contraseña incorrectos."
                );

                return View(vm);
            }

            // =========================================
            // CLAIMS
            // =========================================

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Name,
                    usuario.Username
                ),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Rol
                ),

                new Claim(
                    "UsuarioId",
                    usuario.UsuarioId.ToString()
                )
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            // =========================================
            // LOGIN
            // =========================================

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // =====================================================
        // LOGOUT
        // =====================================================

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction(
                "Login",
                "Account"
            );
        }
    }
}