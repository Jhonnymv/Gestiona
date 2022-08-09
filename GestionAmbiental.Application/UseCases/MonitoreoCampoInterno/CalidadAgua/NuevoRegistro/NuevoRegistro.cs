using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly ICalidadAguaReadOnlyRepository _calidadAguaReadOnlyRepository;

        public NuevoRegistro(ICalidadAguaReadOnlyRepository calidadAguaReadOnlyRepository)
        {
            _calidadAguaReadOnlyRepository = calidadAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(calidadAguaReadOnlyRepository));
        }

        public async Task<int> Execute(DateTime Fecha, string Hora, string Cod_Estacion, string Descripcion, string Ubicacion, int Id_Usuario, string TemperaturaResult, string TemperaturaEquipo, string PhResult, string PhEquipo, string ConductividadResult, string ConductividadEquipo, string OxigenoResult, string OxigenoEquipo, string TurbiedadResult, string TurbiedadEquipo, string CloroResult, string CloroEquipo)
        {
            var input = new Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua();
            input.Fecha = Fecha;
            input.Hora = Hora;
            input.Cod_Estacion = Cod_Estacion;
            input.Descripcion = Descripcion;
            input.Ubicacion = Ubicacion;
            input.Responsable = new Domain.ValueObjects.Usuario { IdUsuario = Id_Usuario };
            input.TemperaturaResult = TemperaturaResult;
            input.TemperaturaEquipo = TemperaturaEquipo;
            input.PhResult = PhResult;
            input.PhEquipo = PhEquipo;
            input.ConductividadResult = ConductividadResult;
            input.ConductividadEquipo = ConductividadEquipo;
            input.OxigenoResult = OxigenoResult;
            input.OxigenoEquipo = OxigenoEquipo;
            input.TurbiedadResult = TurbiedadResult;
            input.TurbiedadEquipo = TurbiedadEquipo;
            input.CloroResult = CloroResult;
            input.CloroEquipo = CloroEquipo;

            var idCalidadAgua = await _calidadAguaReadOnlyRepository.RegistrarCalidaAgua(input);

            return idCalidadAgua;
        }
    }
}
