using GestionAmbiental.Domain.Incidentes.FlashReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.FlashReport
{
    public interface IAccionFlashReportReadOnlyRepository
    {
        Task<List<Accion_Flash_Report>> ListarAccionesFlash(int Id_Flash_Report);
        Task<Accion_Flash_Report> DetalleAccionFlashReport(int id);
        Task<int> EditarAccionFlashReport(Accion_Flash_Report accionflashReport);
        Task<int> RegistrarAccionFlashReport(Accion_Flash_Report accionflashReport);
    }
}
