using InventarioVentasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProductosController : Controller
{
    private readonly InventarioContext _context;

    public ProductosController(InventarioContext context)
    {
        _context = context;
    }

    // GET: Productos
    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var productos = from p in _context.Productos
                        select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            productos = productos.Where(p => p.Nombre.Contains(searchString));
        }

        return View(await productos.ToListAsync());
    }

    // GET: Productos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.ProductoId == id);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // GET: Productos/Create
    public IActionResult Create()
    {
        ViewData["CategoriaId"] = new SelectList(_context.Categorias.ToList(), "CategoriaId", "Nombre");
        ViewData["ProveedorId"] = new SelectList(_context.Proveedores.ToList(), "ProveedorId", "RazonSocial");
        return View();
    }

    // POST: Productos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductoId,CategoriaId,ProveedorId,Nombre,Descripcion,PrecioCompra,PrecioVenta,Stock,StockMinimo,Activo,FechaCreacion")] Producto producto)
    {
        ModelState.Remove("Codigo");

        ModelState.Remove("Codigo");
        ModelState.Remove("FechaCreacion");
        ModelState.Remove("Categoria");
        ModelState.Remove("Proveedor");

        if (producto.PrecioVenta < producto.PrecioCompra)
        {
            ModelState.AddModelError("PrecioVenta",
                "El precio de venta no puede ser menor al precio de compra.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Generar correlativo para Codigo
                var ultimoProducto = await _context.Productos
                    .OrderByDescending(p => p.ProductoId)
                    .FirstOrDefaultAsync();

                string nuevoCodigo = "P001";
                if (ultimoProducto != null && !string.IsNullOrEmpty(ultimoProducto.Codigo))
                {
                    string numeroStr = ultimoProducto.Codigo.Substring(1);
                    if (int.TryParse(numeroStr, out int numero))
                    {
                        nuevoCodigo = "P" + (numero + 1).ToString("D3");
                    }
                }

                producto.Codigo = nuevoCodigo;
                producto.FechaCreacion = DateTime.Now;

                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
            }
        }

        ViewData["CategoriaId"] = new SelectList(_context.Categorias.ToList(), "CategoriaId", "Nombre", producto.CategoriaId);
        ViewData["ProveedorId"] = new SelectList(_context.Proveedores.ToList(), "ProveedorId", "RazonSocial", producto.ProveedorId);
        return View(producto);
    }

    // GET: Productos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }

        ViewData["CategoriaId"] = new SelectList(_context.Categorias.ToList(), "CategoriaId", "Nombre", producto.CategoriaId);
        ViewData["ProveedorId"] = new SelectList(_context.Proveedores.ToList(), "ProveedorId", "RazonSocial", producto.ProveedorId);

        return View(producto);
    }

    // POST: Productos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductoId,CategoriaId,ProveedorId,Codigo,Nombre,Descripcion,PrecioCompra,PrecioVenta,Stock,StockMinimo,Activo,FechaCreacion")] Producto producto)
    {
        if (id != producto.ProductoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.ProductoId))
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

        ViewData["CategoriaId"] = new SelectList(_context.Categorias.ToList(), "CategoriaId", "Nombre", producto.CategoriaId);
        ViewData["ProveedorId"] = new SelectList(_context.Proveedores.ToList(), "ProveedorId", "RazonSocial", producto.ProveedorId);

        return View(producto);
    }

    // GET: Productos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.ProductoId == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // POST: Productos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExists(int id)
    {
        return _context.Productos.Any(e => e.ProductoId == id);
    }
}
