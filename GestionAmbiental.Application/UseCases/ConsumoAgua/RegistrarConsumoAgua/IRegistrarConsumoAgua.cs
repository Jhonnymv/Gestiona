using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAgua
{
    public interface IRegistrarConsumoAgua
    {
        Task<int> Execute(int resolucion, string fecha, double tiempoBombeo, double lecturaInicial, double lecturaFInal, double volBombeado, double caudal, string usuarioRed);
    }
}
