using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReport
{
    public interface IGetFlashReport
    {
        Task<FlashReportOutput> Execute(int Id);
    }
}
