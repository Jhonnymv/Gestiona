using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Investigacion_Final_Incidente
    {
        public int Id_Investigacion_Final { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public string Codigo { get; set; }
        //Titular minero
        public int Num_Trabajadores { get; set; }
        //Contratista - opcional
        public string Razon_Social_Contratista { get; set; }
        public string RUC_Contratista { get; set; }
        public string Actividad_Contratista { get; set; }
        public string Domicilio_Contratista { get; set; }
        public int? Num_Trabajadores_Contratista { get; set; }
        public string Razon_Social_Subcontratista { get; set; }
        public string RUC_Subcontratista { get; set; }
        public string Actividad_Subcontratista { get; set; }
        public string Domicilio_Subcontratista { get; set; }
        public int? Num_Trabajadores_Subcontratista { get; set; }
        //Info Incidente
        public Tipo_Incidente Tipo_Incidente { get; set; }
        public Nivel_Incidente Nivel_Incidente { get; set; }
        public string Lugar_Incidente { get; set; }
        public string Coordenada_Este { get; set; }
        public string Coordenada_Norte { get; set; }
        public string Localidad { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public DateTime? Fecha_Incidente { get; set; }
        public DateTime? Fecha_Inicio_Investigacion { get; set; }
        public string Turno { get; set; }
        public string Descripcion_Incidente { get; set; }
        //Involucrados
        public Gerencia Gerencia_Responsable { get; set; }
        public Usuario Supervisor_MA { get; set; }
        public string Evidencia_Papel { get; set; }
        public string Evidencia_Posicion { get; set; }
        public string Evidencia_Persona { get; set; }
        public string Evidencia_Parte { get; set; }
        public string Estado { get; set; }       
        public int NAccionesAprobadas { get; set; }
        public int NAcciones { get; set; }
    }
}
