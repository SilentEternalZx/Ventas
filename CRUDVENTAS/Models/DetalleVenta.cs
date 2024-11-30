using CRUDVENTAS.Models;
using System.ComponentModel.DataAnnotations.Schema;



namespace CRUDVENTAS.Models
{
    public class DetalleVenta
    {

        //Identificador único para cada detalle de venta

        public int DetalleVentaId { get; set; }

        //Identificador de venta a la que pertenece este detalle

        public int VentaId { get; set; }


        //Navegación a la entidad Venta, permite acceder a la información de la venta relacionada
        public Venta? Venta { get; set; }


        //Identificador del producto asociado a este detalle de venta
        public int ProductoId { get; set; }

        //Navegación a la entidad del producto, permite acceder a la información 
        //del producto relacionado


        public Producto? Producto { get; set; }


        //Cantidad del producto vendido
        public int Cantidad { get; set; }

        //Especificar el tipo de columna con precisión y escala para el precio

        [Column(TypeName = "decimal(18,2)")] //Define que el tipo de dato en la base de datos
        //será decimal con 18 dígitos en total y 2 decimales
        public decimal Precio { get; set; }

        //Especificar el tipo de columna con precisión y escala para el subtotal

        [Column(TypeName = "decimal(18,2)")]  //Igual que el anterior, pero para el subtotal
        public decimal SubTotal { get; set; }

    }
}
