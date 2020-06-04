using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadesMedida
{
    public class GetUnidadesMedida : IGetUnidadesMedida
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUnidadesMedida(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UnidadesMedidaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetUnidad_Medida(filtro);

            var outputs = new List<UnidadMedidaOutput>();

            foreach (var item in data)
            {
                var temp = new UnidadMedidaOutput();
                temp.Id_Unidad_Medida = item.Id_Unidad_Medida.Value;
                temp.Descripcion = item.Descripcion;
                temp.Tipo = item.Tipo;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new UnidadesMedidaOutput
            {
                NUnidades = outputs.Count,
                Unidades = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
