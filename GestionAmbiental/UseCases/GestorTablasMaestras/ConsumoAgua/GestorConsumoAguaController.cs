using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAguaDescarga;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetRegistroRecienteConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAguaMasivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.AgregarFuenteAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.EditarFuenteAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.GetFuenteAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.GetFuentesAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.AgregarOrigenAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.EditarOrigenAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenesAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.AgregarResolucion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.EditarResolucion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResolucion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResoluciones;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.AgregarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.DeleteUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.EditarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.GetUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.GetUsuarios;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua
{
    public class GestorConsumoAguaController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAgregarFuenteAgua _agregarFuenteAgua;
        private readonly IEditarFuenteAgua _editarFuenteAgua;
        private readonly IGetFuenteAgua _getFuenteAgua;
        private readonly IGetFuentesAgua _getFuentesAgua;

        private readonly IAgregarOrigenAgua _agregarOrigenAgua;
        private readonly IEditarOrigenAgua _editarOrigenAgua;
        private readonly IGetOrigenAgua _getOrigenAgua;
        private readonly IGetOrigenesAgua _getOrigenesAgua;

        private readonly IAgregarResolucion _agregarResolucion;
        private readonly IEditarResolucion _editarResolucion;
        private readonly IGetResolucion _getResolucion;
        private readonly IGetResoluciones _getResoluciones;

        private readonly IAgregarUsuario _agregarUsuario;
        private readonly IDeleteUsuario _deleteUsuario;
        private readonly IEditarUsuario _editarUsuario;
        private readonly IGetUsuario _getUsuario;
        private readonly IGetUsuarios _getUsuariosCA;


        private readonly IGetConsumosAgua _getConsumosAgua;
        private readonly IGetConsumosAguaDescarga _getConsumosAguaFechas;

        private readonly Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios.IGetUsuarios _getUsuarios;

        private readonly IRegistrarConsumoAguaMasivo _registrarConsumoAguaMasivo;
        private readonly IGetRegistrosRecientesConsumoAgua _getRegistrosRecientesConsumoAgua;

        private ExcelLecturaEscritura _excelLecturaEscritura;
        private readonly ISelectItems _selectItems;

        public GestorConsumoAguaController(
            IHostingEnvironment hostingEnvironment,
            IAgregarFuenteAgua agregarFuenteAgua, 
            IEditarFuenteAgua editarFuenteAgua,
            IGetFuenteAgua getFuenteAgua,
            IGetFuentesAgua getFuentesAgua,
            IAgregarOrigenAgua agregarOrigenAgua,
            IEditarOrigenAgua editarOrigenAgua,
            IGetOrigenAgua getOrigenAgua, 
            IGetOrigenesAgua getOrigenesAgua,
            IAgregarResolucion agregarResolucion, 
            IEditarResolucion editarResolucion, 
            IGetResolucion getResolucion, 
            IGetResoluciones getResoluciones,
            IRegistrarConsumoAguaMasivo registrarConsumoAguaMasivo,
            IGetRegistrosRecientesConsumoAgua getRegistrosRecientesConsumoAgua,
            IGetConsumosAguaDescarga getConsumosAguaFechas,
            IAgregarUsuario agregarUsuario,
            IDeleteUsuario deleteUsuario,
            IEditarUsuario editarUsuario,
            IGetUsuario getUsuario,
            IGetUsuarios getUsuariosCA,
            Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios.IGetUsuarios getUsuarios,
            ISelectItems selectItems)
        {
            _hostingEnvironment = hostingEnvironment;
            _agregarFuenteAgua = agregarFuenteAgua;
            _editarFuenteAgua = editarFuenteAgua;
            _getFuenteAgua = getFuenteAgua;
            _getFuentesAgua = getFuentesAgua;
            _agregarOrigenAgua = agregarOrigenAgua;
            _editarOrigenAgua = editarOrigenAgua;
            _getOrigenAgua = getOrigenAgua;
            _getOrigenesAgua = getOrigenesAgua;
            _agregarResolucion = agregarResolucion;
            _editarResolucion = editarResolucion;
            _getResolucion = getResolucion;
            _getResoluciones = getResoluciones;
            _getRegistrosRecientesConsumoAgua = getRegistrosRecientesConsumoAgua;
            _getConsumosAguaFechas = getConsumosAguaFechas;
            _registrarConsumoAguaMasivo = registrarConsumoAguaMasivo;
            _selectItems = selectItems;

            _agregarUsuario = agregarUsuario;
            _deleteUsuario = deleteUsuario;
            _editarUsuario = editarUsuario;
            _getUsuario = getUsuario;
            _getUsuariosCA = getUsuariosCA;
            _getUsuarios = getUsuarios;
        }

        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                return View();
            }
        }
        //Cargar datos de Excel
        public async Task<IActionResult> CargaDataExcel(string message, string result)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _selectItems.GetSelectItems();

                var actividades = new List<SelectListItem>();
                foreach (var grupo in items.ItemsOrigenes)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    actividades.AddRange(items.ItemsResoluciones.Where(y => y.Estado.Equals("ACT") && y.FuenteAgua.OrigenAgua.IdOrigenAgua.Equals(grupo.IdOrigenAgua)).OrderBy(x => x.Resolucion).Select(i => new SelectListItem() { Value = i.IdResolucionConsumoAgua.ToString(), Text = i.Actividad, Group = nuevoGrupo }).ToList());
                }

                 items.ItemsResoluciones.Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
                actividades.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var resolucionesAct = items.ItemsResoluciones.Where(x => x.Estado.Equals("ACT")).Select(x => new { x.IdResolucionConsumoAgua, x.Resolucion }).ToArray();

                ViewBag.Actividades = actividades;
                ViewBag.ResolucionesAct = JsonConvert.SerializeObject(resolucionesAct);
                ViewBag.Result = result;
                ViewBag.Message = message;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CargaDataExcel(CargaExcelModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var stream = model.Excel.OpenReadStream();
                _excelLecturaEscritura = new ExcelLecturaEscritura(stream, model.Pagina, model.FilaInicio, model.ColumnaInicio);

                if (!_excelLecturaEscritura.ValidarExistenciaPagina())
                    return RedirectToAction("CargaDataExcel", new { message = "Ocurrió un error: No se encontró la hoja en el archivo Excel", result = "Error" });

                var data = _excelLecturaEscritura.ObtenerDatos();

                var dataFechas = await _getConsumosAguaFechas.ExecuteFechas(data.First().Fecha.ToString(), data.Last().Fecha.ToString(), model.IdResolucion);

                var validacion = await ValidarDatosExcel(data, dataFechas, model.IdResolucion, model.FilaInicio);

                if (validacion.Length == 0)
                {
                    var input = new List<Application.UseCases.ConsumoAgua.ConsumoAguaInput>();

                    foreach (var consumo in data)
                    {
                        if (consumo.TiempoBombeo != 0 && consumo.LecturaInicial != 0 && consumo.LecturaFinal != 0)
                        {
                            input.Add(new Application.UseCases.ConsumoAgua.ConsumoAguaInput
                            {
                                IdResolucionConsumoAgua = model.IdResolucion,
                                Fecha = consumo.Fecha,
                                TiempoBombeo = consumo.TiempoBombeo,
                                LecturaInicial = consumo.LecturaInicial,
                                LecturaFinal = consumo.LecturaFinal,
                                VolumenBombeado = consumo.LecturaFinal - consumo.LecturaInicial,
                                CaudalPromedio = (consumo.LecturaFinal - consumo.LecturaInicial) / (consumo.TiempoBombeo * 3600)
                            });
                        }
                    }
                    var agregados = await _registrarConsumoAguaMasivo.Execute(input, userInfo.NombreUsuario);
                    return RedirectToAction("CargaDataExcel", new { message = "Se agregaron " + agregados + " registros", result = "Success" });
                }
                else
                {
                    return RedirectToAction("CargaDataExcel", new { message = "Ocurrió un error: \n" + validacion, result = "Error" });
                }
            }            
            return RedirectToAction("CargaDataExcel", new { message = "" });
        }

        private async Task<string> ValidarDatosExcel(List<CargaExcel.ConsumoAguaModel> consumos, List<Application.UseCases.ConsumoAgua.ConsumoAguaOutput> consumosFecha, int resolucion, int fila)
        {
            var result = "";
            //Ordenar por fechas
            //consumos = consumos.OrderByDescending(x => x.Fecha).ToList();

            //Obtener ultimo registro de la resolucion
            var resoluciones = new List<int>();
            resoluciones.Add(resolucion);
            var ultimoRegistro = await _getRegistrosRecientesConsumoAgua.Execute(resoluciones.ToArray());

            //Si no hay fechas registradas en el periodo de tiempo
            if (consumosFecha.Count == 0)
            {
                //Validar que la suma de horas de fechas repetidas no sean mayores a 24
                var consumosFechas = consumos.GroupBy(x => x.Fecha).ToList();
                foreach (var item in consumosFechas)
                {
                    var sumaTiempos = item.Sum(x => x.TiempoBombeo);
                    if (sumaTiempos > 24 || sumaTiempos < 0)
                        result += "La suma de horas de la fecha " + item.Key.ToString("dd/MM/yyyy") + " supera las 24 horas o es negativo\n";
                }
            }
            //Hay fechas registradas en el periodo de tiempo
            else
            {
                //Validar que la suma de horas de fechas repetidas (Excel y registradas) no sean mayores a 24
                var consumosFechas = consumos.GroupBy(x => x.Fecha).ToList();
                var consumosFechasReg = consumosFecha.GroupBy(x => x.Fecha).ToList();
                foreach (var item in consumosFechas)
                {
                    //Buscar si existe fechas registradas iguales a las del Excel
                    var registradoFecha = consumosFechasReg.FirstOrDefault(x => x.Key.Equals(item.Key));
                    //Sumar lo tiempos del Excel   
                    var sumaTiempos = item.Sum(x => x.TiempoBombeo);
                    //Si existe fechas registradas iguales a las del Excel, sumar a los tiempos del Excel
                    if (registradoFecha != null)
                        sumaTiempos += registradoFecha.Sum(x => x.TiempoBombeo);
                    if (sumaTiempos > 24 || sumaTiempos < 0)
                        result += "La suma de horas de la fecha " + item.Key.ToString("dd/MM/yyyy") + " del Excel ("+ item.Sum(x => x.TiempoBombeo) + "h) y los registros (" + registradoFecha.Sum(x => x.TiempoBombeo) + "h) supera las 24 horas o es negativo\n";
                }
            }

            //Validar fechas mayores a la última registrada
            if (ultimoRegistro.Count > 0 && consumos[0].Fecha <= ultimoRegistro.First().Fecha)
                result += "La fecha de la fila " + fila + " no es mayor o igual a la fecha del último registro correspondiente a esta resolución (" + ultimoRegistro.First().Fecha.ToString("dd/MM/yyyy") + ")\n";

            //Validar lectura inicial del primer registro del excel es igual al final del ultimo registro si existe
            if (ultimoRegistro.Count > 0 && consumos[0].LecturaInicial != ultimoRegistro.First().LecturaFinal)
                result += "La lectura inicial de la fila " + fila + " no es igual a la lectura final del último registro correspondiente a esta resolución (" + ultimoRegistro.First().LecturaFinal + "m3)\n";
            
            for (int i = 0; i < consumos.Count; i++)
            {
                //Validar lectura inicial igual al final anterior
                if (i != 0 && consumos[i].LecturaInicial != consumos[i - 1].LecturaFinal)
                    result += "La lectura inicial de la fila " + (fila + i) + " no es igual a la lectura final anterior\n";

            }
            return result;
        }

        //Origen Agua
        [HttpPost]
        public async Task<IActionResult> FiltrarOrigenAgua(string filtro, int pagina, int rpp)
        {
            var data = await _getOrigenesAgua.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NOrigenes % rpp != 0 ? (data.NOrigenes / rpp) + 1 : data.NOrigenes / rpp;
            if (data.NOrigenes > 0)
            {
                foreach (var fila in data.Origenes)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorConsumoAgua/DetalleOrigenAgua?IdOrigenAgua=" + fila.IdOrigenAgua + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarOrigenAgua(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarOrigenAgua(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarOrigenAgua(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarOrigenAgua(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleOrigenAgua(int IdOrigenAgua)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var data = await _getOrigenAgua.Execute(IdOrigenAgua);
                var model = new OrigenAguaModel();
                model.IdOrigenAgua = data.IdOrigenAgua;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarOrigenAgua(OrigenAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarOrigenAgua.Execute(model.IdOrigenAgua, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleOrigenAgua", new { IdOrigenAgua = model.IdOrigenAgua });
            }
        }
        public async Task<IActionResult> AgregarOrigenAgua()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarOrigenAgua(OrigenAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    if (ModelState.IsValid)
                    {
                        var idOrigenAgua = await _agregarOrigenAgua.Execute(model.Descripcion);
                        return RedirectToAction("DetalleOrigenAgua", new { IdOrigenAgua = idOrigenAgua });
                    }
                    return RedirectToAction("AgregarOrigenAgua");
                }
            }
        }

        //Fuente Agua
        [HttpPost]
        public async Task<IActionResult> FiltrarFuenteAgua(string filtro, int pagina, int rpp)
        {
            var data = await _getFuentesAgua.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Corrdenadas UTM</th>";
            tabla += "<th>Origen</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NFuentes % rpp != 0 ? (data.NFuentes / rpp) + 1 : data.NFuentes / rpp;
            if (data.NFuentes > 0)
            {
                foreach (var fila in data.Fuentes)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Coordenadas_UTM + "</td>";
                    filasTabla += "<td>" + fila.OrigenAgua + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorConsumoAgua/DetalleFuenteAgua?IdFuenteAgua=" + fila.IdFuenteAgua + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarFuenteAgua(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarFuenteAgua(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarFuenteAgua(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarFuenteAgua(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleFuenteAgua(int IdFuenteAgua)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getOrigenesAgua.Execute();
                var origenes = items.Origenes.Select(g => new SelectListItem() { Value = g.IdOrigenAgua.ToString(), Text = g.Descripcion, Disabled = g.Estado.Equals("INA")}).ToList();
                origenes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.OrigenesAgua = origenes;

                var data = await _getFuenteAgua.Execute(IdFuenteAgua);
                var model = new FuenteAguaModel();
                model.IdFuenteAgua = data.IdFuenteAgua;
                model.Descripcion = data.Descripcion;
                model.Coordenadas_UTM = data.Coordenadas_UTM;
                model.IdOrigenAgua = data.IdOrigenAgua;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarFuenteAgua(FuenteAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarFuenteAgua.Execute(model.IdFuenteAgua, model.Descripcion,model.Coordenadas_UTM,model.IdOrigenAgua, model.Estado);
                return RedirectToAction("DetalleFuenteAgua", new { IdFuenteAgua = model.IdFuenteAgua });
            }
        }
        public async Task<IActionResult> AgregarFuenteAgua()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getOrigenesAgua.Execute();
                var origenes = items.Origenes.Select(g => new SelectListItem() { Value = g.IdOrigenAgua.ToString(), Text = g.Descripcion, Disabled = g.Estado.Equals("INA") }).ToList();
                origenes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.OrigenesAgua = origenes;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarFuenteAgua(FuenteAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    if (ModelState.IsValid)
                    {
                        var idFuenteAgua = await _agregarFuenteAgua.Execute(model.Descripcion,model.Coordenadas_UTM,model.IdOrigenAgua);
                        return RedirectToAction("DetalleFuenteAgua", new { IdFuenteAgua = idFuenteAgua });
                    }
                    return RedirectToAction("AgregarFuenteAgua");
                }
            }
        }

        //Resolucion Consumo Agua
        [HttpPost]
        public async Task<IActionResult> FiltrarResolucionConsumoAgua(string filtro, int pagina, int rpp)
        {
            var data = await _getResoluciones.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Resolución</th>";
            tabla += "<th>Año</th>";
            tabla += "<th>Tipo de Uso</th>";
            tabla += "<th>Clase de Derecho</th>";
            tabla += "<th>Fuente de Agua</th>";
            tabla += "<th>Volumen Otorgado</th>";
            tabla += "<th>Actividad</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NResoluciones % rpp != 0 ? (data.NResoluciones / rpp) + 1 : data.NResoluciones / rpp;
            if (data.NResoluciones > 0)
            {
                foreach (var fila in data.Resoluciones)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Resolucion + "</td>";
                    filasTabla += "<td>" + fila.Anio + "</td>";
                    filasTabla += "<td>" + fila.TipoUso + "</td>";
                    filasTabla += "<td>" + fila.ClaseDerecho + "</td>";
                    filasTabla += "<td>" + fila.FuenteAgua + "</td>";
                    filasTabla += "<td>" + fila.VolumenOtorgado + "</td>";
                    filasTabla += "<td>" + fila.Actividad + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorConsumoAgua/DetalleResolucionConsumoAgua?IdResolucionConsumoAgua=" + fila.IdResolucionConsumoAgua + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarResolucionConsumoAgua(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarResolucionConsumoAgua(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResolucionConsumoAgua(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarResolucionConsumoAgua(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleResolucionConsumoAgua(int IdResolucionConsumoAgua)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getFuentesAgua.Execute();
                var fuentes = items.Fuentes.Select(g => new SelectListItem() { Value = g.IdFuenteAgua.ToString(), Text = g.Descripcion, Disabled = g.Estado.Equals("INA") }).ToList();
                fuentes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.FuentesAgua = fuentes;

                var data = await _getResolucion.Execute(IdResolucionConsumoAgua);
                var model = new ResolucionConsumoAguaModel();
                model.IdResolucionConsumoAgua = data.IdResolucionConsumoAgua;
                model.Resolucion = data.Resolucion;
                model.Actividad = data.Actividad;
                model.Anio = data.Anio;
                model.ClaseDerecho = data.ClaseDerecho;
                model.IdFuenteAgua = data.IdFuenteAgua;
                model.TipoUso = data.TipoUso;
                model.VolumenOtorgado = data.VolumenOtorgado;
                model.LimiteDiario = data.LimiteDiario;
                model.LimiteSemanal = data.LimiteSemanal;
                model.LimiteMensual = data.LimiteMensual;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarResolucionConsumoAgua(ResolucionConsumoAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarResolucion.Execute(model.IdResolucionConsumoAgua, model.Resolucion, model.Anio, model.TipoUso, model.ClaseDerecho, model.IdFuenteAgua, model.VolumenOtorgado, model.Actividad, model.LimiteDiario, model.LimiteSemanal, model.LimiteMensual, model.Estado);
                return RedirectToAction("DetalleResolucionConsumoAgua", new { IdResolucionConsumoAgua = model.IdResolucionConsumoAgua });
            }
        }
        public async Task<IActionResult> AgregarResolucionConsumoAgua()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getFuentesAgua.Execute();
                var fuentes = items.Fuentes.Select(g => new SelectListItem() { Value = g.IdOrigenAgua.ToString(), Text = g.Descripcion, Disabled = g.Estado.Equals("INA") }).ToList();
                fuentes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.FuentesAgua = fuentes;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarResolucionConsumoAgua(ResolucionConsumoAguaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    if (ModelState.IsValid)
                    {
                        var idResolucionConsumoAgua = await _agregarResolucion.Execute(model.Resolucion, model.Anio, model.TipoUso, model.ClaseDerecho, model.IdFuenteAgua, model.VolumenOtorgado, model.LimiteDiario, model.LimiteSemanal, model.LimiteMensual, model.Actividad);
                        return RedirectToAction("DetalleResolucionConsumoAgua", new { IdResolucionConsumoAgua = idResolucionConsumoAgua });
                    }
                    return RedirectToAction("AgregarResolucionConsumoAgua");
                }
            }
        }

        //Usuarios Notificar
        public async Task<IActionResult> FiltrarUsuario(string filtro, int pagina, int rpp)
        {
            var data = await _getUsuariosCA.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>Usuario de Red</th>";
            tabla += "<th>Email</th>";
            tabla += "<th>Rol</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.nUsuarios % rpp != 0 ? (data.nUsuarios / rpp) + 1 : data.nUsuarios / rpp;
            if (data.nUsuarios > 0)
            {
                foreach (var fila in data.Usuarios)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.UsuarioRed + "</td>";
                    filasTabla += "<td>" + fila.Email + "</td>";
                    filasTabla += "<td>" + fila.Rol + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorConsumoAgua/DetalleUsuario?IdUsuario=" + fila.IdUsuario + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";
            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleUsuario(int IdUsuario)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var enumList = Enum.GetValues(typeof(RSRoles)).OfType<RSRoles>().ToList();

                var roles = enumList.Select(g => new SelectListItem() { Value = ((int)g).ToString(), Text = g.ToString() }).ToList();
                roles.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Roles = roles;

                var Usuario = await _getUsuario.Execute(IdUsuario);
                var model = new UsusarioNotificarModel();
                model.Id_Usuario_Notificar = Usuario.IdUsuario;
                model.Id_Rol = Usuario.IdRol;
                model.Nombres = Usuario.Nombres;
                model.Apellidos = Usuario.Apellidos;
                model.usuario_red = Usuario.UsuarioRed;
                model.Email = Usuario.Email;
                model.Estado = Usuario.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarUsuario(UsusarioNotificarModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                if (ModelState.IsValid)
                    await _editarUsuario.Execute(model.Id_Usuario_Notificar, model.Nombres, model.Apellidos, model.Id_Rol, model.Email, model.usuario_red, model.Estado);
                return RedirectToAction("DetalleUsuario", new { IdUsuario = model.Id_Usuario_Notificar });
            }
        }
        public async Task<IActionResult> AgregarUsuario()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var UsuariosAD = await _getUsuarios.Execute();

                var usuarios = UsuariosAD.Usuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ListUsuarios = usuarios;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsusarioNotificarModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarUsuario.Execute(model.Id_Usuario_Notificar);
                return RedirectToAction("DetalleUsuario", new { IdUsuario = id });
            }
            return RedirectToAction("AgregarUsuario");
        }
        public async Task<IActionResult> QuitarUsuario(int Id_Usuario)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                await _deleteUsuario.Execute(Id_Usuario);
                return RedirectToAction("Resumen");
            }
        }

        //Mensajes Error
        [HttpPost]
        public async Task<IActionResult> FiltrarMensajeError(string filtro)
        {
            var textoMensajes = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), System.Text.Encoding.GetEncoding("iso-8859-1"));
            var mensajes = JsonConvert.DeserializeObject<Mensajes>(textoMensajes);

            var data = mensajes.MensajesError;

            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Tipo</th>";
            tabla += "<th>Mensaje</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            if (data.Count > 0)
            {
                foreach (var fila in data)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Tipo + "</td>";
                    filasTabla += "<td>" + fila.Mensaje + "</td>";
                    filasTabla += "<td><a href=\"/GestorConsumoAgua/DetalleMensajeError?Tipo=" + fila.Tipo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }
                paginador += "<ul class=\"pagination\">";
                paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarMensajeError(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleMensajeError(string Tipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getFuentesAgua.Execute();
                var fuentes = items.Fuentes.Select(g => new SelectListItem() { Value = g.IdFuenteAgua.ToString(), Text = g.Descripcion, Disabled = g.Estado.Equals("INA") }).ToList();
                fuentes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.FuentesAgua = fuentes;

                var textoMensajes = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), System.Text.Encoding.GetEncoding("iso-8859-1"));
                var mensajes = JsonConvert.DeserializeObject<Mensajes>(textoMensajes);
                var model = new MensajeErrorModel();
                model.Tipo = mensajes.MensajesError.First(x => x.Tipo.Equals(Tipo)).Tipo;
                model.Mensaje = mensajes.MensajesError.First(x => x.Tipo.Equals(Tipo)).Mensaje;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarMensajeError(MensajeErrorModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                if (ModelState.IsValid)
                {
                    var textoMensajes = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), System.Text.Encoding.GetEncoding("iso-8859-1"));
                    var mensajes = JsonConvert.DeserializeObject<Mensajes>(textoMensajes);

                    mensajes.MensajesError.First(x => x.Tipo.Equals(model.Tipo)).Mensaje = model.Mensaje;                  

                    System.IO.File.WriteAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), JsonConvert.SerializeObject(mensajes), System.Text.Encoding.GetEncoding("iso-8859-1"));                    
                }

                return RedirectToAction("DetalleMensajeError", new { Tipo = model.Tipo });
            }
        }

        private class Mensajes
        {
            public List<MensajeError> MensajesError { get; set; }
        }
        private class MensajeError
        {
            public string Tipo { get; set; }
            public string Mensaje { get; set; }
        }
    }
}
