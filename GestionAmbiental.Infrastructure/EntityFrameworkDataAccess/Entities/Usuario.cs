using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities
{
    [Table("Tb_Usuario")]
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }
        [Column(TypeName = "int")]
        public int Id_Rol { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string usuario_red { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Superior { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}