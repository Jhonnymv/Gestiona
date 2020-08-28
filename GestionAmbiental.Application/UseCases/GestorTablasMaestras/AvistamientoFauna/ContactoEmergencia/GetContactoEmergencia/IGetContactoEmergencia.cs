using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.GetContactoEmergencia
{
    public interface IGetContactoEmergencia
    {
        Task<ContactoEmergenciaOutput> Execute(int idContacto);
    }
}
