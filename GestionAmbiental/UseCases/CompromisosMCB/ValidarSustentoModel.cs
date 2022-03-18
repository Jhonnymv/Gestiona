using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class ValidarSustentoModel
    {
        public int Id { get; set; }
        [DisplayName("Fecha Validación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar la fecha")]
        public DateTime Fecha_validacion { get; set; }

        public int Id_acesor { get; set; }

        [DisplayName("Comentario Verificación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar su Sustento")]
        public string Comentario_verificacion { get; set; }

    
        [DisplayName("Medio Verificación:")]
        public string Comentario_implementacion { get; set; }

    }
}
