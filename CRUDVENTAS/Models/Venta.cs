using CRUDVENTAS.Models;
using System.ComponentModel.DataAnnotations.Schema;



namespace CRUDVENTAS.Models
{
    public class Venta
    {

        //Propiedad que representa el identificador único de la venta

        public int VentaId { get; set; }

        //Propiedad que representa la fecha de la venta

        public DateTime Fecha { get; set; }

        //Propiedad que representa el total de la venta, especificando el
        //tipo de columna en la base de datos

        [Column(TypeName = "decimal(18,2)")] //Define el tipo de dato en la base de datos
        //será decimal con 18 dígitos en total y 2 decimales

        public decimal Total { get; set; }

        //Propiedad que representa el ID del cliente asociado a la venta

        public int ClienteId { get; set; }

        //Propiedad de navegación que permite acceder a la información del cliente relacionado

        public Cliente? Cliente { get; set; }  //Puede ser nulo (nullable)



        //Propiedad que representa el estado de la venta, puede ser
        //"Realizada" o "Anulada"
        public string? Estado { get; set; } //Puede ser nulo (nullable)

        //Lista que contiene los detalles de la venta, inicializada como una nueva lista

        public List<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();




    }
}
