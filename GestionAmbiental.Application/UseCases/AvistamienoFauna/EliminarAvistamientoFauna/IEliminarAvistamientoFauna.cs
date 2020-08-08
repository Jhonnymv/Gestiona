using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarAvistamientoFauna
{
    public interface IEliminarAvistamientoFauna
    {
        Task Execute(int IdAvistamiento);
    }
}
