using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.ListarRegistros
{
    public class ListarRegistros:IListarRegistros
    {
        private readonly ICalidadAguaReadOnlyRepository _calidadAguaReadOnlyRepository;

        public ListarRegistros(ICalidadAguaReadOnlyRepository calidadAguaReadOnlyRepository)
        {
            _calidadAguaReadOnlyRepository = calidadAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(calidadAguaReadOnlyRepository));
        }

        public async Task<CalidadAguasOuput> Execute(int pagina, int rpp)
        {
            var registros = await _calidadAguaReadOnlyRepository.ListCalidadAgua();

            var output = new CalidadAguasOuput();
            output.Registros = new List<CalidadAguaOuput>();

            foreach(var item in registros)
            {
                var CalItem = new CalidadAguaOuput();

                CalItem.Id=item.Id;
                CalItem.Fecha=item.Fecha;
                CalItem.Hora=item.Hora;
                CalItem.Cod_Estacion=item.Cod_Estacion;
                CalItem.Descripcion=item.Descripcion;
                CalItem.Ubicacion=item.Ubicacion;
                if (item.Responsable != null)
                {
                    CalItem.Id_Usuario = item.Responsable.IdUsuario;
                    CalItem.NombreResponsable = item.Responsable.Nombres;
                    CalItem.ApellidoResponsable = item.Responsable.Apellidos;
                }
                CalItem.TemperaturaResult = item.TemperaturaResult;
                CalItem.TemperaturaEquipo = item.TemperaturaEquipo;
                CalItem.PhResult = item.PhResult;
                CalItem.PhEquipo = item.PhEquipo;
                CalItem.ConductividadResult = item.ConductividadResult;
                CalItem.ConductividadEquipo = item.ConductividadEquipo;
                CalItem.OxigenoResult = item.OxigenoResult;
                CalItem.OxigenoEquipo = item.OxigenoEquipo;
                CalItem.TurbiedadResult = item.TurbiedadResult;
                CalItem.TurbiedadEquipo = item.TurbiedadEquipo;
                CalItem.CloroResult = item.CloroResult;
                CalItem.CloroEquipo = item.CloroEquipo;

                output.Registros.Add(CalItem);
            }
            output.NRegistros=output.Registros.Count;

            output.Registros = output.Registros.Skip((pagina - 1) * rpp).Take(rpp).ToList();

            return output;
        }
        public async Task<CalidadAguaOuput> BuscarPorId(int id)
        {
            var item = await _calidadAguaReadOnlyRepository.BuscarPorID(id);

                var CalItem = new CalidadAguaOuput();

                CalItem.Id = item.Id;
                CalItem.Fecha = item.Fecha;
                CalItem.Hora = item.Hora;
                CalItem.Cod_Estacion = item.Cod_Estacion;
                CalItem.Descripcion = item.Descripcion;
                CalItem.Ubicacion = item.Ubicacion;
                if (item.Responsable != null)
                {
                    CalItem.Id_Usuario = item.Responsable.IdUsuario;
                    CalItem.NombreResponsable = item.Responsable.Nombres;
                    CalItem.ApellidoResponsable = item.Responsable.Apellidos;
                }
                CalItem.TemperaturaResult = item.TemperaturaResult;
                CalItem.TemperaturaEquipo = item.TemperaturaEquipo;
                CalItem.PhResult = item.PhResult;
                CalItem.PhEquipo = item.PhEquipo;
                CalItem.ConductividadResult = item.ConductividadResult;
                CalItem.ConductividadEquipo = item.ConductividadEquipo;
                CalItem.OxigenoResult = item.OxigenoResult;
                CalItem.OxigenoEquipo = item.OxigenoEquipo;
                CalItem.TurbiedadResult = item.TurbiedadResult;
                CalItem.TurbiedadEquipo = item.TurbiedadEquipo;
                CalItem.CloroResult = item.CloroResult;
                CalItem.CloroEquipo = item.CloroEquipo;


            return CalItem;
        }
    }
}
