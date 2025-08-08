using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionesFlashReport
{
    public interface IGetAccionesFlashReport
    {
        Task<AccionesFlashReportOutput> Execute(int Id_Flash_Report);
    }
}
