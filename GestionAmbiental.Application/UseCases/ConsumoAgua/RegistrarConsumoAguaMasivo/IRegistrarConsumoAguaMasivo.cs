using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAguaMasivo
{
    public interface IRegistrarConsumoAguaMasivo
    {
        Task<int> Execute(List<ConsumoAguaInput> datos, string usuarioRed);
    }
}
