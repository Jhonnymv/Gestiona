using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class ImplementarSustentoModel
    {
        public int Id { get; set; }
        [DisplayName("Fecha Entrega:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar la fecha")]
        public DateTime Fecha_entrega { get; set; }

        [DisplayName("Sustento de Cumplimiento:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar su Sustento")]
        public string Sustento_cumpliento { get; set; }

        [DisplayName("Documento")]
        public IFormFile Evidencia { get; set; }

        [DisplayName("Comentario implementación:")]
        public string Comentario_implementacion { get; set; }

    }
}
