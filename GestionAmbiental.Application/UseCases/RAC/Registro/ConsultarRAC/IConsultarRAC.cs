using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRAC
{
    public interface IConsultarRAC
    {
        Task<RACOutput> Execute(int Id_RAC,string usuario_red);
    }
}
