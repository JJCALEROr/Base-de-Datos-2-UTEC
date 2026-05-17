
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioVentasMVC.Models;

public class CategoriasController : Controller
{
    private readonly InventarioContext _context;

    public CategoriasController(InventarioContext context)
    {
        _context = context;
    }

    // GET: Categoria
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categorias.ToListAsync());
    }

    // GET: Categoria/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(m => m.CategoriaId == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: Categoria/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categoria/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CategoriaId,Nombre,Descripcion,Activo,FechaCreacion")] Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            try
            {
                categoria.FechaCreacion = DateTime.Now;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Categoría creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear: {ex.Message}";
            }
        }
        return View(categoria);
    }

    // GET: Categoria/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    // POST: Categoria/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre,Descripcion,Activo,FechaCreacion")] Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.CategoriaId))
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
                TempData["ErrorMessage"] = $"Error al actualizar: {ex.Message}";
            }
        }
        return View(categoria);
    }

    // GET: Categoria/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(m => m.CategoriaId == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: Categoria/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
        }
        else
        {
            TempData["ErrorMessage"] = "No se encontró la categoría.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(int id)
    {
        return _context.Categorias.Any(e => e.CategoriaId == id);
    }
}
