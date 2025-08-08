using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReportsMA
{
    public interface IGetFlashReportsMA
    {
        Task<FlashReportsOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp, string usuarioRed);
    }
}
