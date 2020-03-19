using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.EditarMotivo
{
    public interface IEditarMotivo
    {
        Task Execute(int IdMotivo, string Descripcion,string Estado);
    }
}
