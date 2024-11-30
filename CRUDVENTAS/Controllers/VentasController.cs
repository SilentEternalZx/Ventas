using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CRUDVENTAS.Data;
using CRUDVENTAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace WebEmpresa.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index() // Método para mostrar la lista de ventas
        {
            var applicationDbContext = _context.Venta.Include(v => v.Cliente); // Incluye la información del cliente
            return View(await applicationDbContext.ToListAsync()); // Devuelve la vista con la lista de ventas
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)//Verifica si el ID es nulo
            {
                return NotFound();//Devuelve un error 404
            }
            //Obtener la venta con los detalles y el cliente
            var venta = await _context.Venta
                .Include(v => v.Cliente)//Incluye la información del cliente
                .Include(v => v.DetalleVenta)//Incluye los detalles de la venta
                .ThenInclude(dv => dv.Producto)//Incluye el producto en los detalles
                .FirstOrDefaultAsync(m => m.VentaId == id);//Busca la venta por ID
            if (venta == null)//Si no se encuentra la venta
            {
                return NotFound();//Devuelve un error 404
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create() // Método para mostrar el formulario de creación de ventas
        {
            // Cargar datos de Clientes y Productos para la vista
            ViewBag.Clientes = _context.Cliente.ToList(); // Lista de clientes
            ViewBag.Productos = _context.Producto.ToList(); // Lista de productos
            return View(); // Devuelve la vista de creación
        }

        [HttpPost] // Indica que este método maneja solicitudes POST
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Create(VentasViewModel model) // Método para procesar la creación de una venta
        {
            if (ModelState.IsValid) // Verifica si el modelo es válido
            {


                decimal total = model.Total; // El total debe ser recibido desde el formulario

                // Asegurarse de que el valor total está en el formato correcto
                total = decimal.Parse(total.ToString("0.00", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

                // Redondear el total para asegurarse de que tiene solo 2 decimales
                total = Math.Round(total, 2);

                // Procesar los detalles de la venta
                foreach (var detalle in model.Detalles) // Itera sobre los detalles de la venta
                {
                    detalle.Precio = Math.Round(detalle.Precio, 2); // Redondea el precio
                    detalle.Subtotal = Math.Round(detalle.Subtotal, 2); // Redondea el subtotal

                    // Descontar del stock
                    var producto = await _context.Producto.FindAsync(detalle.ProductoId); // Busca el producto
                    if (producto != null) // Si el producto existe
                    {
                        producto.Stock -= detalle.Cantidad; // Descontar la cantidad vendida
                    }
                }

                // Crear la venta
                var venta = new Venta
                {
                    ClienteId = model.ClienteId, // Asigna el ID del cliente
                    Fecha = DateTime.Now, // Establece la fecha actual
                    Total = total, // Asigna el total
                    Estado = "Realizada", // Establece estado como "Realizada"
                    DetalleVenta = model.Detalles.Select(d => new DetalleVenta // Crea los detalles de la venta
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        Precio = d.Precio,
                        SubTotal = d.Subtotal
                    }).ToList()
                };
                Console.WriteLine($"Total a guardar: {total}"); // Imprime el total en la consola
                _context.Venta.Add(venta); // Agrega la venta al contexto
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return Json(new { success = true, message = "Venta creada exitosamente" }); // Redirige a la lista de ventas
            }

            // Si la validación falla, volver a cargar la vista

            ViewBag.Clientes = await _context.Cliente.ToListAsync(); // Recarga la lista de clientes
            ViewBag.Productos = await _context.Producto.ToListAsync(); // Recarga la lista de productos
            return Json(new { success = false, message = "Error al crear el cliente: "  });
        }


        //Clase para recibir el estado desde la solicitud AJAX
        public class EstadoUpdateRequest
        {
            public string? Estado { get; set; }//Propiedad para almacenar el estado

        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody]
        EstadoUpdateRequest request) // Método para actualizar el estado de una venta
        {
            if (request == null || string.IsNullOrEmpty(request.Estado)) // Verifica si el estado es válido
            {
                return BadRequest("Estado no válido."); // Devuelve un error si no es válido
            }

            var venta = await _context.Venta.Include(v => v.DetalleVenta).FirstOrDefaultAsync(v => v.VentaId == id); // Busca la venta por ID
            if (venta == null) // Si no se encuentra la venta
            {
                return NotFound(); // Devuelve un error 
            }

            // Si el estado es "Anulada", devolver los productos al stock
            if (request.Estado == "Anulada")
            {
                foreach (var detalle in venta.DetalleVenta)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += detalle.Cantidad; // Aumenta el stock
                    }
                }
            }
            else if (request.Estado == "Realizada")
            {
                // Si el estado es "Realizada", descontar del stock
                foreach (var detalle in venta.DetalleVenta)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock -= detalle.Cantidad; // Disminuye el stock
                    }
                }
            }

            // Actualiza el estado
            venta.Estado = request.Estado; // Asigna el nuevo estado
            await _context.SaveChangesAsync(); // Guarda los cambios

            return Ok(); // Devuelve un resultado exitoso
        }



        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "ClienteId", venta.ClienteId);
            return View(venta);
        }


        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,Fecha,Total,ClienteId")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Venta actualizada exitosamente" });
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
                    {
                        return Json(new { success = false, message = "Venta no encontrada" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error de concurrencia" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al actualizar: " + ex.Message });
                }
            }
            var errors = ModelState.Values
               .SelectMany(v => v.Errors)
               .Select(e => e.ErrorMessage)
               .ToList();

            return Json(new { success = false, message = "Datos inválidos", errors });
        }

        private bool VentaExists(int ventaId)
        {
            throw new NotImplementedException();
        }


        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }


        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound(); // Devuelve una respuesta de "No encontrado" (404)
            }
            

            _context.Venta.Remove(venta);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}

