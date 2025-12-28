using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.EliminarFlashReport
{
    public interface IEliminarFlashReport
    {
        Task<int> Execute(int IdFlashReport);
    }
}
