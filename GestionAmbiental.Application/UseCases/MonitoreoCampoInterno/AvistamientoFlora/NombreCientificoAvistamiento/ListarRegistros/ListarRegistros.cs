using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly INombreCientificoAvistamientoReadOnlyRepository _nombreCientificoAvistamientoReadOnlyRepository;

        public ListarRegistros(INombreCientificoAvistamientoReadOnlyRepository nombreCientificoAvistamientoReadOnlyRepository)
        {
            _nombreCientificoAvistamientoReadOnlyRepository = nombreCientificoAvistamientoReadOnlyRepository ?? throw new ArgumentNullException(nameof(nombreCientificoAvistamientoReadOnlyRepository));
        }

        public Task<NombreCientificoAvistamientoOuput> BuscarPorID(int idAvistamiento)
        {
            throw new NotImplementedException();
        }

        public async Task<NombreCientificosAvistamientoOuput> BuscarPorIdAvistamiento(int idAvistamiento)
        {
            //Consultar registros de BD
            var registros = await _nombreCientificoAvistamientoReadOnlyRepository.BuscarPorAvistamientoId(idAvistamiento);

            //Nueva lista en blanco
            var output = new NombreCientificosAvistamientoOuput();
            output.Registros = new List<NombreCientificoAvistamientoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new NombreCientificoAvistamientoOuput();
                compItem.Id = item.Id;
                compItem.Id_Avistamiento = item.Avistamiento.Id;
                if (item.NombreCientifico != null)
                {
                    compItem.Id_NombreCientifico = item.NombreCientifico.Id;
                    compItem.NombreCientifico = item.NombreCientifico.Nombre;
                    compItem.NombreComun = item.NombreCientifico.NombreComun;
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

        public Task<NombreCientificosAvistamientoOuput> Execute(int pagina, int rpp)
        {
            throw new NotImplementedException();
        }
    }
}
