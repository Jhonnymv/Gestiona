using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.CompletarAvistamientoFauna
{
    public interface ICompletarAvistamientoFauna
    {
        Task Execute(int IdAvistamiento, string[] fotos, string fecha, string hora, string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, int idLugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck, string[] evidencias, string descripcion, string UsuarioRed);
    }
}
