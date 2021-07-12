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
        [DisplayName("Título del Artículo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Titulo { get; set; }
        [DisplayName("Contenido del Artículo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Contenido { get; set; }
        [DisplayName("Imagen del Artículo")]
        public IFormFile ImagenNueva { get; set; }
        [DisplayName("Imagen Actual")]
        public string Imagen { get; set; }
        [DisplayName("Publicador del Artículo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Publicador { get; set; }
        [DisplayName("Fecha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        [DisplayName("Tamaño de la Tarjeta")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opción")]
        public int Tamanio { get; set; }
        [DisplayName("Posición del Contenido")]
        [Range(1, 99, ErrorMessage = "Debe seleccionar una opción")]
        public int PosicionE { get; set; }
        [DisplayName("Posición del Contenido")]
        public int PosicionM { get; set; }
    }
}
