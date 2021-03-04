using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriaTipo
{
    public interface IGetCategoriaTipo
    {
        Task<CategoriaTipoOutput> Execute(int IdCategoriaTipo);
    }
}
