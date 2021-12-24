using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Test
{
    public class TestModel
    {
        public int Id { get; set; }

        [DisplayName("Nombre Test:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Nombre { get; set; }


        [DisplayName("Número Test:")]
        public int? Numero { get; set; }


        [DisplayName("Evidencia Test:")]
        public IFormFile Evidencia { get; set; }

    }
}
