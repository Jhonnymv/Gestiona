using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarEvidenciaManifiestoRS
{
    public interface IConsultarEvidenciaManifiestoRS
    {
        Task<string[]> Execute(int IdManifiesto, string Tipo);
    }
}
