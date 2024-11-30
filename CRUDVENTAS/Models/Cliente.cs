using System.ComponentModel.DataAnnotations;


namespace CRUDVENTAS.Models
{
    public class Cliente
    {

        public int ClienteId { get; set; }
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "¡El nombre no puede exceder los 100 caracteres!")]
        //Validación para permitir solo espacios y letras

        [RegularExpression(@"^[a-zA-z\s]+$", ErrorMessage = "El nombre solo puede tener letras y espacios")]

        public required string Nombre { get; set; }
        [Required(ErrorMessage = "La dirección del cliente es obligatoria")]
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        [Required(ErrorMessage = "El documento del cliente es obligatorio")]

        public required int Documento { get; set; }


    }


}
