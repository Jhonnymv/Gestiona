using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.AgregarContactoEmergencia
{
    public interface IAgregarContactoEmergencia
    {
        Task<int> Execute(string nombres,string apellidos,string telefono);
    }
}
