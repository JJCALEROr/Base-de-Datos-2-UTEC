using InventarioVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
public class UsuariosController : Controller
{
    private readonly InventarioContext _context;

    public UsuariosController(InventarioContext context)
    {
        _context = context;
    }

    // GET: Usuarios
    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuarios.ToListAsync());
    }

    // GET: Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // GET: Usuarios/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("UsuarioId,Nombre,Apellido,Username,PasswordHash,Rol,Activo")]
        Usuario usuario)
    {
        ModelState.Remove("Ventas");
        ModelState.Remove("Compras");
        ModelState.Remove("MovimientosInventario");

        if (ModelState.IsValid)
        {
            try
            {
                usuario.FechaCreacion = DateTime.Now;

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Usuario creado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
        }

        return View(usuario);
    }

    // GET: Usuarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("UsuarioId,Nombre,Apellido,Username,PasswordHash,Rol,Activo")]
        Usuario usuario)
    {
        if (id != usuario.UsuarioId)
        {
            return NotFound();
        }

        ModelState.Remove("Ventas");
        ModelState.Remove("Compras");
        ModelState.Remove("MovimientosInventario");

        if (ModelState.IsValid)
        {
            try
            {
                // Mantener fecha original
                var fechaOriginal = await _context.Usuarios
                    .AsNoTracking()
                    .Where(u => u.UsuarioId == id)
                    .Select(u => u.FechaCreacion)
                    .FirstOrDefaultAsync();

                usuario.FechaCreacion = fechaOriginal;

                _context.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Usuario actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.UsuarioId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");
            }
        }

        return View(usuario);
    }

    // GET: Usuarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.UsuarioId == id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario != null)
        {
            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    $"No se puede eliminar el usuario: {ex.Message}";
            }
        }

        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.UsuarioId == id);
    }
}