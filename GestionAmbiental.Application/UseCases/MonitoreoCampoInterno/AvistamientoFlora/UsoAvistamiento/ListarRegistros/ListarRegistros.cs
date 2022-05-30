using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IUsoAvistamientoReadOnlyRepository _usoAvistamientoReadOnlyRepository;

        public ListarRegistros(IUsoAvistamientoReadOnlyRepository usoAvistamientoReadOnlyRepository)
        {
            _usoAvistamientoReadOnlyRepository = usoAvistamientoReadOnlyRepository ?? throw new ArgumentNullException(nameof(usoAvistamientoReadOnlyRepository));
        }

        public Task<UsoAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<UsoAvistamientosOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {
            //Consultar registros de BD
            var registros = await _usoAvistamientoReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new UsoAvistamientosOuput();
            output.Registros = new List<UsoAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new UsoAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.Uso != null)
                {
                    compItem.Id_Uso = item.Uso.Id;
                    compItem.Name = item.Uso.Nombre;
                    compItem.Des = item.Uso.Descripcion;
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

        public Task<UsoAvistamientosOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
