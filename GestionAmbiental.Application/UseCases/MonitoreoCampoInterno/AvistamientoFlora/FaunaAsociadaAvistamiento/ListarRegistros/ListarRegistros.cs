using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IFaunaAsociadaReadOnlyRepository _faunaAsociadaReadOnlyRepository;

        public ListarRegistros(IFaunaAsociadaReadOnlyRepository faunaAsociadaReadOnlyRepository)
        {
            _faunaAsociadaReadOnlyRepository = faunaAsociadaReadOnlyRepository ?? throw new ArgumentNullException(nameof(faunaAsociadaReadOnlyRepository));
        }

        public Task<faunaAsociadaAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<FaunasAsociadasAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {
            //Consultar registros de BD
            var registros = await _faunaAsociadaReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new FaunasAsociadasAvistamientosOuput();
            output.Registros = new List<faunaAsociadaAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new faunaAsociadaAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.FaunaAsociada != null)
                {
                    compItem.Id_FaunaAsociada = item.FaunaAsociada.Id;
                    compItem.Name = item.FaunaAsociada.Nombre;
                    compItem.Des = item.FaunaAsociada.Descripcion;
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

        public Task<FaunasAsociadasAvistamientosOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
