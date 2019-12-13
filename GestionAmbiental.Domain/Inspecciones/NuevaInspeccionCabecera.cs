using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class NuevaInspeccionCabecera
    {
        public int IdCabecera { get; set; }
        public int IdRegistradoPor { get; set; }
        public string Codigo { get; set; }
        public int IdGerencia { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime FechaInspeccion { get; set; }
        public string LugarInspeccionado { get; set; }
        public string TipoInspeccion { get; set; }
        public int IdResponsableArea { get; set; }
        public string ResponsableInspeccion { get; set; }
        public string Inspectores { get; set; }
        public string Objetivo { get; set; }
        public string Estado { get; set; }
                
        public NuevaInspeccionCabecera()
        {

        }
    }
}
