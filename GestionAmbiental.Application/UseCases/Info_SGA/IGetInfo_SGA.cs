using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA
{
    public interface IGetInfo_SGA
    {
        Task<Info_SGAOutput> Execute(Tipo_Info_SGA tipo);
    }
}
