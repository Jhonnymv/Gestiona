using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.GetFactoresCausa
{
    public class GetFactoresCausa : IGetFactoresCausa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetFactoresCausa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<FactoresCausaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetFactor_Causa_Incidente(filtro);

            var outputs = new List<FactorCausaOutput>();

            foreach (var item in data)
            {
                var temp = new FactorCausaOutput();
                temp.Id_Factor_Causa_Incidente = item.Id_Factor_Causa_Incidente;
                temp.Descripcion = item.Descripcion;
                temp.Tipo_Factor = item.Tipo_Factor;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new FactoresCausaOutput
            {
                NFactores = outputs.Count,
                Factores = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
