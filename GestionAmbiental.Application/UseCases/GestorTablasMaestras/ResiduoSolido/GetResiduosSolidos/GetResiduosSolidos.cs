using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduosSolidos
{
    public class GetResiduosSolidos : IGetResiduosSolidos
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetResiduosSolidos(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ResiduosSolidosOutput> Execute(string filtro, int pagina, int rpp)
        {
            var residuos_solidos = await _gestorTablasMaestrasReadOnlyRepository.GetResiduo_Solido(filtro);

            var outputs = new List<ResiduoSolidoOutput>();
            foreach (var item in residuos_solidos)
            {
                var temp = new ResiduoSolidoOutput();
                temp.Id_Residuo_Solido = item.Id_Residuo_Solido;
                temp.Descripcion = item.Descripcion;
                temp.Id_Tipo_Residuo = item.Tipo_Residuo.Id_Tipo_Residuo;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new ResiduosSolidosOutput
            {
                NResiduos = residuos_solidos.Count,
                ResiduosSolidos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
