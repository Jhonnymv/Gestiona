using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.RAC
{
    public class RACOutput
    {
        public int Id_RAC { get; set; }
        public string Codigo { get; set; }
        public string Nombres_Apellidos { get; set; }
        public int Id_Registrado_Por { get; set; }
        public int? Id_Gerencia { get; set; }
        public string Gerencia { get; set; }
        public int? Id_Empresa { get; set; }
        public string Empresa { get; set; }
        public string Otra_Empresa { get; set; }
        public int Id_Empresa_Obs { get; set; }
        public string Empresa_Ob { get; set; }
        public int Id_Gerencia_Obs { get; set; }
        public string Gerencia_Obs { get; set; }
        public string Ubicacion { get; set; }
        public DateTime Fecha { get; set; }
        public string SSO_MA { get; set; }
        public string Clasificacion { get; set; }
        public string NivelRiesgo { get; set; }
        public int Id_Tipo_Condicion { get; set; }
        public string TipoCondicion { get; set; }
        public string Descripcion_Acto_Cond { get; set; }
        public string Medida_Control { get; set; }
        public string PAC { get; set; }
        public int Id_Responsable_Cierre { get; set; }
        public string Responsable_Cierre { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public int Id_Supervisor { get; set; }
        public string Supervisor { get; set; }
        public string Evidencia_Obs { get; set; }
        public string Evidencia_Cierre { get; set; }
        public string Comentario_Cierre { get; set; }
        public string Comentario_Verificacion { get; set; }
        public string Estado { get; set; }
        public bool PermisoAdmin { get; set; }
        public bool PermisoAsesor { get; set; }
        public RACOutput(Domain.RAC.RAC rac)
        {
            Id_RAC = rac.Id_RAC;
            Codigo = rac.Codigo;
            Nombres_Apellidos = rac.Nombres_Apellidos;
            Id_Registrado_Por = rac.Id_Registrado_Por;
            Id_Gerencia = rac.Id_Gerencia;
            Gerencia = rac.Gerencia!=null? rac.Gerencia.Descripcion : "";
            Id_Empresa = rac.Id_Empresa;
            Empresa = rac.Empresa != null ? rac.Empresa.RazonSocial : "";
            Otra_Empresa = rac.Otra_Empresa ?? "";
            Id_Empresa_Obs = rac.Id_Empresa_Obs;
            Empresa_Ob = rac.Empresa_Ob != null ? rac.Empresa_Ob.RazonSocial : "";
            Id_Gerencia_Obs = rac.Id_Gerencia_Obs;
            Gerencia_Obs = rac.Gerencia_Obs != null ? rac.Gerencia_Obs.Descripcion : "";
            Ubicacion = rac.Ubicacion;
            Fecha = rac.Fecha;
            SSO_MA = rac.SSO_MA;
            Clasificacion = rac.Clasificacion;
            NivelRiesgo = rac.NivelRiesgo;
            Id_Tipo_Condicion = rac.Id_Tipo_Condicion;
            TipoCondicion = rac.TipoCondicion.Descripcion;
            Descripcion_Acto_Cond = rac.Descripcion_Acto_Cond;
            Medida_Control = rac.Medida_Control;
            PAC = rac.PAC;
            Id_Responsable_Cierre = rac.Id_Responsable_Cierre;
            Responsable_Cierre = rac.Responsable_Cierre.Nombres + " " + rac.Responsable_Cierre.Apellidos;
            Fecha_Cierre = rac.Fecha_Cierre;
            Id_Supervisor = rac.Id_Supervisor;
            Supervisor = rac.Supervisor.Nombres + " " + rac.Supervisor.Apellidos;
            Evidencia_Obs = rac.Evidencia_Obs;
            Evidencia_Cierre = rac.Evidencia_Cierre ?? ""; ;
            Comentario_Cierre = rac.Comentario_Cierre ?? "";
            Comentario_Verificacion = rac.Comentario_Verificacion ?? "";
            Estado = rac.Estado;            
        }
    }

}
