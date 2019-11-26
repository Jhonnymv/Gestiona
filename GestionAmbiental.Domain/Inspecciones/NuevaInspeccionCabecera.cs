using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class NuevaInspeccionCabecera
    {
        public int Id_Cabecera { get; set; }
        public int Id_Registrado_Por { get; set; }
        public string Codigo { get; set; }
        public int Id_Gerencia_Ejecutora { get; set; }
        public int Id_Empresa_Contratista { get; set; }
        public DateTime Fecha_Inspeccion { get; set; }
        public string Lugar_Inspeccionado { get; set; }
        public string Tipo_Inspeccion { get; set; }
        public int Id_Responsable_Lugar { get; set; }
        public string Responsable_Inspeccion { get; set; }
        public string Inspectores { get; set; }
        public string Objetivo { get; set; }
        public string Estado { get; set; }
                
        public NuevaInspeccionCabecera()
        {

        }
    }
}
