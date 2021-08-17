using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Configuracion
{
    public class SlideModel
    {
            public int Id { get; set; }
            [DisplayName("Título del Slide")]
            public string Titulo { get; set; }
            [DisplayName("Contenido del Slide")]
            public string Contenido { get; set; }
            [DisplayName("Imagen del Slide")]
            public IFormFile ImagenNueva { get; set; }
            [DisplayName("Imagen Actual")]
            public string Imagen { get; set; }
        
    }
}
