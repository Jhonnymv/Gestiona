using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.AgregarCategoriaTipo
{
    public interface IAgregarCategoriaTipo
    {
        Task<int> Execute(int IdCategoria, int IdTipoDoc);
    }
}
