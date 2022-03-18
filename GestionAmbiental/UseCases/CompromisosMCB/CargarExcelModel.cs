using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class CargaExcelModel
    {
        [DisplayName("Archivo Excel:")]
        [Required(ErrorMessage = "Debe cargar un archivo Excel")]
        public IFormFile Excel { get; set; }
        [DisplayName("Nombre de la página en la que se encuentran los datos Excel:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Pagina { get; set; }
        [DisplayName("Número de la primera fila de los datos:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int FilaInicio { get; set; }
        [DisplayName("Número de la columna de códigos:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int ColumnaInicio { get; set; }
    }
}
