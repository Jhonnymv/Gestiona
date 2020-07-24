using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.EditarNombreComun
{
    public interface IEditarNombreComun
    {
        Task Execute(int IdNombreComun, string Descripcion,int IdTipoFauna,string Foto, string Estado);
    }
}
