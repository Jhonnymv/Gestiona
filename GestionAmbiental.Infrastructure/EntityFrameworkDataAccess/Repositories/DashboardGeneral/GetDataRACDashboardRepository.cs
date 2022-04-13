using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.RAC;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.DashboardGeneral
{
    public class GetDataRACDashboardRepository: IGetDataRACDashboardRepository
    {
        private readonly Context _context;

        public GetDataRACDashboardRepository(Context context)
        {
            _context = context;
        }

        public async Task<RACMAPSSOOutput> RACActoCondicionBP(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>                
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            racAct = racAct.Where(x => 
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) && 
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = racAct
                .Where(x => x.SSO_MA.Equals("MA"))
                .GroupBy(x => x.Clasificacion)
                .ToList();
            var outputMAP = new List<Dimension> {
                new Dimension("Acto Subestándar", 0, 0),
                new Dimension("Condición Subestándar", 0, 1),
                //new Dimension("Buenas Prácticas", 0, 2),
            };
            foreach (var item in filtradoMAP)
            {
                outputMAP.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = racAct
                .Where(x => x.SSO_MA.Equals("SSO"))
                .GroupBy(x => x.Clasificacion)
                .ToList();
            var outputSSO = new List<Dimension> {
                new Dimension("Acto Subestándar", 0, 0),
                new Dimension("Condición Subestándar", 0, 1),
                //new Dimension("Buenas Prácticas", 0, 2),
            };
            foreach (var item in filtradoSSO)
            {
                outputSSO.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = new List<Dimension> {
                new Dimension("Acto Subestándar", outputMAP.First(x=>x.Nombre.Equals("Acto Subestándar")).Valor + outputSSO.First(x=>x.Nombre.Equals("Acto Subestándar")).Valor , 0),
                new Dimension("Condición Subestándar", outputMAP.First(x=>x.Nombre.Equals("Condición Subestándar")).Valor + outputSSO.First(x=>x.Nombre.Equals("Condición Subestándar")).Valor, 1),
                //new Dimension("Buenas Prácticas", outputMAP.First(x=>x.Nombre.Equals("Buenas Prácticas")).Valor + outputSSO.First(x=>x.Nombre.Equals("Buenas Prácticas")).Valor, 2),
                }
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> RACEstadosCondicion(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = racAct
                .Where(x => x.SSO_MA.Equals("MA") &&
                            x.Clasificacion.Equals("Condición Subestándar"))
                .GroupBy(x => x.Estado)
                .ToList();

            var outputMAP = new List<Dimension> {
                new Dimension("Abierto", 0, 0),
                new Dimension("Cerrado", 0, 1),
                new Dimension("En Plazo", 0, 2),
            };

            foreach (var item in filtradoMAP)
            {
                switch (item.Key)
                {
                    case "Registrado":
                    case "Ampliado":
                    case "Rechazado":
                        outputMAP.FirstOrDefault(x => x.Nombre.Equals("Abierto")).Valor += item.Count();
                        outputMAP.FirstOrDefault(x => x.Nombre.Equals("En Plazo")).Valor += item.Count();
                        break;
                    case "Vencido":
                    case "Implementado":
                        outputMAP.FirstOrDefault(x => x.Nombre.Equals("Abierto")).Valor += item.Count();
                        break;
                    case "Aprobado":
                        outputMAP.FirstOrDefault(x => x.Nombre.Equals("Cerrado")).Valor += item.Count();
                        break;
                    default:
                        break;
                }
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = racAct
                .Where(x => x.SSO_MA.Equals("SSO") &&
                            x.Clasificacion.Equals("Condición Subestándar"))
                .GroupBy(x => x.Estado)
                .ToList();

            var outputSSO = new List<Dimension> {
                new Dimension("Abierto", 0, 0),
                new Dimension("Cerrado", 0, 1),
                new Dimension("En Plazo", 0, 2),
            };

            foreach (var item in filtradoSSO)
            {
                switch (item.Key)
                {
                    case "Registrado":
                    case "Ampliado":
                    case "Rechazado":
                        outputSSO.FirstOrDefault(x => x.Nombre.Equals("Abierto")).Valor += item.Count();
                        outputSSO.FirstOrDefault(x => x.Nombre.Equals("En Plazo")).Valor += item.Count();
                        break;
                    case "Vencido":
                    case "Implementado":
                        outputSSO.FirstOrDefault(x => x.Nombre.Equals("Abierto")).Valor += item.Count();
                        break;
                    case "Aprobado":
                        outputSSO.FirstOrDefault(x => x.Nombre.Equals("Cerrado")).Valor += item.Count();
                        break;
                    default:
                        break;
                }
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = new List<Dimension> {
                new Dimension("Abierto", outputMAP.First(x=>x.Nombre.Equals("Abierto")).Valor + outputSSO.First(x=>x.Nombre.Equals("Abierto")).Valor , 0),
                new Dimension("Cerrado", outputMAP.First(x=>x.Nombre.Equals("Cerrado")).Valor + outputSSO.First(x=>x.Nombre.Equals("Cerrado")).Valor, 1),
                new Dimension("En Plazo", outputMAP.First(x=>x.Nombre.Equals("En Plazo")).Valor + outputSSO.First(x=>x.Nombre.Equals("En Plazo")).Valor, 2),
                }
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> TopActosSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = racAct
                .Where(x => x.SSO_MA.Equals("MA") &&
                            x.Clasificacion.Equals("Acto Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputMAP = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoMAP.Count ? topLimit : filtradoMAP.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoMAP[i].Key));
                outputMAP.Add(new Dimension(tipoCondicion.Descripcion, filtradoMAP[i].Count(), i));
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = racAct
                .Where(x => x.SSO_MA.Equals("SSO") &&
                            x.Clasificacion.Equals("Acto Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputSSO = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoSSO.Count ? topLimit : filtradoSSO.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoSSO[i].Key));
                outputSSO.Add(new Dimension(tipoCondicion.Descripcion, filtradoSSO[i].Count(), i));
            }

            var filtradoTodos = racAct
                .Where(x => x.Clasificacion.Equals("Acto Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x => x.Count())
                .ToList();

            var outputTodos = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoTodos.Count ? topLimit : filtradoTodos.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoTodos[i].Key));
                outputTodos.Add(new Dimension(tipoCondicion.Descripcion, filtradoTodos[i].Count(), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> TopCondicionesSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = racAct
                .Where(x => x.SSO_MA.Equals("MA") &&
                            x.Clasificacion.Equals("Condición Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputMAP = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoMAP.Count ? topLimit : filtradoMAP.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoMAP[i].Key));
                outputMAP.Add(new Dimension(tipoCondicion.Descripcion, filtradoMAP[i].Count(), i));
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = racAct
                .Where(x => x.SSO_MA.Equals("SSO") &&
                            x.Clasificacion.Equals("Condición Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputSSO = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoSSO.Count ? topLimit : filtradoSSO.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoSSO[i].Key));
                outputSSO.Add(new Dimension(tipoCondicion.Descripcion, filtradoSSO[i].Count(), i));
            }

            var filtradoTodos = racAct
                .Where(x => x.Clasificacion.Equals("Condición Subestándar"))
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x => x.Count())
                .ToList();

            var outputTodos = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoTodos.Count ? topLimit : filtradoTodos.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoTodos[i].Key));
                outputTodos.Add(new Dimension(tipoCondicion.Descripcion, filtradoTodos[i].Count(), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> RankingGerenciaReportante(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();

            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var gerencias = await _context.Gerencias.ToListAsync();

            var racMAP = racAct.Where(x => x.SSO_MA.Equals("MA")).GroupBy(x => x.Id_Gerencia)
                .OrderByDescending(x => x.Count())
                .ToList();
            var racSSO = racAct.Where(x => x.SSO_MA.Equals("SSO")).GroupBy(x => x.Id_Gerencia)
                .OrderByDescending(x => x.Count())
                .ToList();

            var outputMAP = new List<Dimension>();
        
            for (int i = 0; i < (topLimit < racMAP.Count ? topLimit : racMAP.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(racMAP[i].Key));
                outputMAP.Add(new Dimension(temp.Descripcion, racMAP[i].Count(), i));
            }

            var outputSSO = new List<Dimension>();

            for (int i = 0; i < (topLimit < racSSO.Count ? topLimit : racSSO.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(racSSO[i].Key));
                outputSSO.Add(new Dimension(temp.Descripcion, racSSO[i].Count(), i));
            }

            var outputTodos = new List<Dimension>();
            var inspeccionesTodos = racAct
                .GroupBy(x => x.Id_Gerencia)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < inspeccionesTodos.Count ? topLimit : inspeccionesTodos.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(inspeccionesTodos[i].Key));
                outputTodos.Add(new Dimension(temp.Descripcion, inspeccionesTodos[i].Count(), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> RankingGerenciaObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var gerencias = await _context.Gerencias.ToListAsync();

            var racVencidas = racAct.Where(x=> x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
            var racMAP = racVencidas.Where(x => x.SSO_MA.Equals("MA")).GroupBy(x=>x.Id_RAC).ToList();
            var racSSO = racVencidas.Where(x => x.SSO_MA.Equals("SSO")).GroupBy(x => x.Id_RAC).ToList();

            var outputMAP = new List<Dimension>();
            var gerenciasMAP = racAct
                .Where(x => racMAP.Select(a => a.Key).Contains(x.Id_RAC))
                .Select(x=>new {
                    Gerencia = gerencias.First(z=>z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = racMAP.First(y=>y.Key.Equals(x.Id_RAC)).Count()})
                .GroupBy(x=>x.Gerencia)
                .OrderByDescending(x => x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasMAP.Count ? topLimit : gerenciasMAP.Count); i++)
            {
                outputMAP.Add(new Dimension(gerenciasMAP[i].Key.Descripcion, gerenciasMAP[i].Sum(x => x.ObsVencidas), i));
            }

            var outputSSO = new List<Dimension>();
            var gerenciasSSO = racAct
                .Where(x => racSSO.Select(x => x.Key).Contains(x.Id_RAC))
                .Select(x => new {
                    Gerencia = gerencias.First(z => z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = racSSO.First(y => y.Key.Equals(x.Id_RAC)).Count()
                })
                .GroupBy(x => x.Gerencia)
                .OrderByDescending(x=>x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasSSO.Count ? topLimit : gerenciasSSO.Count); i++)
            {
                outputSSO.Add(new Dimension(gerenciasSSO[i].Key.Descripcion, gerenciasSSO[i].Sum(x => x.ObsVencidas), i));
            }

            var outputTodos = new List<Dimension>();
            var gerenciasTodos = racAct
                .Where(x => racVencidas.GroupBy(x => x.Id_RAC).Select(x => x.Key).Contains(x.Id_RAC))
                .Select(x => new {
                    Gerencia = gerencias.First(z => z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = racVencidas.GroupBy(x => x.Id_RAC).First(y => y.Key.Equals(x.Id_RAC)).Count()
                })
                .GroupBy(x => x.Gerencia)
                .OrderByDescending(x => x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasTodos.Count ? topLimit : gerenciasTodos.Count); i++)
            {
                outputTodos.Add(new Dimension(gerenciasTodos[i].Key.Descripcion, gerenciasTodos[i].Sum(x => x.ObsVencidas), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> RankingUsuariosIngresoRAC(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();

            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var usuarios = await _context.Usuarios.ToListAsync();
            //Agrupar por campo de BD solicitado para MAP
            var observacionesMAP = racAct
                .Where(x => x.SSO_MA.Equals("MA"))
                .GroupBy(x => x.Id_RAC)
                .ToList();

            var usuariosInspMAP = racAct
                .Where(x => observacionesMAP.Select(y => y.Key).Contains(x.Id_RAC))
                .GroupBy(x=>x.Id_Registrado_Por)
                .OrderByDescending(x=>x.Count())
                .ToList();
            
            var outputMAP = new List<Dimension>();

            for (int i = 0; i < (topLimit < usuariosInspMAP.Count ? topLimit : usuariosInspMAP.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosInspMAP[i].Key));
                outputMAP.Add(new Dimension(usuario.Nombres.ToUpper()+ " "+ usuario.Apellidos.ToUpper(), usuariosInspMAP[i].Count(), i));
            }
            //Agrupar por campo de BD solicitado para SSO
            var observacionesSSO = racAct
                .Where(x => x.SSO_MA.Equals("SSO"))
                .GroupBy(x => x.Id_RAC)
                .ToList();

            var usuariosInspSSO = racAct
                .Where(x => observacionesSSO.Select(y => y.Key).Contains(x.Id_RAC))
                .GroupBy(x=>x.Id_Registrado_Por)
                .OrderByDescending(x=>x.Count())
                .ToList();
            
            var outputSSO = new List<Dimension>();

            for (int i = 0; i < (topLimit < usuariosInspSSO.Count ? topLimit : usuariosInspSSO.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosInspSSO[i].Key));
                outputSSO.Add(new Dimension(usuario.Nombres.ToUpper()+ " "+ usuario.Apellidos.ToUpper(), usuariosInspSSO[i].Count(), i));
            }

            //Agrupar por campo de BD solicitado para Todos
            var observacionesTodos = racAct
                .GroupBy(x => x.Id_RAC)
                .ToList();

            var usuariosInspTodos = racAct
                .Where(x => observacionesTodos.Select(y => y.Key).Contains(x.Id_RAC))
                .GroupBy(x=>x.Id_Registrado_Por)
                .OrderByDescending(x=>x.Count())
                .ToList();
            
            var outputTodos = new List<Dimension>();

            for (int i = 0; i < (topLimit < usuariosInspTodos.Count ? topLimit : usuariosInspTodos.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosInspTodos[i].Key));
                outputTodos.Add(new Dimension(usuario.Nombres.ToUpper()+ " "+ usuario.Apellidos.ToUpper(), usuariosInspTodos[i].Count(), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<RACMAPSSOOutput> RankingUsuariosObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var racAct = await _context.RACs
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();

            racAct = racAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Id_Responsable_Cierre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Cierre) &&
                x.Id_Supervisor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Supervisor) &&
                x.Fecha >= FInicioInsp &&
                x.Fecha <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                racAct = racAct.Where(x => x.Id_Gerencia_Obs.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            if (EstadoInsp != null && EstadoInsp != "")
            {
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        racAct = racAct.Where(x =>
                        //!x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        racAct = racAct.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        racAct = racAct.Where(x =>
                        //x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var usuarios = await _context.Usuarios.ToListAsync();

            var racVencidas = racAct.Where(x => x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
            var racMAP = racVencidas.Where(x => x.SSO_MA.Equals("MA")).ToList();
            var racSSO = racVencidas.Where(x => x.SSO_MA.Equals("SSO")).ToList();

            var outputMAP = new List<Dimension>();
            var usuariosMAP = racMAP
                .GroupBy(x => x.Id_Responsable_Cierre)
                .OrderByDescending(x=>x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosMAP.Count ? topLimit : usuariosMAP.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosMAP[i].Key));
                outputMAP.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosMAP[i].Count(), i));
            }

            var outputSSO = new List<Dimension>();
            var usuariosSSO = racSSO
                .GroupBy(x => x.Id_Responsable_Cierre)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosSSO.Count ? topLimit : usuariosSSO.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosSSO[i].Key));
                outputSSO.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosSSO[i].Count(), i));
            }

            var outputTodos = new List<Dimension>();
            var usuariosTodos = racVencidas
                .GroupBy(x => x.Id_Responsable_Cierre)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosTodos.Count ? topLimit : usuariosTodos.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosTodos[i].Key));
                outputTodos.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosTodos[i].Count(), i));
            }

            var output = new RACMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
    }
}
