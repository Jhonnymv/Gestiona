using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.RegistrarInvolucrado
{
    public class RegistrarInvolucradoIncidente : IRegistrarInvolucradoIncidente
    {
        private readonly IRegistrarInvestigacionFinalReadOnlyRepository _registrarInvestigacionFinalReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public RegistrarInvolucradoIncidente(IRegistrarInvestigacionFinalReadOnlyRepository registrarInvestigacionFinalReadOnlyRepository, IGetUsuario getUsuario)
        {
            _registrarInvestigacionFinalReadOnlyRepository = registrarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarInvestigacionFinalReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdInvestigacion, string tipo, int usuario, string nombres, string apellidos, string docIdentidad, int edad, string puestoTrabajo, string empresa)
        {
            var involucrado = new Involucrado_Incidente_Investigacion();
            involucrado.Id_Investigacion_Final = IdInvestigacion;
            involucrado.Tipo_Involucrado = tipo;
            involucrado.Nombres = nombres;
            involucrado.Apellidos = apellidos;
            involucrado.Doc_Identidad = docIdentidad;

            if (usuario != 0)
            {
                var datos_usuario = await _getUsuario.Execute(usuario);
                involucrado.Id_Usuario = usuario;
                involucrado.Nombres = datos_usuario.Nombres;
                involucrado.Apellidos = datos_usuario.Apellidos;
                involucrado.Email = datos_usuario.Email.ToLower();
            }

            involucrado.Edad = edad;
            involucrado.Puesto_Trabajo = puestoTrabajo;
            involucrado.Empresa = empresa;
            if(involucrado.Tipo_Involucrado.Equals("Supervisor Inmediato (Titular Minero)"))
                involucrado.Empresa = "Marcobre";

            return await _registrarInvestigacionFinalReadOnlyRepository.RegistrarInvolucradoInvestigacion(involucrado);
        }
    }
}
