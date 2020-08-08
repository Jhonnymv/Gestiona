using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna
{
    public interface IGetAvistamientoFauna
    {
        Task<AvistamientoFaunaOutput> Execute(int IdAvistamiento);
    }
}
