using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IAvistamientoFloraReadOnlyRepository _avistamientoFloraReadOnlyRepository;

        public NuevoRegistro(IAvistamientoFloraReadOnlyRepository avistamientoFloraReadOnlyRepository)
        {
            _avistamientoFloraReadOnlyRepository = avistamientoFloraReadOnlyRepository ?? throw new ArgumentNullException(nameof(avistamientoFloraReadOnlyRepository));
        }

        public async Task<int> Execute(string IdFicha, DateTime? FechaValidacion, int? IdZona, string UbicacionEste, string UbicacionNorte, string UbicacionAltitud, string Geneto, string Rameto, string CondicionReubicacion, string Sustrato, decimal? CoberturaDiametroMayor, decimal? CoberturaDiametroMenor, decimal? CoberturaDiametroAltura, string[]? Fotos)
        {
            var input = new Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora();

            input.IdFicha = IdFicha;
            input.Fechavalidacion = (DateTime)FechaValidacion;
            input.Zona = new Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona { Id=(int)IdZona};
            input.Ubicacion_Este = UbicacionEste;
            input.Ubicacion_Norte = UbicacionNorte;
            input.Ubicacion_Altitud = UbicacionAltitud;
            input.Geneto = Geneto;
            input.Rameto= Rameto;
            input.Condicion_Reubicacion = CondicionReubicacion;
            input.Sustrato = Sustrato;
            input.Cobertura_Diametro_Menor = (decimal)CoberturaDiametroMayor;
            input.Cobertura_Diametro_Menor = (decimal)CoberturaDiametroMenor;
            input.Cobertura_Diametro_Altura = (decimal)CoberturaDiametroAltura;
            input.Fotos = JsonConvert.SerializeObject(Fotos);

            var idAvistamientoFlora = await _avistamientoFloraReadOnlyRepository.RegistrarAvistamientoFlora(input);

            return idAvistamientoFlora;
        }
    }
}
