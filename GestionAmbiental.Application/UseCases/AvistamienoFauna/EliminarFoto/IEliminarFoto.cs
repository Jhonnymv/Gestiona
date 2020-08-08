using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarFoto
{
    public interface IEliminarFoto
    {
        Task Execute(int IdAvistamiento, string foto);
    }
}
