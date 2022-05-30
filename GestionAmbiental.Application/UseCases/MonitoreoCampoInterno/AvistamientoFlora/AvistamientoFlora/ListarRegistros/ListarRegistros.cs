using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IAvistamientoFloraReadOnlyRepository _avistamientoFloraReadOnlyRepository;

        public ListarRegistros(IAvistamientoFloraReadOnlyRepository avistamientoFloraReadOnlyRepository)
        {
            _avistamientoFloraReadOnlyRepository = avistamientoFloraReadOnlyRepository ?? throw new ArgumentNullException(nameof(avistamientoFloraReadOnlyRepository));
        }

        public async Task<AvistamientosFloraOuput> Execute(int pagina, int rpp)
        {
            //Consultar registros de BD
            var registros = await _avistamientoFloraReadOnlyRepository.ListAvistamientoFlora();

            //Nueva lista en blanco
            var output = new AvistamientosFloraOuput();
            output.Registros = new List<AvistamientoFloraOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var AvistItem = new AvistamientoFloraOuput();

                AvistItem.Id = item.Id;
                AvistItem.IdFicha = item.IdFicha;
                AvistItem.Fechavalidacion = item.Fechavalidacion;

                if (item.Zona != null)
                {
                    AvistItem.IdZonaMCIF = item.Zona.Id;
                    AvistItem.NombreZona = item.Zona.Nombre;
                }

                AvistItem.Ubicacion_Este = item.Ubicacion_Este;
                AvistItem.Ubicacion_Norte = item.Ubicacion_Norte;
                AvistItem.Ubicacion_Altitud = item.Ubicacion_Altitud;

                AvistItem.Geneto=item.Geneto;
                AvistItem.Rameto=item.Rameto;
                AvistItem.Condicion_Reubicacion= item.Condicion_Reubicacion;
                AvistItem.Sustrato=item.Sustrato;
                AvistItem.Cobertura_Diametro_Mayor= item.Cobertura_Diametro_Mayor;
                AvistItem.Cobertura_Diametro_Menor= item.Cobertura_Diametro_Menor;
                AvistItem.Cobertura_Diametro_Altura = item.Cobertura_Diametro_Altura;
                AvistItem.Fotos=item.Fotos;

                output.Registros.Add(AvistItem);
            }
            //Contar registros
            output.NRegistros = output.Registros.Count;
            //Filtrar registros en funcion a la pagina y registros por pagina
            output.Registros = output.Registros.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            //Retornar registros
            return output;
        }

       public async Task<AvistamientoFloraOuput> BuscarPorID(int idAvistamiento)
        {
            var avistameinto = await _avistamientoFloraReadOnlyRepository.BuscarPorID(idAvistamiento);
            var ouput = new AvistamientoFloraOuput();
            ouput.Id = avistameinto.Id;
            ouput.IdFicha = avistameinto.IdFicha;
            if (avistameinto.Zona != null)
            {
                ouput.IdZonaMCIF = avistameinto.Zona.Id;
                ouput.NombreZona = avistameinto.Zona.Nombre;
            }

            ouput.Ubicacion_Este = avistameinto.Ubicacion_Este;
            ouput.Ubicacion_Norte = avistameinto.Ubicacion_Norte;
            ouput.Ubicacion_Altitud = avistameinto.Ubicacion_Altitud;

            ouput.Geneto = avistameinto.Geneto;
            ouput.Rameto = avistameinto.Rameto;
            ouput.Condicion_Reubicacion = avistameinto.Condicion_Reubicacion;
            ouput.Sustrato = avistameinto.Sustrato;
            ouput.Cobertura_Diametro_Mayor = avistameinto.Cobertura_Diametro_Mayor;
            ouput.Cobertura_Diametro_Menor = avistameinto.Cobertura_Diametro_Menor;
            ouput.Cobertura_Diametro_Altura = avistameinto.Cobertura_Diametro_Altura;
            ouput.Fotos = avistameinto.Fotos;

            return ouput;
        }
    }
}
