using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.EditarEO_RS
{
    public interface IEditarEO_RS
    {
        Task Execute(int IdEO_RS, string Razon_Social, string Registro, string Fecha_Vencimiento_Registro);
    }
}
