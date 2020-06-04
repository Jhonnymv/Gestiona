using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.EditarFactorCausa
{
    public interface IEditarFactorCausa
    {
        Task Execute(int id, string descripcion,string tipo, string estado);
    }
}
