using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriasTipos
{
    public interface IGetCategoriasTipo
    {
        Task<CategoriasTiposOutput> Execute(string filtro, int pagina, int rpp);
    }
}
