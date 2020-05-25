using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EditarInvolucrado
{
    public class EditarInvolucradoIncidente : IEditarInvolucradoIncidente
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public EditarInvolucradoIncidente(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository, IGetUsuario getUsuario)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<int> Execute(int IdInvolucrado, int IdInvestigacion, string tipo, string nombres, string apellidos, int usuario, string DocIdentidad, int Edad, string PuestoTrabajo, string Empresa)
        {
            var involucrado = new Involucrado_Incidente_Investigacion();
            involucrado.Id_Involucrado_Incidente_Investigacion = IdInvolucrado;
            involucrado.Id_Investigacion_Final = IdInvestigacion;
            involucrado.Tipo_Involucrado = tipo;
            involucrado.Nombres = nombres;
            involucrado.Apellidos = apellidos;

            if (usuario != 0)
            {
                var datos_usuario = await _getUsuario.Execute(usuario);
                involucrado.Id_Usuario = usuario;
                involucrado.Nombres = datos_usuario.Nombres;
                involucrado.Apellidos = datos_usuario.Apellidos;
                involucrado.Email = datos_usuario.Email.ToLower();
            }

            involucrado.Doc_Identidad = DocIdentidad;
            involucrado.Edad = Edad;
            involucrado.Puesto_Trabajo = PuestoTrabajo;
            involucrado.Empresa = Empresa;

            if (involucrado.Tipo_Involucrado.Equals("Supervisor Inmediato (Titular Minero)"))
                involucrado.Empresa = "Marcobre";

            involucrado.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarInvestigacionFinalReadOnlyRepository.EditarInvolucradoInvestigacion(involucrado);
        }
    }
}
