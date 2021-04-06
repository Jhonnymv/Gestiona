using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Home
{
    public class HomeModel
    {
        public List<TarjetaModel> Tarjetas { get; set; }
        public ConfiguracionesTarjeta Configuraciones { get; set; }
    }
    public class TarjetaModel
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string Tipo { get; set; }
        public string LugarImg { get; set; }
        public int? AnchoImg { get; set; }
    }
    public class ConfiguracionesTarjeta
    {
        public List<TipoTarjetaModel> TiposTarjeta { get; set; }
        public List<string> PosicionesImagen { get; set; }
        public List<AnchoImagenModel> AnchosImagen { get; set; }
    }
    public class AnchoImagenModel
    {
        public string Porcentaje { get; set; }
        public int Ancho { get; set; }
    }
    public class TipoTarjetaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TamanioColumna Caracteristicas { get; set; }
    }
    public class TamanioColumna
    {
        public int SM { get; set; }
        public int MD { get; set; }
        public int LG { get; set; }
        public int XL { get; set; }
    }
}
