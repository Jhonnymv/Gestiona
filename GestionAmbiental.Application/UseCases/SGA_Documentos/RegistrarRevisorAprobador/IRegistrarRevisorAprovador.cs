using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarRevisorAprobador
{
    public interface IRegistrarRevisorAprovador
    {
        Task<int> Execute(int idDocumento, int tipo, int orden,int idUsuario, string fechaLimite);
    }
}
