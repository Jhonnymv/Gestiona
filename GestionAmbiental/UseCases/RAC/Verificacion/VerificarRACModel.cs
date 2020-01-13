using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.RAC.Verificacion
{
    public class VerificarRACModel
    {
        public int Id_RAC { get; set; }
        public string IdEncriptado { get; set; }
        [DisplayName("Nombres y Apellidos:")]
        public string Nombres_Apellidos { get; set; }
        [DisplayName("Área de Marcobre:")]
        public string Gerencia { get; set; }
        [DisplayName("Empresa:")]
        public string Empresa { get; set; }
        [DisplayName("Otra empresa:")]
        public string Otra_Empresa { get; set; }
        [DisplayName("Empresa observada:")]
        public string Empresa_Obs { get; set; }
        [DisplayName("Gerencia a la que pertenece (Marcobre):")]
        public string Gerencia_Obs { get; set; }
        [DisplayName("Ubicación de la observación:")]
        public string Ubicacion { get; set; }
        [DisplayName("Fecha:")]
        public string Fecha { get; set; }
        [DisplayName("Hora (24h):")]
        public string Hora { get; set; }
        [DisplayName("Observación SSO/MA:")]
        public string SSO_MA { get; set; }
        [DisplayName("Clasificación:")]
        public string Clasificacion { get; set; }
        [DisplayName("Nivel de Riesgo:")]
        public string NivelRiesgo { get; set; }
        [DisplayName("Tipo:")]
        public string Tipo_Condicion { get; set; }
        [DisplayName("Descripción del acto o condición subestándar:")]
        public string Descripcion_Acto_Cond { get; set; }
        [DisplayName("Medida(s) de control inmediata:")]
        public string Medida_Control { get; set; }
        [DisplayName("¿Se planteó una primera acción correctiva?:")]
        public string PAC { get; set; }
        [DisplayName("Responsable de Marcobre del cierre de la observación:")]
        public string Responsable_Cierre { get; set; }
        [DisplayName("Fecha de cierre programada:")]
        public string Fecha_Cierre { get; set; }
        [DisplayName("Verificador de SSO/MA (Marcobre) a notificar:")]
        public string Supervisor { get; set; }
        [DisplayName("Evidencia de la Observación:")]
        public string Evidencia_Obs { get; set; }
        [DisplayName("Evidencia del levantamiento de observación (Cierre de la acción correctiva):")]
        public string Evidencia_Cierre { get; set; }
        [DisplayName("Comentario del levanamiento de la observación:")]
        public string Comentario_Cierre { get; set; }
        [DisplayName("Comentario de la verificación de la observación:")]
        public string Comentario_Verificacion { get; set; }
        public string Opcion { get; set; }
        public string Estado { get; set; }
    }
}
