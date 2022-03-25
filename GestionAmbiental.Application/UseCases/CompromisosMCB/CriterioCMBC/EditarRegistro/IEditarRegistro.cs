using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.EditarRegistro
{
    public interface IEditarRegistro
    {
        Task Execute(int Id, string Criterio, string Detalle);
    }
}
