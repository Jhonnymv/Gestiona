using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivos
{
    public class GetMotivos : IGetMotivos
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetMotivos(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<MotivosOutput> Execute(string filtro, int pagina, int rpp)
        {
            var motivos = await _gestorTablasMaestrasReadOnlyRepository.GetMotivo_Ingreso_Salida(filtro);

            var outputs = new List<MotivoOutput>();
            foreach (var item in motivos)
            {
                var temp = new MotivoOutput();
                temp.Id_Motivo = item.Id_Motivo;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new MotivosOutput
            {
                NMotivos = motivos.Count,
                Motivos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
