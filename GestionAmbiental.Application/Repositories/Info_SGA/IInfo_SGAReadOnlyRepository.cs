using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Info_SGA
{
    public interface IInfo_SGAReadOnlyRepository
    {
        Task<Domain.ValueObjects.Info_SGA> GetInfo_SGA(string tipo);
    }
}
