using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetObservacionesXEstado
{
    public interface IGetObservacionesXEstado
    {
        Task<List<DimensionOutput>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario);
    }
}
