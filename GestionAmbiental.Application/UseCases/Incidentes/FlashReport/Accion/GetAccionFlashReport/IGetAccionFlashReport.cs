using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionFlashReport
{
    public interface IGetAccionFlashReport
    {
        Task<AccionFlashReportOutput> Execute(int Id_Accion_Flash_Report);
    }
}
