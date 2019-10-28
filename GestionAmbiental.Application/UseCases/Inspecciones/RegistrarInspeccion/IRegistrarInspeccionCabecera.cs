using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public interface IRegistrarInspeccionCabecera
    {
        Task<int> Execute(string objetivo, int gerenciaMarc, int empresaMarc, int gerenciaEPCM, int empresaEPCM, string lugar, string fechaInspeccion,string hora, int responsableLugar, int responsableMA, string inspectores);
    }
}
