using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinalMA
{
    public class EditarInvestigacionModel
    {
        public string Id_Encriptado_Investigacion_Final { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public int? Id_Reporte_Preliminar { get; set; }
        [DisplayName("Código:")]
        public string Codigo { get; set; }
        [DisplayName("N° Trabajadores en el Centro Laboral:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public int? Num_Trabajadores { get; set; }
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
        [DisplayName("Razón Social:")]
        public string Razon_Social_SubContratista { get; set; }
        [DisplayName("RUC:")]
        public string RUC_SubContratista { get; set; }
        [DisplayName("Actividad:")]
        public string Actividad_SubContratista { get; set; }
        [DisplayName("Domicilio Legal:")]
        public string Domicilio_SubContratista { get; set; }
        [DisplayName("N° Trabajadores en el Centro Laboral:")]
        public int? Num_Trabajadores_SubContratista { get; set; }
        [DisplayName("Tipo de incidente:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Id_Tipo_Incidente { get; set; }
        [DisplayName("Nivel de incidente:")]
        public int Id_Nivel_Incidente { get; set; }
        [DisplayName("Lugar donde ocurrió:")]
        public string Lugar_Incidente { get; set; }
        [DisplayName("Este:")]
        public string Coordenada_Este { get; set; }
        [DisplayName("Norte:")]
        public string Coordenada_Norte { get; set; }
        [DisplayName("Localidad/Zona/Sector:")]
        public string Localidad { get; set; }
        [DisplayName("Distrito:")]
        public string Distrito { get; set; }
        [DisplayName("Provincia:")]
        public string Provincia { get; set; }
        [DisplayName("Departamento:")]
        public string Departamento { get; set; }
        [DisplayName("Fecha del Incidente:")]
        public string Fecha_Incidente { get; set; }
        [DisplayName("Hora del Incidente:")]
        public string Hora_Incidente { get; set; }
        [DisplayName("Fecha de Incio de Investigación:")]
        public string Fecha_Inicio_Investigacion { get; set; }
        [DisplayName("Turno:")]
        public string Turno { get; set; }
        [DisplayName("Descripción detallada del incidente:")]
        public string Descripcion_Incidente { get; set; }
        [DisplayName("Gerencia Responsable (Titular Minero):")]
        public int Id_Gerencia_Responsable { get; set; }
        [DisplayName("Supervisor de Medio Ambiente encargado del seguimiento:")]
        public int Id_Supervisor { get; set; }
        [DisplayName("Evidencias Papel:")]
        public List<IFormFile> Evidencias_Papel { get; set; }
        [DisplayName("Evidencias Persona:")]
        public List<IFormFile> Evidencias_Persona { get; set; }
        [DisplayName("Evidencias Posición:")]
        public List<IFormFile> Evidencias_Posicion { get; set; }
        [DisplayName("Evidencias Partes:")]
        public List<IFormFile> Evidencias_Partes { get; set; }
        public string Evidencias_Completas_Papel { get; set; }
        public string Evidencias_Completas_Persona { get; set; }
        public string Evidencias_Completas_Posicion { get; set; }
        public string Evidencias_Completas_Partes { get; set; }
        public string Evidencias_Nombres { get; set; }
        public ImpactoAmbientalModel ImpactoAmbiental { get; set; }
        public int NInvolucrados { get; set; }
        public int NProductos { get; set; }
        public int NAcciones { get; set; }
        public int NCausas { get; set; }
        public string Estado { get; set; }
        public string Opcion { get; set; }
        public int? Id_Registrado_Por { get; set; }
    }
}
