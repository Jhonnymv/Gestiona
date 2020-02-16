using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRACs
{
    public interface IConsultarRACs
    {
        Task<RACsOutput> Execute(int pagina, int fpp, DateTime fecha, string codigo,int filtroUsuario,int filtroGerenciaObs);
    }
}
