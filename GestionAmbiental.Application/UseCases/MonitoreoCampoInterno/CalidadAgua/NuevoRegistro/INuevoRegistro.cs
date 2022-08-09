using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            DateTime Fecha,
            string Hora,
            string Cod_Estacion,
            string Descripcion,
            string Ubicacion,
            int Id_Usuario,
            string TemperaturaResult,
            string TemperaturaEquipo,
            string PhResult,
            string PhEquipo,
            string ConductividadResult,
            string ConductividadEquipo,
            string OxigenoResult,
            string OxigenoEquipo,
            string TurbiedadResult,
            string TurbiedadEquipo,
            string CloroResult,
            string CloroEquipo
  
            );
    }
}
