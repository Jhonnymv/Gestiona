using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.EditarConsumoAgua
{
    public interface IEditarConsumoAgua
    {
        Task Execute(int idConsumo, int resolucion, string fecha, double tiempoBombeo, double lecturaInicial, double lecturaFinal, double volBombeado, double caudal);
    }
}
