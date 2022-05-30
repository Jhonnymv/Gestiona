using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IFenologiaAvistamientoReadOnlyRepository _fenologiaAvistamientoReadOnlyRepository;

        public ListarRegistros(IFenologiaAvistamientoReadOnlyRepository fenologiaAvistamientoReadOnlyRepository)
        {
            _fenologiaAvistamientoReadOnlyRepository = fenologiaAvistamientoReadOnlyRepository ?? throw new ArgumentNullException(nameof(fenologiaAvistamientoReadOnlyRepository));
        }

        public Task<FenologiaAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<FenologiasAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {

            var registros = await _fenologiaAvistamientoReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new FenologiasAvistamientosOuput();
            output.Registros = new List<FenologiaAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new FenologiaAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.Fenologia != null)
                {
                    compItem.Id_Fenologia = item.Fenologia.Id;
                    compItem.Name = item.Fenologia.Nombre;
                    compItem.Des = item.Fenologia.Descripcion;
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

        public Task<FenologiasAvistamientosOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
