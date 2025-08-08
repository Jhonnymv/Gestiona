using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Domain.Incidentes.FlashReport
{
    public class Accion_Flash_Report
    {
        public int Id_Accion_Flash_Report { get; set; }
        public int Id_Flash_Report { get; set; }
        public Flash_Report? FlashReport { get; set; }
        public string Descripcion { get; set; }
        public int Id_Usuario { get; set; }
        public Usuario? Usuario { get; set; }
        public string Responsable { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; }
        public string Estado { get; set; }
    }
}
