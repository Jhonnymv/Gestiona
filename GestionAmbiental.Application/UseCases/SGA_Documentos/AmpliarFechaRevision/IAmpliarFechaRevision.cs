using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.AmpliarFechaRevision
{
    public interface IAmpliarFechaRevision
    {
        Task Execute(int IdRevision, string nuevaFecha);
    }
}
