using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionFinal
{
    public class GetInvestigacionFinal : IGetInvestigacionFinal
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;
        private readonly IGetUsuario _getUsuario;

        public GetInvestigacionFinal(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository, IGetUsuario getUsuario)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<InvestigacionFinalOutput> Execute(int IdInvestigacion, string UsuarioRed)
        {
            var investigacion = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionFinal(IdInvestigacion);
            var output = new InvestigacionFinalOutput(investigacion);
            var usuario = await _getUsuario.Execute(UsuarioRed);
            if (usuario.IdRol.Equals((int)InspeccionRol.AsesorMAP))
                output.PermisoMA = true;
            else
                output.PermisoMA = false;
            if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                output.PermisoAdmin = true;
            else
                output.PermisoAdmin = false;
            return output;
        }
        public async Task<InvestigacionFinalOutput> Execute(int IdInvestigacion)
        {
            var investigacion = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionFinal(IdInvestigacion);
            var output = new InvestigacionFinalOutput(investigacion);            
            return output;
        }
    }
}
