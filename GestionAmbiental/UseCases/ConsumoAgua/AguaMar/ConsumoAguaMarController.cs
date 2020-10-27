using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.EditarConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetRegistroRecienteConsumoAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using GestionAmbiental.Application.UseCases.ConsumoAgua.RegistrarConsumoAgua;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResolucion;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.ConsumoAgua.AguaMar
{
    public class ConsumoAguaMarController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGetConsumosAgua _getConsumosAgua;
        private readonly IGetConsumoAgua _getConsumoAgua;
        private readonly IRegistrarConsumoAgua _registrarConsumoAgua;
        private readonly IEditarConsumoAgua _editarConsumoAgua;
        private readonly IGetRegistrosRecientesConsumoAgua _getRegistrosRecientesConsumoAgua;
        private readonly IGetResolucion _getResolucion;
        private readonly ISelectItems _selectItems;
        private readonly IDataProtector _protector;

        public ConsumoAguaMarController(
            IHostingEnvironment hostingEnvironment,
            IGetConsumosAgua getConsumosAgua, 
            IGetConsumoAgua getConsumoAgua, 
            IRegistrarConsumoAgua registrarConsumoAgua,
            IEditarConsumoAgua editarConsumoAgua,
            IGetRegistrosRecientesConsumoAgua getRegistrosRecientesConsumoAgua,
            ISelectItems selectItems,
            IGetResolucion getResolucion,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetUsuario getUsuario)
        {
            _hostingEnvironment = hostingEnvironment;
            _getConsumosAgua = getConsumosAgua;
            _getConsumoAgua = getConsumoAgua;
            _registrarConsumoAgua = registrarConsumoAgua;
            _editarConsumoAgua = editarConsumoAgua;
            _getRegistrosRecientesConsumoAgua = getRegistrosRecientesConsumoAgua;
            _selectItems = selectItems;
            _getResolucion = getResolucion;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ConsumoAguaMarIdRouteValue);
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
                var items = await _selectItems.GetSelectItems();

                var resoluciones = items.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text ="("+g.Actividad+") "+ g.Resolucion }).ToList();
                resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Resoluciones = resoluciones;
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int resolucion, int rpp, int pagina)
        {
            var items = await _selectItems.GetSelectItems();

            var datosTabla = await _getConsumosAgua.Execute(fecha,resolucion, pagina, rpp,items.ItemsOrigenes.FirstOrDefault(x=>x.Descripcion.Equals("Agua de Mar")).IdOrigenAgua);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NConsumo % rpp != 0 ? (datosTabla.NConsumo / rpp) + 1 : datosTabla.NConsumo / rpp;
            if (datosTabla.NConsumo > 0)
            {
                foreach (var fila in datosTabla.Consumos)
                {
                    var IdEncriptado = _protector.Protect(fila.IdConsumoAgua.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Actividad + "</td>";
                    filasTabla += "<td>" + fila.ResolucionConsumoAgua + "</td>";
                    filasTabla += "<td>" + fila.FuenteAgua + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.TiempoBombeo + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.LecturaInicial) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.LecturaFinal) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.VolumenBombeado) + "</td>";
                    filasTabla += "<td>" + String.Format("{0:0.00}", fila.CaudalPromedio) + "</td>";
                    filasTabla += "<td><a href=\"/ConsumoAguaMar/EditarRegistro?IdConsumo=" + IdEncriptado + "\">Ver Detalles</a></td>";
                    filasTabla += "</tr>";                   
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";
            }
            else
            {
                filasTabla = "Sin datos";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }

        public async Task<IActionResult> NuevoRegistro(string message)
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

                var idsResoluciones = items.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(x=>x.IdResolucionConsumoAgua).ToArray();
                var ultimosRegistros = await _getRegistrosRecientesConsumoAgua.Execute(idsResoluciones);

                var actividades = items.ItemsResoluciones.Where(x => x.Estado.Equals("ACT") && x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Actividad }).ToList();
                actividades.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var resolucionesAct = items.ItemsResoluciones.Where(x => x.Estado.Equals("ACT") && x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(x => new { x.IdResolucionConsumoAgua, x.Resolucion }).ToArray();

                var textoMensajes = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), System.Text.Encoding.GetEncoding("iso-8859-1"));
                
                ViewBag.MensajesError = textoMensajes;
                ViewBag.Actividades = actividades;
                ViewBag.ResolucionesAct = JsonConvert.SerializeObject(resolucionesAct);
                ViewBag.RegistrosResolucion = JsonConvert.SerializeObject(ultimosRegistros.Select(x => new { IdResolucion = x.IdResolucionConsumoAgua, Fecha = x.Fecha.ToString("yyyy-MM-dd"), FechaNueva = x.Fecha.AddDays(1).ToString("yyyy-MM-dd"), LecturaInicial = x.LecturaFinal, TiempoBombeo = x.TiempoBombeo }));
                ViewBag.Message = message;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(ConsumoAguaMarModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var volumenAguaBombeado = CalcularVolumenAguaBombeado(model.LecturaInicial, model.LecturaFinal);
                var caudalPromedio = CalcularCaudalPromedio(model.LecturaInicial, model.LecturaFinal, model.TiempoBombeo);
                var resolucion = await _getResolucion.Execute(model.IdResolucion);
                var items = await _selectItems.GetSelectItems();

                if (volumenAguaBombeado != -1 && caudalPromedio != -1)
                {
                    var id = await _registrarConsumoAgua.Execute(
                        model.IdResolucion,
                        model.Fecha,
                        model.TiempoBombeo,
                        model.LecturaInicial,
                        model.LecturaFinal,
                        volumenAguaBombeado,
                        caudalPromedio,
                        userInfo.NombreUsuario);

                    var registradosMensual = await _getConsumosAgua.Execute(model.Fecha, items.ItemsOrigenes.FirstOrDefault(x => x.Descripcion.Equals("Agua de Mar")).IdOrigenAgua);
                    var consumoActual = registradosMensual.Consumos.Sum(x => x.VolumenBombeado);
                    if (consumoActual > (resolucion.LimiteMensual*0.9))
                    {                        
                        return RedirectToAction("NotificarLimiteConsumoAguaMarMensual", "NotificacionesConsumoAgua", new { IdConsumo = id, limiteMensual = resolucion.LimiteMensual, volumenActual = consumoActual });                        
                    }
                    else
                        return RedirectToAction("Resumen");
                }
                else
                {
                    return RedirectToAction("NuevoRegistro", new { message = "Los valores ingresados son incorrectos" });
                }
            }
            return RedirectToAction("Resumen");
        }
        public async Task<IActionResult> EditarRegistro(string IdConsumo)
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

                var desencriptado = Int32.Parse(_protector.Unprotect(IdConsumo));

                var items = await _selectItems.GetSelectItems();

                var actividades = items.ItemsResoluciones.Where(x => x.Estado.Equals("ACT") && x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Actividad }).ToList();
                actividades.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var resolucionesAct = items.ItemsResoluciones.Where(x => x.Estado.Equals("ACT") && x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(x => new { x.IdResolucionConsumoAgua, x.Resolucion }).ToArray();

                var idsResoluciones = items.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(x => x.IdResolucionConsumoAgua).ToArray();
                var ultimosRegistros = await _getRegistrosRecientesConsumoAgua.Execute(idsResoluciones);

                var textoMensajes = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "ConsumoAgua.json"), System.Text.Encoding.GetEncoding("iso-8859-1"));

                ViewBag.MensajesError = textoMensajes;
                ViewBag.Actividades = actividades;
                ViewBag.ResolucionesAct = JsonConvert.SerializeObject(resolucionesAct);
                ViewBag.RegistrosResolucion = JsonConvert.SerializeObject(ultimosRegistros.Select(x => new { IdResolucion = x.IdResolucionConsumoAgua, Fecha = x.Fecha.ToString("yyyy-MM-dd"), FechaNueva = x.Fecha.AddDays(1).ToString("yyyy-MM-dd"), LecturaInicial = x.LecturaFinal, TiempoBombeo = x.TiempoBombeo }));

                var consumo = await _getConsumoAgua.Execute(desencriptado);
                var model = new ConsumoAguaMarModel
                {
                    Id = consumo.IdConsumoAgua,
                    IdResolucion = consumo.IdResolucionConsumoAgua,
                    Resolucion = consumo.ResolucionConsumoAgua,
                    FuenteAgua = consumo.FuenteAgua,
                    Fecha = consumo.Fecha.ToString("yyyy-MM-dd"),
                    TiempoBombeo = consumo.TiempoBombeo,
                    LecturaInicial = consumo.LecturaInicial,
                    LecturaFinal = consumo.LecturaFinal,
                    VolumenAguaBombeado = consumo.VolumenBombeado,
                    CaudalPromedio = consumo.CaudalPromedio
                };

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarRegistro(ConsumoAguaMarModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var volumenAguaBombeado = CalcularVolumenAguaBombeado(model.LecturaInicial, model.LecturaFinal);
                var caudalPromedio = CalcularCaudalPromedio(model.LecturaInicial, model.LecturaFinal, model.TiempoBombeo);
                if (volumenAguaBombeado != -1 && caudalPromedio != -1)
                {
                    await _editarConsumoAgua.Execute(
                        model.Id,
                        model.IdResolucion,
                        model.Fecha,
                        model.TiempoBombeo,
                        model.LecturaInicial,
                        model.LecturaFinal,
                        model.VolumenAguaBombeado,
                        model.CaudalPromedio);
                }
            }
            var encriptado = _protector.Protect(model.Id.ToString());
            return RedirectToAction("EditarRegistro", new { IdConsumo = encriptado });
        }
        private double CalcularVolumenAguaBombeado(double lecturaInicial, double lecturaFinal)
        {
            if (lecturaFinal - lecturaInicial >= 0)
                return lecturaFinal - lecturaInicial;
            else
                return -1;
        }
        private double CalcularCaudalPromedio(double lecturaInicial, double lecturaFinal, double horas)
        {
            if (((lecturaFinal - lecturaInicial) / (horas * 3600)) >= 0)
                return (lecturaFinal - lecturaInicial) / (horas * 3600);
            else
                return -1;
        }
        
    }
}
