using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.FirmarDocumento
{
    public interface IFirmarDocumento
    {
        Task Execute(int IdDocumento, string DocFirmado,string Codigo, int IdCategoriaTipo);
    }
}
