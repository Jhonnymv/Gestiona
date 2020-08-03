using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.RegistrarAvistamientoFauna
{
    public interface IRegistrarAvistamientoFauna
    {
        Task<int> Execute(string[]fotos,string fecha, string hora, string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, string lugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck);
    }
}
