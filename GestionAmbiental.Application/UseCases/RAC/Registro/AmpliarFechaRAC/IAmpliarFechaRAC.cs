using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.AmpliarFechaRAC
{
    public interface IAmpliarFechaRAC
    {
        Task Execute(int IdRAC, string nuevaFecha,string usuario_red);
    }
}
