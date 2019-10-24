using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public class RegistrarInspeccion : IRegistrarInspeccion
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;

        public RegistrarInspeccion(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository;
        }

        public async Task Execute(string data)
        {
            //11 campos cabecera
            var decodedData = HttpUtility.UrlDecode(data);
            var camposFormulario = decodedData.Split('&');

            
            var objetivo = camposFormulario[0].Split("=")[1];
            var gerenciaMarc = camposFormulario[1].Split("=")[1];
            var empresaMarc = camposFormulario[2].Split("=")[1];
            var gerenciaEPCM = camposFormulario[3].Split("=")[1];
            var empresaEPCM = camposFormulario[4].Split("=")[1];
            var lugar = camposFormulario[5].Split("=")[1];
            var fechaInspeccion = camposFormulario[6].Split("=")[1] + " " + camposFormulario[7].Split("=")[1] + ":00";
            var responsableLugar = camposFormulario[8].Split("=")[1];
            var responsableMA = camposFormulario[9].Split("=")[1];
            var inspectores = camposFormulario[10].Split("=")[1];
            

            var cabecera = new NuevaInspeccionCabecera(
                Int32.Parse(gerenciaMarc),
                Int32.Parse(gerenciaEPCM),
                Int32.Parse(empresaMarc),
                Int32.Parse(empresaEPCM),
                Convert.ToDateTime(fechaInspeccion),
                lugar,
                Int32.Parse(responsableLugar),
                Int32.Parse(responsableMA),
                inspectores,
                objetivo);


            //TODO pasar datos
            await _registarInspeccionReadOnlyRepository.RegistrarInspeccionCabecera(cabecera);
            var listFilas = data.Split('&');
            foreach(var fila in listFilas)
            {
                var detalle = new NuevaInspeccionDetalle();
                
                //await _registarInspeccionReadOnlyRepository.RegistrarInspeccionDetalle(detalle);
            }
        }
    }
}
