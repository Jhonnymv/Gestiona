using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna
{
    [Table("Tb_Usuarios_Notificar_Avistamiento_Fauna")]
    public class Usuario_Notificar_Avistamiento_Fauna
    {
        [Key]
        public int Id_Usuarios_Notificar_Avistamiento_Fauna { get; set; }
        [Column(TypeName = "int")]
        public int Id_Rol { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Nombres { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Apellidos { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string usuario_red { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(3)")]
        public string Estado { get; set; }
    }
}
