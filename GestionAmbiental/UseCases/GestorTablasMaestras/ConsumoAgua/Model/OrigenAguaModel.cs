using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model
{
    public class OrigenAguaModel
    {
        public int IdOrigenAgua { get; set; }
        [DisplayName("Descripción:")]
        public string Descripcion { get; set; }
        [DisplayName("Estado:")]
        public string Estado { get; set; }
    }
}
