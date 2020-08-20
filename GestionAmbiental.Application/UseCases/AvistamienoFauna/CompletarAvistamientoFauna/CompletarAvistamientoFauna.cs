using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.CompletarAvistamientoFauna
{
    public class CompletarAvistamientoFauna : ICompletarAvistamientoFauna
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public CompletarAvistamientoFauna(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdAvistamiento, string[] Evidencias, string Descripcion, string UsuarioRed)
        {
            var input = new AvistamientoFauna();

            var IdUsuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(UsuarioRed);

            input.IdAvistamientoFauna = IdAvistamiento;
            input.EvidenciaAccion = JsonConvert.SerializeObject(Evidencias);
            input.DescripcionAccion = Descripcion;
            input.Supervisor = new Domain.ValueObjects.Usuario { IdUsuario = IdUsuario };
            input.Estado = EstadosAvistamiento.Completado.ToString();

            await _editarAvistamientoFaunaReadOnlyRepository.CompletarAvistamiento(input);
        }

        public async Task Execute(int IdAvistamiento, string[] fotos, string fecha, string hora, string ubicacion, int cantidad, int idNombreComun, int idTipoAvistamiento, int idEstadoFauna, int idLugarReferencia, int idUsuario, int idEmpresa, int idArea, string nFotocheck, string[] evidencias, string descripcion, string UsuarioRed)
        {
            var input = new AvistamientoFauna();

            var IdUsuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(UsuarioRed);

            input.IdAvistamientoFauna = IdAvistamiento;
            input.EvidenciaAccion = JsonConvert.SerializeObject(evidencias);
            input.DescripcionAccion = descripcion;
            input.Supervisor = new Domain.ValueObjects.Usuario { IdUsuario = IdUsuario };
            input.Estado = EstadosAvistamiento.Completado.ToString();

            input.Fotos = JsonConvert.SerializeObject(fotos);
            var nuevaFecha = DateTime.Parse(fecha + " " + hora);
            input.FechaAvistamiento = nuevaFecha;
            input.Ubicacion = ubicacion;
            input.Cantidad = cantidad;

            if (idNombreComun != 0)
                input.NombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna { IdNombreComunFauna = idNombreComun };

            if (idTipoAvistamiento != 0)
                input.TipoAvistamiento = new Domain.AvistamientoFauna.Items.TipoAvistamiento { IdTipoAvistamiento = idTipoAvistamiento };

            if (idEstadoFauna != 0)
                input.EstadoFauna = new Domain.AvistamientoFauna.Items.EstadoFauna { IdEstadoFauna = idEstadoFauna };

            if (idLugarReferencia != 0)
                input.LugarReferencia = new Domain.AvistamientoFauna.Items.LugarReferencia { IdLugarReferencia = idLugarReferencia };

            input.Usuario = new Domain.ValueObjects.Usuario { IdUsuario = idUsuario };

            await _editarAvistamientoFaunaReadOnlyRepository.CompletarAvistamiento(input);
        }
    }
}
