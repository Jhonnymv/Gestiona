using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.EditarCategoriaTipo
{
    public interface IEditarCategoriaTipo
    {
        Task Execute(int IdCategoriaTipo, int IdCategoria, int IdTipoDoc, string Estado);
    }
}
