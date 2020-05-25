using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.ImplementarAccion
{
    public interface IImplementarAccion
    {
        Task<int> Execute(int IdAccion,List<string> evidencias, string comentario);
    }
}
