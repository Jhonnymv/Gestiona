using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminarMA
{
    public class EditarReporteModel
    {
        public int Id_Reporte_Preliminar { get; set; }
        [DisplayName("Código:")]
        public string Codigo { get; set; }
        [DisplayName("N° Trabajadores en el Centro Laboral:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int Num_Trabajadores { get; set; }
        [DisplayName("Razón Social:")]
        public string Razon_Social_Contratista { get; set; }
        [DisplayName("RUC:")]
        public string RUC_Contratista { get; set; }
        [DisplayName("Actividad:")]
        public string Actividad_Contratista { get; set; }
        [DisplayName("Domicilio Legal:")]
        public string Domicilio_Contratista { get; set; }
        [DisplayName("N° Trabajadores en el Centro Laboral:")]
        public int? Num_Trabajadores_Contratista { get; set; }
        [DisplayName("Tipo de incidente:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Id_Tipo_Incidente { get; set; }
        [DisplayName("Nivel de incidente:")]
        public int Id_Nivel_Incidente { get; set; }
        [DisplayName("Fecha:")]
        public string Fecha_Incidente { get; set; }
        [DisplayName("Hora:")]
        public string Hora_Incidente { get; set; }
        [DisplayName("Producto Derramado:")]
        public string Producto_Derramado { get; set; }
        [DisplayName("Cantidad aprox. derramada:")]
        public double? Cantidad_Derramado { get; set; }
        public int Id_Unidad_Cantidad_Derramado { get; set; }
        [DisplayName("Área afectada:")]
        public double? Area_Afectada { get; set; }
        public int Id_Unidad_Area_Afectada { get; set; }
        [DisplayName("Lugar donde ocurrió:")]
        public string Lugar_Incidente { get; set; }
        [DisplayName("Equipo/instalación involucrado:")]
        public string Equipo_Involucrado { get; set; }
        [DisplayName("Componente Ambiental afectado:")]
        public int Id_Componente_Ambiental { get; set; }
        [DisplayName("Descripción preliminar del incidente:")]
        public string Descripcion_Incidente { get; set; }
        [DisplayName("Escribir otra causa inmediata:")]
        public string Otra_Causa_Incidente { get; set; }
        [DisplayName("Gerencia Responsable (Titular Minero):")]
        public int Id_Gerencia_Responsable { get; set; }
        [DisplayName("Comentario:")]
        public string Comentario_Accion { get; set; }
        [DisplayName("Supervisor de Medio Ambiente encargado del seguimiento:")]
        public int Id_Supervisor { get; set; }
        [DisplayName("Evidencias:")]
        public List<IFormFile> Evidencias { get; set; }
        public string Evidencias_Completas { get; set; }
        [DisplayName("Iniciar Investigación  ")]
        public bool Investigacion { get; set; }
        public int NInvolucrados { get; set; }
        public int NAcciones { get; set; }
        public int NCausas { get; set; }
        public string Estado { get; set; }
        public string Opcion { get; set; }
    }
}
