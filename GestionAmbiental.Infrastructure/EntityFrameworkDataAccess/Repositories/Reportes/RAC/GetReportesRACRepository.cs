using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.RAC
{
    public class GetReportesRACRepository : IGetReportesRACReadOnlyRepository
    {
        private Context _context;
        public GetReportesRACRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario)
        {
            var reportes = await _context.RACs
                .Where(x=>
                x.Id_Gerencia.Equals(IdGerencia>0?IdGerencia:x.Id_Gerencia)&&
                x.Id_Empresa.Equals(IdEmpresa>0?IdEmpresa:x.Id_Empresa)&&
                x.Id_Responsable_Cierre.Equals(IdUsuario>0?IdUsuario:x.Id_Responsable_Cierre))
                .ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosRAC)))
            {
                output.Add(new Dimension(item, reportes.Where(x => x.Estado.Equals(item)).Count()));
            }

            return output;
        }
        public async Task<List<Dimension>> GetNReportesEntreFechas(int IdGerencia, int IdEmpresa, int IdUsuario,DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var reportes = await _context.RACs
                .Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre)&&
                x.Fecha >= FechaInicio&&
                x.Fecha < FechaFin)
                .ToListAsync();

            var nReportesxFecha = reportes.Select(x => x.Fecha).GroupBy(y => y.Date).ToList();

            int nDias = 0;
            //Calcular n días entre fechas de distinto año (considerando años biciestos)
            if (FechaFin.Year>FechaInicio.Year)
                nDias = FechaFin.DayOfYear + (FechaInicio.Year%4==0?366:365) - FechaInicio.DayOfYear;
            else
                nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;
            var lstDiasSinValor = new List<Dimension>();
            var lstDiasConValor = new List<Dimension>();

            for (int i = nDias; i >= 0; i--)
            {
                lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, i));
            }

            foreach (var item in nReportesxFecha)
            {
                lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), FechaFin.DayOfYear-item.Key.DayOfYear));
            }
            var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y =>y.Nombre).ToList().Contains(x.Nombre)).ToList();
            //var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
            output = distinct.Union(lstDiasConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

            //var union = distinct.Union(output).OrderBy(x=>x.Nombre).ToList();
            return output;
        }
        public async Task<List<Dimension>> GetReportesNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x =>
                x.SSO_MA.Equals("MA") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha < FechaFin)
                .GroupBy(y=>y.NivelRiesgo)
                .ToListAsync();

            var output = new List<Dimension>();
            
            output.Add(new Dimension("Tolerable", 0, 0));
            output.Add(new Dimension("Alerta", 0, 1));
            output.Add(new Dimension("No Aceptable", 0, 2));
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Tolerable": output.FirstOrDefault(x => x.Nombre.Equals("Tolerable")).Valor = item.Count(); break;
                    case "Alerta": output.FirstOrDefault(x => x.Nombre.Equals("Alerta")).Valor = item.Count(); break;
                    case "No Aceptable": output.FirstOrDefault(x => x.Nombre.Equals("No Aceptable")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x =>
                x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha < FechaFin)
                .GroupBy(y => y.NivelRiesgo)
                .ToListAsync();

            var output = new List<Dimension>();

            output.Add(new Dimension("Alto", 0, 0));
            output.Add(new Dimension("Medio", 0, 1));
            output.Add(new Dimension("Bajo", 0, 2));
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Alto": output.FirstOrDefault(x => x.Nombre.Equals("Alto")).Valor = item.Count(); break;
                    case "Medio": output.FirstOrDefault(x => x.Nombre.Equals("Medio")).Valor = item.Count(); break;
                    case "Bajo": output.FirstOrDefault(x => x.Nombre.Equals("Bajo")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesClasificacionMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x =>
                x.SSO_MA.Equals("MA") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha < FechaFin)
                .GroupBy(x=>x.Clasificacion)
                .ToListAsync();

            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));
            
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesClasificacionSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x =>
                x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha < FechaFin)
                .GroupBy(x => x.Clasificacion)
                .ToListAsync();

            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));

            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetReportesTipoCondicionxNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x =>
                x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha < FechaFin)
                .GroupBy(y => y.NivelRiesgo)
                .ToListAsync();
            
            var output = new List<List<Dimension>>();
            foreach(var item in reportes)
            {
                item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
            }
            
            return output;
        }
    }
}
