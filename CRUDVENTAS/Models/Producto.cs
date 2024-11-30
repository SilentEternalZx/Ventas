
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CRUDVENTAS.Models
{
    public class Producto
    {

        public int ProductoId { get; set; }


        [Required(ErrorMessage = "¡El nombre del producto es obligatorio!")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres. ")]

        //Validación para permitir solo espacios y letras
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede tener letras y espacios ")]
        public required string Nombre { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]

        [Required(ErrorMessage = "¡El precio es obligatorio!")]
        public decimal Precio { get; set; }


        [Required(ErrorMessage = "¡El stock es obligatorio!")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0 ")]

        public int Stock { get; set; }


    }

}
