using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AreaCMCB.EditarRegistro
{
    public interface IEditarRegistro
    {
        Task Execute(int Id, string Area);
    }
}
