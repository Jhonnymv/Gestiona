using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.Incidentes.InvestigacionFinal
{
    [Table("Tb_Investigacion_Final_Incidente")]
    public class Investigacion_Final_Incidente
    {
        [Key]
        public int Id_Investigacion_Final { get; set; }
        [Column(TypeName = "int")]
        public int Id_Reporte_Preliminar { get; set; }
        [Column(TypeName = "varhcar(10)")]
        public string Codigo { get; set; }
        //Titular minero
        [Column(TypeName = "int")]
        public int Num_Trabajadores { get; set; }
        //Contratista - opcional
        [Column(TypeName = "varhcar(250)")]
        public string Razon_Social_Contratista { get; set; }
        [Column(TypeName = "varhcar(11)")]
        public string RUC_Contratista { get; set; }
        [Column(TypeName = "varhcar(100)")]
        public string Actividad_Contratista { get; set; }
        [Column(TypeName = "varhcar(250)")]
        public string Domicilio_Contratista { get; set; }
        [Column(TypeName = "int")]
        public int? Num_Trabajadores_Contratista { get; set; }
        //Subcontratista - opcional
        [Column(TypeName = "varchar(250)")]
        public string Razon_Social_Subcontratista { get; set; }
        [Column(TypeName = "varchar(11)")]
        public string RUC_Subcontratista { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Actividad_Subcontratista { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Domicilio_Subcontratista { get; set; }
        [Column(TypeName = "int")]
        public int? Num_Trabajadores_Subcontratista { get; set; }
        //Info Incidente
        [Column(TypeName = "int")]
        public int? Id_Tipo_Incidente { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Nivel_Incidente { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Lugar_Incidente { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Coordenada_Este { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Coordenada_Norte { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Localidad { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Distrito { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Provincia { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Departamento { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Incidente { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Inicio_Investigacion { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Turno { get; set; }
        [Column(TypeName = "varchar(1500)")]
        public string Descripcion_Incidente { get; set; }
        //Involucrados
        [Column(TypeName = "int")]
        public int? Id_Gerencia_Responsable { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Supervisor_MA { get; set; }        
        [Column(TypeName = "text")]
        public string Evidencia_Papel { get; set; }   
        [Column(TypeName = "text")]
        public string Evidencia_Persona { get; set; }   
        [Column(TypeName = "text")]
        public string Evidencia_Posicion { get; set; }   
        [Column(TypeName = "text")]
        public string Evidencia_Parte { get; set; }        
        [Column(TypeName = "varchar(20)")]
        public string Estado { get; set; }
        [Column(TypeName = "text")]
        public string Nombres_Archivos { get; set; }

    }
}
