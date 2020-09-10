using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.Enum;
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
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarAvistaminetoFauna(IRegistrarAvistamientoFaunaReadOnlyRepository registrarAvistamientoFaunaReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registrarAvistamientoFaunaReadOnlyRepository = registrarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarAvistamientoFaunaReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(string usuarioRed, string[] fotos, string fecha, string hora,string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, int idLugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck, string opcion)
        {
            var nuevaFecha = DateTime.Parse(fecha+ " "+hora);
            var input = new Domain.AvistamientoFauna.AvistamientoFauna();
            input.Fotos = JsonConvert.SerializeObject(fotos);
            input.FechaAvistamiento = nuevaFecha;
            input.Ubicacion = ubicacion;
            input.Cantidad = cantidad;

            var ultimoCodigo = await _registrarAvistamientoFaunaReadOnlyRepository.GetUltimoCodigo();
            if (opcion.Equals("Registrar"))
            {
                input.Estado = EstadosAvistamiento.Registrado.ToString();

                if (ultimoCodigo != null && ultimoCodigo != "TBD")
                    input.Codigo = (Int32.Parse(ultimoCodigo) + 1).ToString();
                else
                    input.Codigo = "1";
            }
            else if (opcion.Equals("GuardarEstado"))
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
                
            if (idLugarReferencia != 0)
                input.LugarReferencia = new Domain.AvistamientoFauna.Items.LugarReferencia { IdLugarReferencia = idLugarReferencia };
                
            input.Usuario = new Domain.ValueObjects.Usuario { IdUsuario = idUsuario};


            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuarioRed);

            return await _registrarAvistamientoFaunaReadOnlyRepository.RegistrarAvistamientoFauna(Id_Usuario, input);
        }
    }
}
