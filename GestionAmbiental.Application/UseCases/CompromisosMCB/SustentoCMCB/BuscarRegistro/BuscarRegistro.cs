using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.BuscarRegistro
{
    public class BuscarRegistro : IBuscarRegistro
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public BuscarRegistro(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }

        public async Task<SustentoOuput> Execute(int Id)
        {
            var data = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

            return new SustentoOuput { 
                Id = Id,
                IdCompromiso = data.CompromisoCMCB.Id,
                Comentario_implementacion = data.Comentario_implementacion,
                Comentario_verificacion = data.Comentario_verificacion,
                Detalle_compromiso = data.Detalle_compromiso,
                Estado = data.Estado,
                Evidencia = data.Evidencia,
                Fecha_entrega = data.Fecha_entrega,
                Fecha_limite = data.Fecha_limite,
                Fecha_validacion = data.Fecha_validacion,
                IdAsesor = data.Asesor.IdUsuario,
                IdResponsable1 = data.Responsable1.IdUsuario,
                IdResponsable2 = data.Responsable2.IdUsuario,
                Medio_verificacion = data.Medio_verificacion,
                Sustento_cumplimiento = data.Sustento_cumplimiento                
            };
        }
    }
}
