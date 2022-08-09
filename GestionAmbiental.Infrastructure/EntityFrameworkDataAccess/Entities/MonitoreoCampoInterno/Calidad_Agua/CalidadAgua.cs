using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.Calidad_Agua
{
    [Table("Tb_MCI_Calidad_Agua")]
    public class CalidadAgua
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="Date")]
        public DateTime Fecha { get; set; }

        [Column(TypeName ="Varchar(255)")]
        public string Hora { get; set; }

        [Column(TypeName ="Varchar(255)")]
        public string Cod_Estacion { get; set; }

        [Column(TypeName ="Text")]
        public string Descripcion { get; set; }

        [Column(TypeName = "Text")]
        public string Ubicacion { get; set; }

        [Column(TypeName ="Int")]
        public int Id_Usuario { get; set; }

        [Column(TypeName = "Text")]
        public string TemperaturaResult { get; set; }

        [Column(TypeName = "Text")]
        public string TemperaturaEquipo { get; set; }

        [Column(TypeName = "Text")]
        public string PhResult { get; set; }

        [Column(TypeName = "Text")]
        public string PhEquipo { get; set; }

        [Column(TypeName = "Text")]
        public string ConductividadResult { get; set; }

        [Column(TypeName = "Text")]
        public string ConductividadEquipo { get; set; }

        [Column(TypeName = "Text")]
        public string OxigenoResult { get; set; }

        [Column(TypeName = "Text")]
        public string OxigenoEquipo { get; set; }

        [Column(TypeName = "Text")]
        public string TurbiedadResult { get; set; }

        [Column(TypeName = "Text")]
        public string TurbiedadEquipo { get; set; }

        [Column(TypeName = "Text")]
        public string CloroResult { get; set; }

        [Column(TypeName = "Text")]
        public string CloroEquipo { get; set; }
    }
}
