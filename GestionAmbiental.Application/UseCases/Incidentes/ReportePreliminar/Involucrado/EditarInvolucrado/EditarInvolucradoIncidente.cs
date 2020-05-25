using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EditarInvolucrado
{
    public class EditarInvolucradoIncidente : IEditarInvolucradoIncidente
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public EditarInvolucradoIncidente(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository, IGetUsuario getUsuario)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdInvolucrado, int IdReporte, string tipo, int usuario, string nombres, string apellidos, string correo, string puesto, string celular, string turno)
        {
            var involucrado = new Involucrado_Incidente_Reporte();
            involucrado.Id_Involucrado_Incidente_Reporte = IdInvolucrado;
            involucrado.Id_Reporte_Preliminar = IdReporte;
            involucrado.Tipo_Involucrado = tipo;
            involucrado.Nombres = nombres;
            involucrado.Apellidos = apellidos;
            involucrado.Email = correo;

            if (usuario != 0)
            {
                var datos_usuario = await _getUsuario.Execute(usuario);
                involucrado.Id_Usuario = usuario;
                involucrado.Nombres = datos_usuario.Nombres;
                involucrado.Apellidos = datos_usuario.Apellidos;
                involucrado.Email = datos_usuario.Email.ToLower();
            }

            involucrado.Puesto_Trabajo = puesto;
            involucrado.Celular = celular;
            involucrado.Turno = turno;
            involucrado.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarReportePreliminarReadOnlyRepository.EditarInvolucradoReportePreliminar(involucrado);
        }
    }
}
