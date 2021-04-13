using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class TarjetaModel
    {
        public int Id { get; set; }
        [DisplayName("Orden:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int Orden { get; set; }
        [DisplayName("Título:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Titulo { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Cargar Imagen:")]
        public IFormFile ImagenNueva { get; set; }
        [DisplayName("Imagen Actual:")]
        public string Imagen { get; set; }
        [DisplayName("Tamaño de Tarjeta:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string Tipo { get; set; }
        [DisplayName("Ubicación de la Imagen:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar una opción")]
        public string LugarImg { get; set; }
        [DisplayName("Ancho de la Imagen:")]
        public int? AnchoImg { get; set; }
    }
}
