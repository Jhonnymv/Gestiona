using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.CompletarAvistamientoFauna
{
    public interface ICompletarAvistamientoFauna
    {
        Task Execute(int IdAvistamiento, string[] evidencias, string descripcion, string UsuarioRed);
    }
}
