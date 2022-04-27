using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RACActoCondicionBP;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RACEstadosCondicion;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RankingGerenciaObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RankingGerenciaReportante;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RankingUsuariosIngresoInspecciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.RankingUsuariosObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.TopActosSubestandar;
using GestionAmbiental.Application.UseCases.DashboardGeneral.RAC.TopCondicionesSubestandar;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardRACController : Controller
    {
        private readonly IRACActoCondicionBP _RACActoCondicionBP;
        private readonly IRACEstadosCondicion _RACEstadosCondicion;
        private readonly IRankingGerenciaObsVencidas _rankingGerenciaObsVencidas;
        private readonly IRankingGerenciaReportante _rankingGerenciaReportante;
        private readonly IRankingUsuariosIngresoRAC _rankingUsuariosIngresoRAC;
        private readonly IRankingUsuariosObsVencidas _rankingUsuariosObsVencidas;
        private readonly ITopActosSubestandar _topActosSubestandar;
        private readonly ITopCondicionesSubestandar _topCondicionesSubestandar;

        public DashboardRACController(
            IRACActoCondicionBP rACActoCondicionBP, 
            IRACEstadosCondicion rACEstadosCondicion, 
            IRankingGerenciaObsVencidas rankingGerenciaObsVencidas,
            IRankingGerenciaReportante rankingGerenciaReportante,
            IRankingUsuariosIngresoRAC rankingUsuariosIngresoRAC,
            IRankingUsuariosObsVencidas rankingUsuariosObsVencidas,
            ITopActosSubestandar topActosSubestandar, 
            ITopCondicionesSubestandar topCondicionesSubestandar)
        {
            _RACActoCondicionBP = rACActoCondicionBP;
            _RACEstadosCondicion = rACEstadosCondicion;
            _rankingGerenciaObsVencidas = rankingGerenciaObsVencidas;
            _rankingGerenciaReportante = rankingGerenciaReportante;
            _rankingUsuariosIngresoRAC = rankingUsuariosIngresoRAC;
            _rankingUsuariosObsVencidas = rankingUsuariosObsVencidas;
            _topActosSubestandar = topActosSubestandar;
            _topCondicionesSubestandar = topCondicionesSubestandar;
        }

        //INICIO RAC
        [HttpPost]
        public async Task<IActionResult> RACActoCondicionBP(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _RACActoCondicionBP.Execute(IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RACEstadosCondicion(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _RACEstadosCondicion.Execute(IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaObsVencidas(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _rankingGerenciaObsVencidas.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaReportante(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _rankingGerenciaReportante.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosIngresoRAC(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _rankingUsuariosIngresoRAC.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosObsVencidas(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _rankingUsuariosObsVencidas.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopActosSubestandar(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _topActosSubestandar.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopCondicionesSubestandar(int IdGerenciaRAC, int IdEmpresaRAC, string FInicioRAC, string FFinRAC, int IdRespLevRAC, int IdAsesorRAC, string EstadoRAC, int IdGerenciaInspeccionadaRAC)
        {
            var data = await _topCondicionesSubestandar.Execute(10, IdGerenciaRAC, IdEmpresaRAC, FInicioRAC, FFinRAC, IdRespLevRAC, IdAsesorRAC, EstadoRAC, IdGerenciaInspeccionadaRAC);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        //FIN RAC
    }
}
