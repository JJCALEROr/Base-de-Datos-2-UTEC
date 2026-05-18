using InventarioVentasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin,Bodeguero")]

public class ProveedoresController : Controller
{
    private readonly InventarioContext _context;

    public ProveedoresController(InventarioContext context)
    {
        _context = context;
    }

    // GET: Proveedores
    public async Task<IActionResult> Index()
    {
        return View(await _context.Proveedores.ToListAsync());
    }

    // GET: Proveedores/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(m => m.ProveedorId == id);

        if (proveedor == null)
        {
            return NotFound();
        }

        return View(proveedor);
    }

    // GET: Proveedores/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Proveedores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProveedorId,RazonSocial,NIT,Telefono,Email,Direccion,Activo,FechaCreacion")] Proveedor proveedor)
    {
        ModelState.Remove("Productos");
        ModelState.Remove("Compras");

        if (ModelState.IsValid)
        {
            try
            {
                proveedor.FechaCreacion = DateTime.Now;

                _context.Add(proveedor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proveedor creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
        }

        return View(proveedor);
    }

    // GET: Proveedores/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var proveedor = await _context.Proveedores.FindAsync(id);

        if (proveedor == null)
        {
            return NotFound();
        }

        return View(proveedor);
    }

    // POST: Proveedores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProveedorId,RazonSocial,NIT,Telefono,Email,Direccion,Activo,FechaCreacion")] Proveedor proveedor)
    {
        if (id != proveedor.ProveedorId)
        {
            return NotFound();
        }

        ModelState.Remove("Productos");
        ModelState.Remove("Compras");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(proveedor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Proveedor actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExists(proveedor.ProveedorId))
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

        return View(proveedor);
    }

    // GET: Proveedores/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(m => m.ProveedorId == id);

        if (proveedor == null)
        {
            return NotFound();
        }

        return View(proveedor);
    }

    // POST: Proveedores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);

        if (proveedor != null)
        {
            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Proveedor eliminado correctamente.";
        }
        else
        {
            TempData["ErrorMessage"] = "Proveedor no encontrado.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProveedorExists(int id)
    {
        return _context.Proveedores.Any(e => e.ProveedorId == id);
    }
}