using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EditarAvistamientoFauna
{
    public class EditarAvistamientoFauna : IEditarAvistamientoFauna
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;
        private readonly IRegistrarAvistamientoFaunaReadOnlyRepository _registrarAvistamientoFaunaReadOnlyRepository;

        public EditarAvistamientoFauna(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository,IRegistrarAvistamientoFaunaReadOnlyRepository registrarAvistamientoFaunaReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
            _registrarAvistamientoFaunaReadOnlyRepository = registrarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarAvistamientoFaunaReadOnlyRepository));
        }

        public async Task Execute(int IdAvistamiento, string[] fotos, string fecha, string hora, string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, string lugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck)
        {
            var nuevaFecha = DateTime.Parse(fecha + " " + hora);
            var input = new Domain.AvistamientoFauna.AvistamientoFauna();

            input.IdAvistamientoFauna = IdAvistamiento;
            input.Fotos = JsonConvert.SerializeObject(fotos);
            input.Fecha_Avistamiento = nuevaFecha;
            input.Ubicacion = ubicacion;
            input.Cantidad = cantidad;

            var ultimoCodigo = await _registrarAvistamientoFaunaReadOnlyRepository.GetUltimoCodigo();
            if (ultimoCodigo != null && ultimoCodigo != "TBD" && (idNombreComun != 0 || idTipoAvistamiento != 0 || idEstadoFauna != 0))
            {
                input.Codigo = (Int32.Parse(ultimoCodigo) + 1).ToString();
                input.Estado = EstadosAvistamiento.Registrado.ToString();
            }
            else if (idNombreComun != 0 || idTipoAvistamiento != 0 || idEstadoFauna != 0)
            {
                input.Codigo = "1";
                input.Estado = EstadosAvistamiento.Registrado.ToString();
            }
            else if (idNombreComun == 0 || idTipoAvistamiento == 0 || idEstadoFauna == 0)
            {
                input.Codigo = "TBD";
                input.Estado = EstadosAvistamiento.Pendiente.ToString();
            }

            if (idNombreComun != 0)
                input.NombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna { IdNombreComunFauna = idNombreComun };

            if (idTipoAvistamiento != 0)
                input.TipoAvistamiento = new Domain.AvistamientoFauna.Items.TipoAvistamiento { IdTipoAvistamiento = idTipoAvistamiento };

            if (idEstadoFauna != 0)
                input.EstadoFauna = new Domain.AvistamientoFauna.Items.EstadoFauna { IdEstadoFauna = idEstadoFauna };

            input.Lugar_Referencia = lugarReferencia;
            input.Usuario = new Domain.ValueObjects.Usuario { IdUsuario = idUsuario };
            if (idEmpresa != 0)
                input.Empresa = new Domain.ValueObjects.Empresa { IdEmpresa = idEmpresa };
            if (idArea != 0)
                input.AreaTrabajo = new Domain.ValueObjects.AreaTrabajo { IdAreaTrabajo = idArea };
            input.NFotocheck = nFotocheck;

            await _editarAvistamientoFaunaReadOnlyRepository.ActualizarAvistamiento(input);

        }
    }
}
