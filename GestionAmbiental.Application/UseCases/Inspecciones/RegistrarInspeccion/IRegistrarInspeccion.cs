using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public interface IRegistrarInspeccion
    {
        Task Execute(string data);
    }
}
