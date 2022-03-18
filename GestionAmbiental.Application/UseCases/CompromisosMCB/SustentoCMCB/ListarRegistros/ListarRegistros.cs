using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public ListarRegistros(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }

        public async Task<SustentosOuput> Execute(int pagina, int fpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            var data = await _sustentoReadOnlyRepository.ListarSustentos(gerencias,areas,criterios,estados,etapas);
            var output = new SustentosOuput();

            var sustentos = new List<SustentoOuput>();
            foreach (var item in data)
            {
                sustentos.Add(new SustentoOuput(item));
            }

            output.Registros = sustentos.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            output.NRegistros = sustentos.Count;

            return output;
        }
        public async Task<SustentoOuput> ObtenerSustento(int Id)
        {
            var sustemto= await _sustentoReadOnlyRepository.ObtenerSustento(Id);
            var ouput = new SustentoOuput();
            ouput.Id = sustemto.Id;
            ouput.IdCompromiso = sustemto.CompromisoCMCB.Id;
            ouput.Fecha_limite= sustemto.Fecha_limite;
            ouput.Fecha_entrega= sustemto.Fecha_entrega;
            ouput.Fecha_validacion= sustemto.Fecha_validacion;
            ouput.IdResponsable1 = sustemto.Responsable1?.IdUsuario;
            ouput.IdResponsable2 = sustemto.Responsable2?.IdUsuario;
            ouput.IdAsesor = sustemto.Asesor?.IdUsuario;
            ouput.Estado = sustemto.Estado;
            ouput.Sustento_cumplimiento= sustemto.Sustento_cumplimiento;
            ouput.Detalle_compromiso= sustemto.Detalle_compromiso;
            ouput.Evidencia= sustemto.Evidencia;
            ouput.Comentario_implementacion= sustemto.Comentario_implementacion;
            ouput.Comentario_verificacion= sustemto.Comentario_verificacion;
            ouput.Medio_verificacion= sustemto.Medio_verificacion;

            return ouput;
        }
    }
}
