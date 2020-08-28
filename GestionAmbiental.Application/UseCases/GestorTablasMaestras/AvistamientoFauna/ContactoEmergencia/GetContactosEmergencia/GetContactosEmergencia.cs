using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia.GetContactosEmergencia
{
    public class GetContactosEmergencia : IGetContactosEmergencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetContactosEmergencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ContactosEmergenciaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetContactoEmergencia(filtro);

            var outputs = new List<ContactoEmergenciaOutput>();
            foreach (var item in input)
            {
                var temp = new ContactoEmergenciaOutput();
                temp.IdContactoEmergencia = item.IdContactoEmergencia;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.Telefono = item.Telefono;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new ContactosEmergenciaOutput
            {
                NContactos = input.Count,
                ContactosEmergencia = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
