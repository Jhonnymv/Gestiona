using GestionAmbiental.Domain.AvistamientoFauna.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna
{
    public class AvistamientoFauna
    {
        public int IdAvistamientoFauna { get; set; }
        public string Codigo { get; set; }
        public string Fotos { get; set; }
        public DateTime Fecha_Avistamiento { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public NombreComunFauna NombreComun { get; set; }
        public TipoAvistamiento TipoAvistamiento { get; set; }
        public EstadoFauna EstadoFauna { get; set; }
        public string Lugar_Referencia { get; set; }
        public Usuario Usuario { get; set; }
        public Empresa Empresa { get; set; }
        public AreaTrabajo AreaTrabajo { get; set; }
        public string NFotocheck { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Evidencia_Accion { get; set; }
        public Usuario Supervisor { get; set; }
        public string Estado { get; set; }
    }
}
