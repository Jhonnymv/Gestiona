using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.SelectItems
{
    public interface ISelectItems
    {
        Task<ItemsSelectResumen> Execute();
        Task<ItemsSelectResumen> ItemVistaResumen();
        Task<ItemsSelectResumen> ItemVistaNuevaInspeccion();
        Task<ItemsSelectResumen> ItemVistaEditarInspeccion();
        Task<ItemsSelectResumen> ItemVistaNuevaObservacion();

    }
}
