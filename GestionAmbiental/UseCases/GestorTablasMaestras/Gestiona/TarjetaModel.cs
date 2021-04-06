using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class TarjetaModel
    {
        public int Id { get; set; }
        [DisplayName("Orden:")]
        public int Orden { get; set; }
        [DisplayName("Título:")]
        public string Titulo { get; set; }
        [DisplayName("Descripción:")]
        public string Descripcion { get; set; }
        [DisplayName("Cargar Imagen:")]
        public IFormFile ImagenNueva { get; set; }
        [DisplayName("Imagen Actual:")]
        public string Imagen { get; set; }
        [DisplayName("Tamaño de Tarjeta:")]
        public string Tipo { get; set; }
        [DisplayName("Ubicación de la Imagen:")]
        public string LugarImg { get; set; }
        [DisplayName("Ancho de la Imagen:")]
        public int? AnchoImg { get; set; }
    }
}
