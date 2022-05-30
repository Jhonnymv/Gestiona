using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona>> GetZonas();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico>> GetNombrescientificos();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia>> GetFenologias();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor>> GetVigores();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud>> GetSaluds();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso>> GetUsos();
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada>> GetfaunasAsociadas();
        Task<List<Domain.ValueObjects.Usuario>> GetUsuarios();
    }
}
