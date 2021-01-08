using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento
{
    public interface IEditarRevisionDocumento
    {
        Task Execute(int IdDocumento, int IdRevisionDocumento, string Comentario, string Opcion);
    }
}
