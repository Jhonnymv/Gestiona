using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion
{
    public interface IEditarInspeccionCabecera
    {
        Task Execute(int idCabecera, string objetivo, int gerenciaMarc, int empresaMarc, string lugar, string fechaInspeccion, string hora, int responsableLugar, string inspectores, string opcion, string usuario_red);
    }
}
