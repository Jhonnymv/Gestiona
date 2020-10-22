using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class CargaExcelModel
    {
        [DisplayName("Archivo Excel:")]
        public IFormFile Excel { get; set; }
        [DisplayName("Nombre de la página en la que se encuentran los datos Excel:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Pagina { get; set; }
        [DisplayName("Número de la primera fila de los datos:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int FilaInicio { get; set; }
        [DisplayName("Número de la columna de fechas:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int ColumnaInicio { get; set; }
        [DisplayName("Actividad a la cual pertenecen los datos:")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int IdResolucion { get; set; }
        [DisplayName("Resolución a la cual pertenecen los datos:")]
        public string Resolucion { get; set; }
    }
}
