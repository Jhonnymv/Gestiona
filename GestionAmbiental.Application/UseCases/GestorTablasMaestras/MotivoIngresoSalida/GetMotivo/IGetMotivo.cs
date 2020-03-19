using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivo
{
    public interface IGetMotivo
    {
        Task<MotivoOutput> Execute(int idMotivo);
    }
}
