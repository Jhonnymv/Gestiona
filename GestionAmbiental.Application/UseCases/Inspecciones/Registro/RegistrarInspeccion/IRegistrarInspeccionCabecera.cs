using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion
{
    public interface IRegistrarInspeccionCabecera
    {
        Task<int> Execute(string objetivo, string usuario_red, int gerenciaMarc, int empresaMarc, int gerenciaInspec, string lugar, string fechaInspeccion, string hora, string tipoInspeccion, int responsableLugar, string responsableInspeccion, string inspectores, string opcion, string codigoFormato, string areaFormato, int?idMiembroComision, string participantesInspeccion);
    }
}
