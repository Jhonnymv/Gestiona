using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObsxEstado
{
    public class DetalleObsxEstadoOutput
    {
        public int Id_Cabecera { get; set; }
        public string Id_Cabecera_Encriptada { get; set; }
        public string Codigo { get; set; }
        public string Empresa { get; set; }
        public string Gerencia { get; set; }
        public string Fecha_Inspeccion { get; set; }
        public string Tipo_Observacion { get; set; }
        public string Asesor { get; set; }
        public string Tipo_Condicion { get; set; }
        public string Nivel_Riesgo { get; set; }
        public string Responsable { get; set; }
        public string Fecha_Programada { get; set; }
    }
}
