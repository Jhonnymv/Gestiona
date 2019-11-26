using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Empresa_Contratista")]
    public class Empresa_Contratista
    {
        [Key]
        public int Id_Empresa_Contratista { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Razon_Social { get; set; }
    }
}
