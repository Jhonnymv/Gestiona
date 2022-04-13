using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.Inspecciones;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.DashboardGeneral
{
    public class GetDataInspeccionesDashboardRepository: IGetDataInspeccionesDashboardRepository
    {
        private readonly Context _context;

        public GetDataInspeccionesDashboardRepository(Context context)
        {
            _context = context;
        }

        public async Task<InspeccionesMAPSSOOutput> InspeccionesActoCondicionBP(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>                
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x => 
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FInicioInsp &&
                x.Fecha_Inspeccion <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();
            //Obtener lista de observaciones de las inspecciones activas
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            //Agregar mas filtros si es necesario
            observaciones = observaciones
                .Where(x =>
                inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)&&
                x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
                .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = observaciones
                .Where(x => x.Tipo_Observacion.Equals("MAP"))
                .GroupBy(x => x.Descripcion_Condicion)
                .ToList();
            var outputMAP = new List<Dimension> {
                new Dimension("Acto Subestándar", 0, 0),
                new Dimension("Condición Subestándar", 0, 1),
                new Dimension("Buenas Prácticas", 0, 2),
            };
            foreach (var item in filtradoMAP)
            {
                outputMAP.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = observaciones
                .Where(x => x.Tipo_Observacion.Equals("SSO"))
                .GroupBy(x => x.Descripcion_Condicion)
                .ToList();
            var outputSSO = new List<Dimension> {
                new Dimension("Acto Subestándar", 0, 0),
                new Dimension("Condición Subestándar", 0, 1),
                new Dimension("Buenas Prácticas", 0, 2),
            };
            foreach (var item in filtradoSSO)
            {
                outputSSO.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = new List<Dimension> {
                new Dimension("Acto Subestándar", outputMAP.First(x=>x.Nombre.Equals("Acto Subestándar")).Valor + outputSSO.First(x=>x.Nombre.Equals("Acto Subestándar")).Valor , 0),
                new Dimension("Condición Subestándar", outputMAP.First(x=>x.Nombre.Equals("Condición Subestándar")).Valor + outputSSO.First(x=>x.Nombre.Equals("Condición Subestándar")).Valor, 1),
                new Dimension("Buenas Prácticas", outputMAP.First(x=>x.Nombre.Equals("Buenas Prácticas")).Valor + outputSSO.First(x=>x.Nombre.Equals("Buenas Prácticas")).Valor, 2),
                }
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> InspeccionesEstadosCondicion(int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FInicioInsp &&
                x.Fecha_Inspeccion <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();
            //Obtener lista de observaciones de las inspecciones activas
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            //Agregar mas filtros si es necesario
            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = observaciones
                .Where(x => x.Tipo_Observacion.Equals("MAP") &&
                            x.Descripcion_Condicion.Equals("Condición Subestándar"))
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
            var filtradoSSO = observaciones
                .Where(x => x.Tipo_Observacion.Equals("SSO") &&
                            x.Descripcion_Condicion.Equals("Condición Subestándar"))
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

            var output = new InspeccionesMAPSSOOutput()
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
        public async Task<InspeccionesMAPSSOOutput> TopActosSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FInicioInsp &&
                x.Fecha_Inspeccion <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            //Obtener lista de observaciones de las inspecciones activas
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            //Agregar mas filtros si es necesario
            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = observaciones
                .Where(x => x.Tipo_Observacion.Equals("MAP") &&
                            x.Descripcion_Condicion.Equals("Acto Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputMAP = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoMAP.Count ? topLimit : filtradoMAP.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoMAP[i].Key.Value));
                outputMAP.Add(new Dimension(tipoCondicion.Descripcion, filtradoMAP[i].Count(), i));
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = observaciones
                .Where(x => x.Tipo_Observacion.Equals("SSO") &&
                            x.Descripcion_Condicion.Equals("Acto Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputSSO = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoSSO.Count ? topLimit : filtradoSSO.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoSSO[i].Key.Value));
                outputSSO.Add(new Dimension(tipoCondicion.Descripcion, filtradoSSO[i].Count(), i));
            }

            var filtradoTodos = observaciones
                .Where(x => x.Descripcion_Condicion.Equals("Acto Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x => x.Count())
                .ToList();

            var outputTodos = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoTodos.Count ? topLimit : filtradoTodos.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoTodos[i].Key.Value));
                outputTodos.Add(new Dimension(tipoCondicion.Descripcion, filtradoTodos[i].Count(), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> TopCondicionesSubestandar(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FInicioInsp &&
                x.Fecha_Inspeccion <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            //Obtener lista de observaciones de las inspecciones activas
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            //Agregar mas filtros si es necesario
            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            //Agrupar por campo de BD solicitado para MAP
            var filtradoMAP = observaciones
                .Where(x => x.Tipo_Observacion.Equals("MAP") &&
                            x.Descripcion_Condicion.Equals("Condición Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputMAP = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoMAP.Count ? topLimit : filtradoMAP.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoMAP[i].Key.Value));
                outputMAP.Add(new Dimension(tipoCondicion.Descripcion, filtradoMAP[i].Count(), i));
            }

            //Agrupar por campo de BD solicitado para SSO
            var filtradoSSO = observaciones
                .Where(x => x.Tipo_Observacion.Equals("SSO") &&
                            x.Descripcion_Condicion.Equals("Condición Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x=>x.Count())
                .ToList();

            var outputSSO = new List<Dimension>();
            for(int i = 0; i < (topLimit < filtradoSSO.Count ? topLimit : filtradoSSO.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoSSO[i].Key.Value));
                outputSSO.Add(new Dimension(tipoCondicion.Descripcion, filtradoSSO[i].Count(), i));
            }

            var filtradoTodos = observaciones
                .Where(x => x.Descripcion_Condicion.Equals("Condición Subestándar") &&
                            x.Id_Tipo_Condicion.HasValue)
                //int? para buenas practicas
                .GroupBy(x => x.Id_Tipo_Condicion)
                .OrderByDescending(x => x.Count())
                .ToList();

            var outputTodos = new List<Dimension>();
            for (int i = 0; i < (topLimit < filtradoTodos.Count ? topLimit : filtradoTodos.Count); i++)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(filtradoTodos[i].Key.Value));
                outputTodos.Add(new Dimension(tipoCondicion.Descripcion, filtradoTodos[i].Count(), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> RankingGerenciaReportante(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();

            inspeccionesAct = inspeccionesAct.Where(x =>
            x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
                //x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FInicioInsp &&
                x.Fecha_Inspeccion <= FFinInsp)
                .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            var gerencias = await _context.Gerencias.ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();

            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            var observacionesMAP = observaciones.Where(x => x.Tipo_Observacion.Equals("MAP")).ToList();
            var observacionesSSO = observaciones.Where(x => x.Tipo_Observacion.Equals("SSO")).ToList();

            var outputMAP = new List<Dimension>();
            var inspeccionesMAP = inspeccionesAct
                .Where(x => observacionesMAP.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .GroupBy(x=>x.Id_Gerencia)
                .OrderByDescending(x=>x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < inspeccionesMAP.Count ? topLimit : inspeccionesMAP.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(inspeccionesMAP[i].Key));
                outputMAP.Add(new Dimension(temp.Descripcion, inspeccionesMAP[i].Count(), i));
            }

            var outputSSO = new List<Dimension>();
            var inspeccionesSSO = inspeccionesAct
                .Where(x => observacionesSSO.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .GroupBy(x => x.Id_Gerencia)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < inspeccionesSSO.Count ? topLimit : inspeccionesSSO.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(inspeccionesSSO[i].Key));
                outputSSO.Add(new Dimension(temp.Descripcion, inspeccionesSSO[i].Count(), i));
            }

            var outputTodos = new List<Dimension>();
            var inspeccionesTodos = inspeccionesAct
                .GroupBy(x => x.Id_Gerencia)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < inspeccionesTodos.Count ? topLimit : inspeccionesTodos.Count); i++)
            {
                var temp = gerencias.First(x => x.Id_Gerencia.Equals(inspeccionesTodos[i].Key));
                outputTodos.Add(new Dimension(temp.Descripcion, inspeccionesTodos[i].Count(), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> RankingGerenciaObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x =>
           x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
               //x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
               x.Fecha_Inspeccion >= FInicioInsp &&
               x.Fecha_Inspeccion <= FFinInsp)
               .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            var gerencias = await _context.Gerencias.ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();

            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }

            var observacionesVencidas = observaciones.Where(x=> x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
            var observacionesMAP = observacionesVencidas.Where(x => x.Tipo_Observacion.Equals("MAP")).GroupBy(x=>x.Id_Inspeccion_Cabecera).ToList();
            var observacionesSSO = observacionesVencidas.Where(x => x.Tipo_Observacion.Equals("SSO")).GroupBy(x => x.Id_Inspeccion_Cabecera).ToList();

            var outputMAP = new List<Dimension>();
            var gerenciasMAP = inspeccionesAct
                .Where(x => observacionesMAP.Select(a => a.Key).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x=>new {
                    Gerencia = gerencias.First(z=>z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = observacionesMAP.First(y=>y.Key.Equals(x.Id_Inspeccion_Cabecera)).Count()})
                .GroupBy(x=>x.Gerencia)
                .OrderByDescending(x => x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasMAP.Count ? topLimit : gerenciasMAP.Count); i++)
            {
                outputMAP.Add(new Dimension(gerenciasMAP[i].Key.Descripcion, gerenciasMAP[i].Sum(x => x.ObsVencidas), i));
            }

            var outputSSO = new List<Dimension>();
            var gerenciasSSO = inspeccionesAct
                .Where(x => observacionesSSO.Select(x => x.Key).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => new {
                    Gerencia = gerencias.First(z => z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = observacionesSSO.First(y => y.Key.Equals(x.Id_Inspeccion_Cabecera)).Count()
                })
                .GroupBy(x => x.Gerencia)
                .OrderByDescending(x=>x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasSSO.Count ? topLimit : gerenciasSSO.Count); i++)
            {
                outputSSO.Add(new Dimension(gerenciasSSO[i].Key.Descripcion, gerenciasSSO[i].Sum(x => x.ObsVencidas), i));
            }

            var outputTodos = new List<Dimension>();
            var gerenciasTodos = inspeccionesAct
                .Where(x => observacionesVencidas.GroupBy(x => x.Id_Inspeccion_Cabecera).Select(x => x.Key).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => new {
                    Gerencia = gerencias.First(z => z.Id_Gerencia.Equals(x.Id_Gerencia)),
                    ObsVencidas = observacionesVencidas.GroupBy(x => x.Id_Inspeccion_Cabecera).First(y => y.Key.Equals(x.Id_Inspeccion_Cabecera)).Count()
                })
                .GroupBy(x => x.Gerencia)
                .OrderByDescending(x => x.Sum(y => y.ObsVencidas))
                .ToList();

            for (int i = 0; i < (topLimit < gerenciasTodos.Count ? topLimit : gerenciasTodos.Count); i++)
            {
                outputTodos.Add(new Dimension(gerenciasTodos[i].Key.Descripcion, gerenciasTodos[i].Sum(x => x.ObsVencidas), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> RankingUsuariosIngresoInspecciones(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            
            inspeccionesAct = inspeccionesAct.Where(x =>
              x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
              x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
              x.Fecha_Inspeccion >= FInicioInsp &&
              x.Fecha_Inspeccion <= FFinInsp)
              .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();
            var usuarios = await _context.Usuarios.ToListAsync();

            //Obtener lista de observaciones de las inspecciones activas
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            //Agrupar por campo de BD solicitado para MAP
            var observacionesMAP = observaciones
                .Where(x => x.Tipo_Observacion.Equals("MAP"))
                .GroupBy(x => x.Id_Inspeccion_Cabecera)
                .ToList();

            var usuariosInspMAP = inspeccionesAct
                .Where(x => observacionesMAP.Select(y => y.Key).Contains(x.Id_Inspeccion_Cabecera))
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
            var observacionesSSO = observaciones
                .Where(x => x.Tipo_Observacion.Equals("SSO"))
                .GroupBy(x => x.Id_Inspeccion_Cabecera)
                .ToList();

            var usuariosInspSSO = inspeccionesAct
                .Where(x => observacionesSSO.Select(y => y.Key).Contains(x.Id_Inspeccion_Cabecera))
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
            var observacionesTodos = observaciones
                .GroupBy(x => x.Id_Inspeccion_Cabecera)
                .ToList();

            var usuariosInspTodos = inspeccionesAct
                .Where(x => observacionesTodos.Select(y => y.Key).Contains(x.Id_Inspeccion_Cabecera))
                .GroupBy(x=>x.Id_Registrado_Por)
                .OrderByDescending(x=>x.Count())
                .ToList();
            
            var outputTodos = new List<Dimension>();

            for (int i = 0; i < (topLimit < usuariosInspTodos.Count ? topLimit : usuariosInspTodos.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosInspTodos[i].Key));
                outputTodos.Add(new Dimension(usuario.Nombres.ToUpper()+ " "+ usuario.Apellidos.ToUpper(), usuariosInspTodos[i].Count(), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
        public async Task<InspeccionesMAPSSOOutput> RankingUsuariosObsVencidas(int topLimit, int IdGerenciaInsp, int IdEmpresaInsp, DateTime FInicioInsp, DateTime FFinInsp, int IdRespLevInsp, int IdAsesorInsp, string EstadoInsp, int IdGerenciaInspeccionadaInsp)
        {
            //Obtener lista de inspecciones con código (activas)
            var inspeccionesAct = await _context.Inspecciones_Cabecera
                .Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                .ToListAsync();
            inspeccionesAct = inspeccionesAct.Where(x =>
             x.Id_Empresa.Equals(IdEmpresaInsp > 0 ? IdEmpresaInsp : x.Id_Empresa) &&
             x.Id_Gerencia.Equals(IdGerenciaInsp > 0 ? IdGerenciaInsp : x.Id_Gerencia) &&
             x.Fecha_Inspeccion >= FInicioInsp &&
             x.Fecha_Inspeccion <= FFinInsp)
             .ToList();

            if (IdGerenciaInspeccionadaInsp != 0)
                inspeccionesAct = inspeccionesAct.Where(x => x.Id_Gerencia_Inspeccionada.Equals(IdGerenciaInspeccionadaInsp)).ToList();

            var usuarios = await _context.Usuarios.ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            observaciones = observaciones
               .Where(x =>
               inspeccionesAct.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
               x.Id_Responsable_Marcobre.Equals(IdRespLevInsp > 0 ? IdRespLevInsp : x.Id_Responsable_Marcobre) &&
               x.Id_Asesor.Equals(IdAsesorInsp > 0 ? IdAsesorInsp : x.Id_Asesor))
               .ToList();
            if (EstadoInsp != null && EstadoInsp != "")
            {

                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (EstadoInsp)
                {
                    case "Pendiente":
                        observaciones = observaciones.Where(x =>
                        !x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado":
                        observaciones = observaciones.Where(x =>
                        x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado":
                        observaciones = observaciones.Where(x =>
                        x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                        x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var observacionesVencidas = observaciones.Where(x => x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
            var observacionesMAP = observacionesVencidas.Where(x => x.Tipo_Observacion.Equals("MAP")).ToList();
            var observacionesSSO = observacionesVencidas.Where(x => x.Tipo_Observacion.Equals("SSO")).ToList();

            var outputMAP = new List<Dimension>();
            var usuariosMAP = observacionesMAP
                .GroupBy(x => x.Id_Responsable_Marcobre)
                .OrderByDescending(x=>x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosMAP.Count ? topLimit : usuariosMAP.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosMAP[i].Key));
                outputMAP.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosMAP[i].Count(), i));
            }

            var outputSSO = new List<Dimension>();
            var usuariosSSO = observacionesSSO
                .GroupBy(x => x.Id_Responsable_Marcobre)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosSSO.Count ? topLimit : usuariosSSO.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosSSO[i].Key));
                outputSSO.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosSSO[i].Count(), i));
            }

            var outputTodos = new List<Dimension>();
            var usuariosTodos = observacionesVencidas
                .GroupBy(x => x.Id_Responsable_Marcobre)
                .OrderByDescending(x => x.Count())
                .ToList();

            for (int i = 0; i < (topLimit < usuariosTodos.Count ? topLimit : usuariosTodos.Count); i++)
            {
                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(usuariosTodos[i].Key));
                outputTodos.Add(new Dimension(usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper(), usuariosTodos[i].Count(), i));
            }

            var output = new InspeccionesMAPSSOOutput()
            {
                ResultadosMAP = outputMAP,
                ResultadosSSO = outputSSO,
                ResultadosTodos = outputTodos
            };

            return output;
        }
    }
}
