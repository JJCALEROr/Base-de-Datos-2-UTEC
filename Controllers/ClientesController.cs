using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioVentasMVC.Models;

public class ClientesController : Controller
{
    private readonly InventarioContext _context;

    public ClientesController(InventarioContext context)
    {
        _context = context;
    }

    // GET: Clientes
    public async Task<IActionResult> Index()
    {
        return View(await _context.Clientes.ToListAsync());
    }

    // GET: Clientes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.ClienteId == id);

        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // GET: Clientes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Clientes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClienteId,Nombre,Apellido,DUI,Telefono,Email,Direccion,Activo,FechaCreacion")] Cliente cliente)
    {
        ModelState.Remove("Ventas");

        if (ModelState.IsValid)
        {
            try
            {
                cliente.FechaCreacion = DateTime.Now;

                _context.Add(cliente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cliente creado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
        }

        return View(cliente);
    }

    // GET: Clientes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // POST: Clientes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nombre,Apellido,DUI,Telefono,Email,Direccion,Activo,FechaCreacion")] Cliente cliente)
    {
        if (id != cliente.ClienteId)
        {
            return NotFound();
        }

        ModelState.Remove("Ventas");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cliente actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.ClienteId))
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

        return View(cliente);
    }

    // GET: Clientes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.ClienteId == id);

        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // POST: Clientes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cliente eliminado correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(int id)
    {
        return _context.Clientes.Any(e => e.ClienteId == id);
    }
}