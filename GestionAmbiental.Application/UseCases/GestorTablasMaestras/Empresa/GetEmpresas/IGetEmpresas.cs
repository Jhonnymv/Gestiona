using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresas
{
    public interface IGetEmpresas
    {
        Task<EmpresasOutput> Execute(string filtro, int pagina, int rpp);
    }
}
