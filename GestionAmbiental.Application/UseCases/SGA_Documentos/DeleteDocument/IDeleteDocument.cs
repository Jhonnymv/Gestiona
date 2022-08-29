using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.DeleteDocument
{
    public interface IDeleteDocument
    {
        Task Execute(int IdDocument);
    }
}
