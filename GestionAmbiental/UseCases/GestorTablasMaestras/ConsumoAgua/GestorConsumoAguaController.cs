using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua
{
    public class GestorConsumoAguaController : Controller
    {
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

        private readonly IRegistrarConsumoAguaMasivo _registrarConsumoAguaMasivo;
        private ExcelLecturaEscritura _excelLecturaEscritura;
        public GestorConsumoAguaController(
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
            ExcelLecturaEscritura excelLecturaEscritura)
        {
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
            _registrarConsumoAguaMasivo = registrarConsumoAguaMasivo;
            _excelLecturaEscritura = excelLecturaEscritura;
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
        public async Task<IActionResult> CargaDataExcel()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var resoluciones = await _getResoluciones.Execute();

                var resolucionesVB = resoluciones.Resoluciones.Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
                resolucionesVB.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Resoluciones = resolucionesVB;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CargaDataExcel(CargaExcelModel model)
        {
            if (ModelState.IsValid)
            {
                var stream = model.Excel.OpenReadStream();
                _excelLecturaEscritura = new ExcelLecturaEscritura(stream,model.Pagina,model.FilaInicio,model.ColumnaInicio);
                var data = _excelLecturaEscritura.ObtenerDatos();
                                
                var input = new List<Application.UseCases.ConsumoAgua.ConsumoAguaInput>();

                foreach (var consumo in data)
                {
                    if(consumo.TiempoBombeo!=0 && consumo.LecturaInicial != 0 && consumo.LecturaFinal != 0)
                    {
                        input.Add(new Application.UseCases.ConsumoAgua.ConsumoAguaInput
                        {
                            IdResolucionConsumoAgua = model.Resolucion,
                            Fecha = consumo.Fecha,
                            TiempoBombeo = consumo.TiempoBombeo,
                            LecturaInicial = consumo.LecturaInicial,
                            LecturaFinal = consumo.LecturaFinal,
                            VolumenBombeado = consumo.LecturaFinal -consumo.LecturaInicial,
                            CaudalPromedio = (consumo.LecturaFinal - consumo.LecturaInicial)/(consumo.TiempoBombeo*3600)
                        });
                    }
                }

                var agregados = await _registrarConsumoAguaMasivo.Execute(input, "kevin");
                
            }
            /*
             function CalcularVolumenAguaBombeado() {
            var volumen = lecturaFinal - lecturaInicial;
            $("#VolumenAguaBombeado").val(volumen);
        }
        function CalcularCaudalPromedio() {
            var volumen = lecturaFinal - lecturaInicial;
            var segundos = horas * 3600;
            if (segundos > 0)
                $("#CaudalPromedio").val((volumen / segundos).toFixed(2));
            else
                $("#CaudalPromedio").val(0);
        }
             */
            return RedirectToAction("CargaDataExcel");
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
                    await _editarResolucion.Execute(model.IdResolucionConsumoAgua,model.Resolucion,model.Anio,model.TipoUso,model.ClaseDerecho,model.IdFuenteAgua,model.VolumenOtorgado,model.Actividad, model.Estado);
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
                        var idResolucionConsumoAgua = await _agregarResolucion.Execute(model.Resolucion,model.Anio,model.TipoUso,model.ClaseDerecho,model.IdFuenteAgua,model.VolumenOtorgado,model.Actividad);
                        return RedirectToAction("DetalleResolucionConsumoAgua", new { IdResolucionConsumoAgua = idResolucionConsumoAgua });
                    }
                    return RedirectToAction("AgregarResolucionConsumoAgua");
                }
            }
        }

    }
}
