using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ISaludAvistamientoReadOnlyRepository _saludAvistamientoReadOnlyRepository;

        public ListarRegistros(ISaludAvistamientoReadOnlyRepository saludAvistamientoReadOnlyRepository)
        {
            _saludAvistamientoReadOnlyRepository = saludAvistamientoReadOnlyRepository ?? throw new ArgumentNullException(nameof(saludAvistamientoReadOnlyRepository));
        }

        public Task<SaludAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<SaludAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {
            var registros = await _saludAvistamientoReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new SaludAvistamientosOuput();
            output.Registros = new List<SaludAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new SaludAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.Salud != null)
                {
                    compItem.Id_Salud = item.Salud.Id;
                    compItem.Name = item.Salud.Nombre;
                    compItem.Des = item.Salud.Descripcion;
                }


                compItem.Cantidad = item.Cantidad;
                compItem.Descripcion = item.Descripcion;
                output.Registros.Add(compItem);
            }
            //Contar registros
            output.NRegistros = output.Registros.Count;

            //Retornar registros
            return output;
        }

        public Task<SaludAvistamientosOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
