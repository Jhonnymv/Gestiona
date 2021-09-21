using GestionAmbiental.Domain.Enum;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA
{
    public interface IGetInfo_SGA
    {
        Task<Info_SGAOutput> Execute(Tipo_Info_SGA tipo);
        Task<LDAPConnectionOutput> ExecuteLDAPConnection(Tipo_Info_SGA tipo);
    }
}
