using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Tipo_Condicion")]
    public class Tipo_Condicion
    {
        [Key]
        public int Id_Tipo_Condicion { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Grupo { get; set; }
    }
}
