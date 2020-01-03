using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGAs
{
    public interface IGetInfo_SGAs
    {
        Task<Info_SGAsOutput> Execute(string filtro, int pagina, int rpp);
    }
}
