using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Home
{
    public class HomeModel
    {
        public List<TarjetaModel> Tarjetas { get; set; }
    }
    public class TarjetaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string Imagen { get; set; }
        public string Publicador { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public int Tamanio { get; set; }
        public int PosicionE{ get; set; }
        public int PosicionM{ get; set; }
    }
}
