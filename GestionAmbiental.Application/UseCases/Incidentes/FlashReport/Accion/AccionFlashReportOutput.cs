using GestionAmbiental.Domain.Incidentes.FlashReport;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion
{
    public class AccionFlashReportOutput
    {
        public int Id_Accion_Flash_Report { get; set; }
        public int Id_Flash_Report { get; set; }
        //public Flash_Report? FlashReport { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public string Nombres_Usuario { get; set; }
        //public Usuario? Usuario { get; set; }
        public string Responsable { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; }
        public string Estado { get; set; }

        public AccionFlashReportOutput(Domain.Incidentes.FlashReport.Accion_Flash_Report accion)
        {
            Id_Accion_Flash_Report = accion.Id_Accion_Flash_Report;
            Descripcion = accion.Descripcion;
            Id_Flash_Report = accion.Id_Flash_Report;
            IdUsuario = accion.Id_Usuario;
            Nombres_Usuario = accion.Usuario?.Nombres + " " + accion.Usuario?.Apellidos;
            Responsable = accion.Responsable;
            Fecha = accion.Fecha;
            Estatus = accion.Estatus;
            Estado = accion.Estado;
        }
    }
}
