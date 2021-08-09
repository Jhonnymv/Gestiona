using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.RAC
{
    public class RAC
    {
        public int Id_RAC { get; set; }
        public string Codigo { get; set; }
        public string Nombres_Apellidos { get; set; }
        public int Id_Registrado_Por { get; set; }
        public int? Id_Gerencia { get; set; }
        public Gerencia Gerencia { get; set; }
        public int? Id_Empresa { get; set; }
        public Empresa Empresa { get; set; }
        public string Otra_Empresa { get; set; }
        public int Id_Empresa_Obs { get; set; }
        public Empresa Empresa_Ob { get; set; }
        public int Id_Gerencia_Obs { get; set; }
        public Gerencia Gerencia_Obs { get; set; }
        public string Ubicacion { get; set; }
        public DateTime Fecha { get; set; }
        public string SSO_MA { get; set; }
        public string Clasificacion { get; set; }
        public string NivelRiesgo { get; set; }
        public int Id_Tipo_Condicion { get; set; }
        public TipoCondicion TipoCondicion { get; set; }
        public string Descripcion_Acto_Cond { get; set; }
        public string Medida_Control { get; set; }
        public string PAC { get; set; }
        public int Id_Responsable_Cierre { get; set; }
        public Usuario Responsable_Cierre { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public int Id_Supervisor { get; set; }
        public Usuario Supervisor { get; set; }
        public string Evidencia_Obs { get; set; }
        public string Evidencia_Cierre { get; set; }
        public string Comentario_Cierre { get; set; }
        public string Comentario_Verificacion { get; set; }
        public string Estado { get; set; }
    }
}
