using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.GetContactosEmergencia
{
    public interface IGetContactosEmergencia
    {
        Task<ContactosEmergenciaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
