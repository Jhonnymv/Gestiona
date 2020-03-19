using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivos
{
    public interface IGetMotivos
    {
        Task<MotivosOutput> Execute(string filtro, int pagina, int rpp);
    }
}
