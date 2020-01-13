using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.DescargarRAC
{
    public interface IGetRACDescarga
    {
        Task<RACOutput> Execute(int Id_RAC);
    }
}
