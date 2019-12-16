using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.GetGerencias
{
    public interface IGetGerencias
    {
        Task<GerenciasOutput> Execute(string filtro, int pagina, int rpp);
    }
}
