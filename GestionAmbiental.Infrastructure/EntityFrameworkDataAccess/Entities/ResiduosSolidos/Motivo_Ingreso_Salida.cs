using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    
    public class Motivo_Ingreso_Salida
    {
        [Key]
        public int Id_Motivo { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Descripcion { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Estado { get; set; }

    }
}
