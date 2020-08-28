using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.EditarContactoEmergencia
{
    public interface IEditarContactoEmergencia
    {
        Task Execute(int id, string nombres,string apellidos,string telefono, string estado);
    }
}
