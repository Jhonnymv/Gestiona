using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.RegistrarInvolucrado
{
    public class RegistrarInvolucradoIncidente : IRegistrarInvolucradoIncidente
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarInvolucradoIncidente(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdReporte, string tipo, int usuario, string nombres, string apellidos, string correo, string puesto, string celular, string turno)
        {

            var involucrado = new Involucrado_Incidente_Reporte();
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

            return await _registrarReportePreliminarReadOnlyRepository.RegistrarInvolucradoReportePreliminar(involucrado);
        }
    }
}
