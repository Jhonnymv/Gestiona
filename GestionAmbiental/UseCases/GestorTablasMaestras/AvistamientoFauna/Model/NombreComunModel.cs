using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna.Model
{
    public class NombreComunModel
    {
        public int IdNombreComun { get; set; }
        [DisplayName("Descripción:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Descripcion { get; set; }
        [DisplayName("Tipo de Fauna:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public int IdTipoFauna { get; set; }
        [DisplayName("Foto:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe cargar un archivo")]
        [DataType(DataType.Upload)]
        public IFormFile Foto { get; set; }
        [DisplayName("Foto:")]
        public string RutaFoto { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
