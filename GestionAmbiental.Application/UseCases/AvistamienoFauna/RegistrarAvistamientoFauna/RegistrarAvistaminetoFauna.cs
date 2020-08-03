using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.RegistrarAvistamientoFauna
{
    public class RegistrarAvistaminetoFauna : IRegistrarAvistamientoFauna
    {
        private readonly IRegistrarAvistamientoFaunaReadOnlyRepository _registrarAvistamientoFaunaReadOnlyRepository;

        public RegistrarAvistaminetoFauna(IRegistrarAvistamientoFaunaReadOnlyRepository registrarAvistamientoFaunaReadOnlyRepository)
        {
            _registrarAvistamientoFaunaReadOnlyRepository = registrarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarAvistamientoFaunaReadOnlyRepository));
        }

        public async Task<int> Execute(string[] fotos, string fecha, string hora,string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, string lugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck)
        {
            var nuevaFecha = DateTime.Parse(fecha);
            var input = new Domain.AvistamientoFauna.AvistamientoFauna();
            input.Fotos = JsonConvert.SerializeObject(fotos);
            input.Fecha_Avistamiento = nuevaFecha;
            input.Ubicacion = ubicacion;
            input.Cantidad = cantidad;
            if(idNombreComun != 0)
                input.NombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna { IdNombreComunFauna = idNombreComun};
            if(idTipoAvistamiento != 0)
                input.TipoAvistamiento = new Domain.AvistamientoFauna.Items.TipoAvistamiento { IdTipoAvistamiento = idTipoAvistamiento};
            if(idEstadoFauna != 0)
                input.EstadoFauna = new Domain.AvistamientoFauna.Items.EstadoFauna { IdEstadoFauna = idEstadoFauna};
            input.Lugar_Referencia = lugarReferencia;
            input.Usuario = new Domain.ValueObjects.Usuario { IdUsuario = idUsuario};
            if (idEmpresa != 0)
                input.Empresa = new Domain.ValueObjects.Empresa { IdEmpresa = idEmpresa };
            if(idArea != 0)
                input.AreaTrabajo = new Domain.ValueObjects.AreaTrabajo { IdAreaTrabajo = idArea};
            input.NFotocheck = nFotocheck;


            return await _registrarAvistamientoFaunaReadOnlyRepository.RegistrarAvistamientoFauna(input);
        }
    }
}
