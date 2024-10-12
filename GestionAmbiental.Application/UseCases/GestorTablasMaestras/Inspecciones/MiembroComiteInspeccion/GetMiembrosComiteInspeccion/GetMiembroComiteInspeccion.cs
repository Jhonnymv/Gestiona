using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembrosComiteInspeccion
{
    internal class GetMiembroComiteInspeccion : IGetMiembrosComiteInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetMiembroComiteInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<MiembrosComiteInspeccionOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetMiembrosComiteInspeccion(filtro);

            var outputs = new List<MiembroComiteInspeccionOutput>();
            foreach (var item in input)
            {
                var temp = new MiembroComiteInspeccionOutput();
                temp.IdMiembroComite = item.Id_Miembro_Comite;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new MiembrosComiteInspeccionOutput
            {
                NMiembrosInspeccion = input.Count,
                MiembrosComiteInspeccion = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
