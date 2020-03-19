using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.AgregarEO_RS
{
    public interface IAgregarEO_RS
    {
        Task<int> Execute(string razon_social, string registro, string fecha_vencimiento);
    }
}
