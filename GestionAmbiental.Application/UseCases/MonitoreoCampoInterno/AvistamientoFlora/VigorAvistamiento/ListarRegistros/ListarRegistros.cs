using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IVigorAvistamientoReadOnlyRepository _vigorAvistamientoReadOnlyRepository;

        public ListarRegistros(IVigorAvistamientoReadOnlyRepository vigorAvistamientoReadOnlyRepository)
        {
            _vigorAvistamientoReadOnlyRepository = vigorAvistamientoReadOnlyRepository ?? throw new ArgumentNullException(nameof(vigorAvistamientoReadOnlyRepository));
        }

        public Task<VigorAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<vigorAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {
            //Consultar registros de BD
            var registros = await _vigorAvistamientoReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new vigorAvistamientosOuput();
            output.Registros = new List<VigorAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new VigorAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.Vigor != null)
                {
                    compItem.Id_vigor = item.Vigor.Id;
                    compItem.Name = item.Vigor.Nombre;
                    compItem.Des = item.Vigor.Descripcion;
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

        public Task<vigorAvistamientosOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
