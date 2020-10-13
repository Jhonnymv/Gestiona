using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class FuenteAguaModel
    {
        public int IdFuenteAgua { get; set; }
        [DisplayName("Origen de Agua:")]
        public int IdOrigenAgua { get; set; }
        [DisplayName("Descripción:")]
        public string Descripcion { get; set; }
        [DisplayName("Coordenadas UTM:")]
        public string Coordenadas_UTM { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
