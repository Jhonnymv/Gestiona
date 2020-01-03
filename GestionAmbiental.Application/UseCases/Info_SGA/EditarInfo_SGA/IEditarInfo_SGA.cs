using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA.EditarInfo_SGA
{
    public interface IEditarInfo_SGA
    {
        Task Execute(int idInfo, string tipo,string info);
    }
}
