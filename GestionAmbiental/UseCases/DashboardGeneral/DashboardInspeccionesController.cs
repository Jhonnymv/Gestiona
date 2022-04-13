using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesActoCondicionBP;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesEstadosCondicion;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaReportante;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosIngresoInspecciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopActosSubestandar;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopCondicionesSubestandar;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardInspeccionesController : Controller
    {
        private readonly IInspeccionesActoCondicionBP _inspeccionesActoCondicionBP;
        private readonly IInspeccionesEstadosCondicion _inspeccionesEstadosCondicion;
        private readonly IRankingGerenciaObsVencidas _rankingGerenciaObsVencidas;
        private readonly IRankingGerenciaReportante _rankingGerenciaReportante;
        private readonly IRankingUsuariosIngresoInspecciones _rankingUsuariosIngresoInspecciones;
        private readonly IRankingUsuariosObsVencidas _rankingUsuariosObsVencidas;
        private readonly ITopActosSubestandar _topActosSubestandar;
        private readonly ITopCondicionesSubestandar _topCondicionesSubestandar;

        private readonly IGetItemsFiltro _getItemsFiltroInspecciones;
        public DashboardInspeccionesController(
            IInspeccionesActoCondicionBP inspeccionesActoCondicionBP,
            IInspeccionesEstadosCondicion inspeccionesEstadosCondicion,
            IRankingGerenciaObsVencidas rankingGerenciaObsVencidas,
            IRankingGerenciaReportante rankingGerenciaReportante,
            IRankingUsuariosIngresoInspecciones rankingUsuariosIngresoInspecciones,
            IRankingUsuariosObsVencidas rankingUsuariosObsVencidas,
            ITopActosSubestandar topActosSubestandar,
            ITopCondicionesSubestandar topCondicionesSubestandar,
            IGetItemsFiltro getItemsFiltroInspecciones)
        {
            _inspeccionesActoCondicionBP = inspeccionesActoCondicionBP;
            _inspeccionesEstadosCondicion = inspeccionesEstadosCondicion;
            _rankingGerenciaObsVencidas = rankingGerenciaObsVencidas;
            _rankingGerenciaReportante = rankingGerenciaReportante;
            _rankingUsuariosIngresoInspecciones = rankingUsuariosIngresoInspecciones;
            _rankingUsuariosObsVencidas = rankingUsuariosObsVencidas;
            _topActosSubestandar = topActosSubestandar;
            _topCondicionesSubestandar = topCondicionesSubestandar;
            _getItemsFiltroInspecciones = getItemsFiltroInspecciones;
        }
        //INICIO INSPECCIONES
        [HttpPost]
        public async Task<IActionResult> InspeccionesActoCondicionBP(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _inspeccionesActoCondicionBP.Execute(IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> InspeccionesEstadosCondicion(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _inspeccionesEstadosCondicion.Execute(IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaObsVencidas(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _rankingGerenciaObsVencidas.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaReportante(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _rankingGerenciaReportante.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosIngresoInspecciones(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _rankingUsuariosIngresoInspecciones.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosObsVencidas(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _rankingUsuariosObsVencidas.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopActosSubestandar(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _topActosSubestandar.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopCondicionesSubestandar(int IdGerenciaInsp, int IdEmpresaInsp, string FInicioInsp, string FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            var data = await _topCondicionesSubestandar.Execute(10, IdGerenciaInsp, IdEmpresaInsp, FInicioInsp, FFinInsp, IdRespLevInsp, IdAsesorInsp, EstadoInsp, IdGerenciaInspeccionadaInsp);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        //FIN INSPECCIONES
    }
}
