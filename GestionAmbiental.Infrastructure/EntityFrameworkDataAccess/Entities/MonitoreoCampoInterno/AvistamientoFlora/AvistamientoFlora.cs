using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.MonitoreoCampoInterno.AvistamientoFlora
{
    [Table("Tb_MCIF_Avistamiento_Flora")]
    public class AvistamientoFlora
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varhar(255)")]
        public string IdFicha { get; set; }
        [Column(TypeName = "Date")]
        public DateTime FechaValidacion { get; set; }
        [Column(TypeName = "Int")]
        public int Id_Zona { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Ubicacion_Este { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Ubicacion_Norte { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Ubicacion_Altitud { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Geneto { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Rameto { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Condicion_Reubicacion { get; set; }
        [Column(TypeName = "Varchar(255)")]
        public string Sustrato { get; set; }
        [Column(TypeName = "Decimal(8,2)")]
        public decimal Cobertura_Diametro_Mayor { get; set; }
        [Column(TypeName = "Decimal(8,2)")]
        public decimal Cobertura_Diametro_Menor { get; set; }
        [Column(TypeName = "Decimal(8,2)")]
        public decimal Cobertura_Diametro_Altura { get; set; }
        [Column(TypeName = "Text")]
        public string? Fotos { get; set; }
    }
}
