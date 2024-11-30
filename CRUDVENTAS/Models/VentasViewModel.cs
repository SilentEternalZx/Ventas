using System.ComponentModel.DataAnnotations; //Importa el espacio de nombres para usar atributos
                                             //de validación

using System.ComponentModel.DataAnnotations.Schema; //Importa el espacio de nombres para usar
//atributos de mapeo de columnas


namespace CRUDVENTAS.Models  //Define el espacio de nombres para la
                                      //aplicación de gestión de ventas
{

    //Clase que representa el modelo de vista para una venta
    public class VentasViewModel
    {

        //Propiedad que representa el ID del cliente, es obligatoria
        [Required]
        public int ClienteId { get; set; }

        //Propiedad que representa la fecha de la venta, 
        //se inicializa con la fecha y hora actuales

        public DateTime Fecha { get; set; } = DateTime.Now;


        //Propiedad opcional que representa el ID de la venta, puede ser nula

        public int? VentaId { get; set; }

        //Propiedad opcional que representa el estado de la venta
        public string? Estado { get; set; }


        //Propiedad que representa el total de la venta, especificando 
        //el tipo de columna en la base de datos

        [Column(TypeName = "decimal(18,2)")] //Defini el tipo de dato
        //en la base de datos será decimal con 18 dígitos en total y 2 decimales

        public decimal Total { get; set; }

        //Lista que contiene los detalles de la venta, inicializada
        //como una nueva lista

        //Asegúrate de que esta lista esté preparada para recibir múltiples detalles

        public List<DetalleVentaViewModel> Detalles { get; set; } = new List<DetalleVentaViewModel>();


    }
    //Clase que representa el modelo de vista para los detalles de una venta
    //
    public class DetalleVentaViewModel
    {
        //Propiedad que representa el ID del producto, es obligatoria
        [Required]
        public int ProductoId { get; set; }

        //Propiedad que representa la cantidad del producto vendido, es obligatoria

        [Required]
        public int Cantidad { get; set; }


        //Propiedad que representa el precio del producto, es obligatoria y especifica el tipo de columna

        [Required]
        [Column(TypeName = "decimal(18,2)")] //Defini que el tipo de dato
        //en la base de datos será decimal con 18 dígitos en total y 2 decimales

        public decimal Precio { get; set; }

        //Propiedad que representa el subtotal de este detalle de venta, es obligatoria y especifica el tipo de columna


        [Required]
        [Column(TypeName = "decimal(18,2)")] //Igual que el anterior, pero para el subtotal
        public decimal Subtotal { get; set; }
    }
}
