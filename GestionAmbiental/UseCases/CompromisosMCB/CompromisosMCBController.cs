using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.NuevoRegistro;
//using GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros;
using GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.EditarRegistro;
using GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.Items;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.IO;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Renci.SshNet;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using GestionAmbiental.UseCases.Util.Interfaces;

namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class CompromisosMCBController : Controller
    {
        private ExcelLecturaEscritura _excelLecturaEscritura;
        private readonly INuevoRegistro _nuevoRegistro;
        private readonly ISelectItems _selectItems;
        private readonly IEditarRegistro _editarRegistro;
        private readonly Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros.IListarRegistros _listarCompromisos;
        private readonly Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros.IListarRegistros _listarSustentos;
        private readonly Application.UseCases.CompromisosMCB.SustentoCMCB.EditarRegistro.IEditarRegistro _editarSustento;
        private readonly Application.UseCases.CompromisosMCB.GerenciaUsuarioCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroGerenciaUsuario;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly Application.UseCases.CompromisosMCB.CriterioCMBC.NuevoRegistro.INuevoRegistro _nuevoRegistroCriterio;
        private readonly Application.UseCases.CompromisosMCB.EtapaCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroEtapa;
        private readonly Application.UseCases.CompromisosMCB.OrigenCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroOrigen;
        private readonly Application.UseCases.CompromisosMCB.AutoridadCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroAutoridad;
        private readonly Application.UseCases.CompromisosMCB.AspectoCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroAspecto;
        private readonly Application.UseCases.CompromisosMCB.ComponentesCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroComponente;
        private readonly Application.UseCases.CompromisosMCB.GerenciaCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroGerencia;
        private readonly Application.UseCases.CompromisosMCB.AreaCMCB.NuevoRegistro.INuevoRegistro _nuevoRegistroArea;
        public CompromisosMCBController(
            IGestorArchivos gestorArchivos,
            INuevoRegistro nuevoRegistro,
            ISelectItems selectItems,
            IEditarRegistro editarRegistro,
            Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros.IListarRegistros listarCompromisos,
            Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros.IListarRegistros listarSustentos,
            Application.UseCases.CompromisosMCB.SustentoCMCB.EditarRegistro.IEditarRegistro editarSustento,
            Application.UseCases.CompromisosMCB.GerenciaUsuarioCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroGerenciaUsuario,
            IGetInfo_SGA info_SGA,
            IWebHostEnvironment hostingEnvironment,

            Application.UseCases.CompromisosMCB.CriterioCMBC.NuevoRegistro.INuevoRegistro nuevoRegistroCriterio,
            Application.UseCases.CompromisosMCB.EtapaCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroEtapa,
            Application.UseCases.CompromisosMCB.OrigenCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroOrigen,
            Application.UseCases.CompromisosMCB.AutoridadCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroAutoridad,
            Application.UseCases.CompromisosMCB.AspectoCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroAspecto,
            Application.UseCases.CompromisosMCB.ComponentesCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroComponente,
            Application.UseCases.CompromisosMCB.GerenciaCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroGerencia,
            Application.UseCases.CompromisosMCB.AreaCMCB.NuevoRegistro.INuevoRegistro nuevoRegistroArea
            )
        {
            _gestorArchivos = gestorArchivos;
            _nuevoRegistro = nuevoRegistro;
            _selectItems = selectItems;
            _editarRegistro = editarRegistro;
            _listarCompromisos = listarCompromisos;
            _listarSustentos = listarSustentos;
            _editarSustento = editarSustento;
            _nuevoRegistroGerenciaUsuario = nuevoRegistroGerenciaUsuario;
            _info_SGA = info_SGA;
            _hostingEnvironment = hostingEnvironment;

            _nuevoRegistroCriterio = nuevoRegistroCriterio;
            _nuevoRegistroEtapa = nuevoRegistroEtapa;
            _nuevoRegistroOrigen = nuevoRegistroOrigen;
            _nuevoRegistroAutoridad = nuevoRegistroAutoridad;
            _nuevoRegistroAspecto = nuevoRegistroAspecto;
            _nuevoRegistroComponente = nuevoRegistroComponente;
            _nuevoRegistroGerencia = nuevoRegistroGerencia;
            _nuevoRegistroArea = nuevoRegistroArea;
        }
        public IActionResult PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Compromisos"));
                if (dataModulo != null)
                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(dataModulo)
                    };
                else
                    response = new
                    {
                        success = false,
                        msg = "No se encontraron las propiedades del módulo"
                    };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    msg = e.Message
                };
            }
            return Ok(response);
        }
        public async Task<IActionResult> ResumenCompromisos()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();

                var areasItems = items.ItemsArea.OrderBy(x => x.Area).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Area }).ToList();

                var criteriosItems = items.ItemsCriterio.OrderBy(x => x.Criterio).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Criterio }).ToList();

                var etapasItems = items.ItemsEtapa.OrderBy(x => x.Etapa).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Etapa }).ToList();

                var origenesItems = items.ItemsOrigen.OrderBy(x => x.Origen).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Origen }).ToList();

                var autoridadesItems = items.ItemsAutoridad.OrderBy(x => x.Autoridad).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Autoridad }).ToList();

                var aspectosItems = items.ItemsAspecto.OrderBy(x => x.Aspecto).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Aspecto }).ToList();

                var componentesItems = items.ItemsComponente.OrderBy(x => x.Componente).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();

                var GerenciasItems = items.ItemsGerencia.OrderBy(x => x.Gerencia).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Gerencia }).ToList();

                var UsuariosItems = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres }).ToList();

                var EstadosItems = items.EstadosCompromiso.OrderBy(x => x).Select(g => new SelectListItem() { Value = g, Text = g }).ToList();

                ViewBag.Areas = areasItems;
                ViewBag.Criterios = criteriosItems;
                ViewBag.Etapas = etapasItems;
                ViewBag.Origenes = origenesItems;
                ViewBag.Autoridades = autoridadesItems;
                ViewBag.Aspectos = aspectosItems;
                ViewBag.Componentes = componentesItems;
                ViewBag.Gerencias = GerenciasItems;
                ViewBag.Usuarios = UsuariosItems;
                ViewBag.Estados = EstadosItems;

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        public async Task<IActionResult> ResumenSustentos()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();

                var areasItems = items.ItemsArea.OrderBy(x => x.Area).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Area }).ToList();

                var criteriosItems = items.ItemsCriterio.OrderBy(x => x.Criterio).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Criterio }).ToList();

                var etapasItems = items.ItemsEtapa.OrderBy(x => x.Etapa).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Etapa }).ToList();

                var origenesItems = items.ItemsOrigen.OrderBy(x => x.Origen).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Origen }).ToList();

                var autoridadesItems = items.ItemsAutoridad.OrderBy(x => x.Autoridad).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Autoridad }).ToList();

                var aspectosItems = items.ItemsAspecto.OrderBy(x => x.Aspecto).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Aspecto }).ToList();

                var componentesItems = items.ItemsComponente.OrderBy(x => x.Componente).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();

                var GerenciasItems = items.ItemsGerencia.OrderBy(x => x.Gerencia).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Gerencia }).ToList();

                var UsuariosItems = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres }).ToList();

                var EstadosItems = items.EstadosSustento.OrderBy(x => x).Select(g => new SelectListItem() { Value = g, Text = g }).ToList();


                ViewBag.Areas = areasItems;
                ViewBag.Criterios = criteriosItems;
                ViewBag.Etapas = etapasItems;
                ViewBag.Origenes = origenesItems;
                ViewBag.Autoridades = autoridadesItems;
                ViewBag.Aspectos = aspectosItems;
                ViewBag.Componentes = componentesItems;
                ViewBag.Gerencias = GerenciasItems;
                ViewBag.Usuarios = UsuariosItems;
                ViewBag.Estados = EstadosItems;

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;



                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListarSustentos(int pagina, int rpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var datosTabla = await _listarSustentos.Execute(pagina, rpp, gerencias, areas, criterios, estados, etapas, userInfo.NombreUsuario);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                //Contenido de la tabla
                foreach (var fila in datosTabla.Registros)
                {
                    var button = "";
                    if (fila.Estado == "Registrado")
                    {
                        button = "<button type =\"button\" class=\"btn btn-mc\" onclick=\"Implementar(" + fila.Id + ")\">Implementar</button>";
                    }
                    if (fila.Estado == "Implementado")
                    {
                        button = "<button type =\"button\" class=\"btn btn-mc\" onclick=\"Validar(" + fila.Id + ")\">Validar</button>";
                    }

                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.IdCompromiso + "</td>";
                    filasTabla += "<td><a href=\"#\"></a>" +
                        "<button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.IdCompromiso + "," + fila.Id + ")\">Detalle</button>" +
                        button
                         +
                        " </td>";
                    filasTabla += "<td>" + fila.Criterio + "</td>";
                    filasTabla += "<td>" + fila.Detalle_compromiso + "</td>";
                    filasTabla += "<td>" + fila.Etapa + "</td>";
                    filasTabla += "<td>" + fila.Componente + "</td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Area + "</td>";
                    filasTabla += "<td>" + fila.Periodicidad + "</td>";
                    filasTabla += "<td>" + fila.Frecuencia_Reporte + "</td>";
                    filasTabla += "<td>" + fila.Entidad_Responsable + "</td>";
                    filasTabla += "<td>" + fila.Medio_verificacion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td>" + fila.Responsable1 + "</td>";
                    filasTabla += "<td>" + fila.Responsable2 + "</td>";
                    filasTabla += "<td>" + fila.Asesor + "</td>";
                    filasTabla += "<td>" + fila.Detalle_compromiso + "</td>";
                    filasTabla += "<td>" + fila.Sustento_cumplimiento + "</td>";
                    filasTabla += "</tr>";
                }
                //Paginador
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> NuevoRegistro()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();

                var areasItems = items.ItemsArea.OrderBy(x => x.Area).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Area }).ToList();
                areasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var criteriosItems = items.ItemsCriterio.OrderBy(x => x.Criterio).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Criterio }).ToList();
                criteriosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var etapasItems = items.ItemsEtapa.OrderBy(x => x.Etapa).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Etapa }).ToList();
                etapasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var origenesItems = items.ItemsOrigen.OrderBy(x => x.Origen).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Origen }).ToList();
                origenesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var autoridadesItems = items.ItemsAutoridad.OrderBy(x => x.Autoridad).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Autoridad }).ToList();
                autoridadesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var aspectosItems = items.ItemsAspecto.OrderBy(x => x.Id).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Aspecto }).ToList();
                //aspectosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var componentesItems = items.ItemsComponente.OrderBy(x => x.Componente).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();
                componentesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var GerenciasItems = items.ItemsGerencia.OrderBy(x => x.Gerencia).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Gerencia }).ToList();
                GerenciasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var UsuariosItems = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres }).ToList();
                UsuariosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var TiposCompromisosItems = items.TipoCompromisos.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Tipo }).ToList();

                var ComponentesAux = items.ComponentesAux.OrderBy(x => x.Componente).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();
                ComponentesAux.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var FactoresAmbientales = items.FactoresAmbientales.OrderBy(x => x.Factor).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Factor }).ToList();
                FactoresAmbientales.Insert(0, new SelectListItem("--SELECCIONE--", "", true));


                //var EstadosItems = items.EstadosSustento.OrderBy(x => x).Select(g => new SelectListItem() { Value = g, Text = g }).ToList();
                //EstadosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                ViewBag.Areas = areasItems;
                ViewBag.Criterios = criteriosItems;
                ViewBag.Etapas = etapasItems;
                ViewBag.Origenes = origenesItems;
                ViewBag.Autoridades = autoridadesItems;
                ViewBag.Aspectos = aspectosItems;
                ViewBag.Componentes = componentesItems;
                ViewBag.Gerencias = GerenciasItems;
                ViewBag.Usuarios = UsuariosItems;
                //ViewBag.Estados = EstadosItems;

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                ViewBag.TipoCompromisos = TiposCompromisosItems;
                ViewBag.ComponentesAux = ComponentesAux;
                ViewBag.FactoresAmbientales = FactoresAmbientales;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(CompromisoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _nuevoRegistro.Execute(model.Codigo, model.Id_Criterio, model.Detalle_compromiso, model.Id_Etapa, model.Texto_compromiso, model.Id_Origen, model.Tomo,
                    model.Capitulo, model.Seccion, model.Pagina, model.Resolucion, model.Id_Autoridad, model.Fecha, model.Id_Compromiso, model.Id_Aspecto, model.Id_Componente,
                    model.Id_Gerencia, model.Id_Area, model.Frecuencia, model.Id_Entidad, model.Id_Responsable1, model.Id_Responsable2, model.Frecuencia_reporte, model.Sustentos,
                    model.Estado, model.Id_Aprueba, model.Medio_verificacion, model.Ano, model.Duplicado, model.Id_TipoCompromiso, model.Actividad, model.Id_ComponenteAux,
                    model.Id_FactorAmbiental, model.Vigencia, model.Motivo, model.Comentario);


                if (model.Id_Responsable1 != null && model.Id_Gerencia != null)
                {
                    var idGerenciaUsuario = await _nuevoRegistroGerenciaUsuario.Execute((int)model.Id_Responsable1, (int)model.Id_Gerencia, "1");
                }
                if (model.Id_Responsable2 != null && model.Id_Gerencia != null)
                {
                    var idGerenciaUsuario = await _nuevoRegistroGerenciaUsuario.Execute((int)model.Id_Responsable2, (int)model.Id_Gerencia, "1");
                }

                return RedirectToAction("ResumenCompromisos");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ListarCompromisos(int pagina, int rpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            var datosTabla = await _listarCompromisos.Execute(pagina, rpp, gerencias, areas, criterios, estados, etapas);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                //Contenido de la tabla
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><span>" + fila.Id + "</span></br>" +
                        "<a href=\"/CompromisosMCB/EditarRegistro?id=" + fila.Id + "\">" +
                        "<button type =\"button\" class=\"btn btn-mc\">Editar</button>" +
                        "</a>" +
                        "<button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button>" +
                        "</td>";
                    filasTabla += "<td>" + fila.Codigo + "</td>";
                    filasTabla += "<td>" + fila.NomCriterio + "</td>";
                    filasTabla += "<td>" + fila.Detalle_compromiso + "</td>";
                    filasTabla += "<td>" + fila.NomEtapa + "</td>";
                    filasTabla += "<td>" + fila.NomAspecto + "</td>";
                    filasTabla += "<td>" + fila.NomComponente + "</td>";
                    filasTabla += "<td>" + fila.NomGerencia + "</td>";
                    filasTabla += "<td>" + fila.NomArea + "</td>";
                    filasTabla += "<td>" + fila.Frecuencia + "</td>";
                    //filasTabla += "<td>" + fila.NomResponsable1 + "</td>";
                    filasTabla += "<td>" + fila.Frecuencia_reporte + "</td>";
                    filasTabla += "<td>" + "<a style=\" cursor:pointer\" onclick=\"GetSustentosByCompromiso(" + fila.Id + ")\">Sustentos</a>" + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    //filasTabla += "<td>" + fila.Texto_compromiso + "</td>";
                    filasTabla += "</tr>";
                }
                //Paginador
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCompromiso(int idCompromiso, int? idSustento)
        {

            var datos = await _listarCompromisos.BuscarPorID(idCompromiso);
            dynamic response = new { Data = datos };
            if (idSustento.HasValue)
            {
                var sustento = await _listarSustentos.ObtenerSustento(Convert.ToInt32(idSustento));
                response.Sustento = sustento;
            }

            return Ok(response);

        }
        public async Task<IActionResult> EditarRegistro(int id)
        {

            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var items = await _selectItems.Execute();

            var areasItems = items.ItemsArea.OrderBy(x => x.Area).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Area }).ToList();
            areasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var criteriosItems = items.ItemsCriterio.OrderBy(x => x.Criterio).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Criterio }).ToList();
            criteriosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var etapasItems = items.ItemsEtapa.OrderBy(x => x.Etapa).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Etapa }).ToList();
            etapasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var origenesItems = items.ItemsOrigen.OrderBy(x => x.Origen).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Origen }).ToList();
            origenesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var autoridadesItems = items.ItemsAutoridad.OrderBy(x => x.Autoridad).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Autoridad }).ToList();
            autoridadesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var aspectosItems = items.ItemsAspecto.OrderBy(x => x.Aspecto).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Aspecto }).ToList();
            aspectosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var componentesItems = items.ItemsComponente.OrderBy(x => x.Componente).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();
            componentesItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var GerenciasItems = items.ItemsGerencia.OrderBy(x => x.Gerencia).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Gerencia }).ToList();
            GerenciasItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var UsuariosItems = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres }).ToList();
            UsuariosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var TiposCompromisosItems = items.TipoCompromisos.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Tipo }).ToList();

            var ComponentesAux = items.ComponentesAux.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Componente }).ToList();

            var FactoresAmbientales = items.FactoresAmbientales.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Factor }).ToList();

            var registros = await _listarCompromisos.Execute(1, 100, new int[0], new int[0], new int[0], new string[0], new int[0]);

            var registroEncontrado = registros.Registros.Find(x => x.Id.Equals(id));

            var model = new CompromisoModel();

            model.Id = registroEncontrado.Id;
            model.Codigo = registroEncontrado.Codigo;
            model.Id_Criterio = registroEncontrado.IdCriterio;
            model.Detalle_compromiso = registroEncontrado.Detalle_compromiso;
            model.Id_Etapa = registroEncontrado.IdEtapa;
            model.Texto_compromiso = registroEncontrado.Texto_compromiso;
            model.Id_Origen = registroEncontrado.IdOrigen;
            model.Tomo = registroEncontrado.Tomo;
            model.Capitulo = registroEncontrado.Capitulo;
            model.Seccion = registroEncontrado.Seccion;
            model.Pagina = registroEncontrado.Pagina;
            model.Resolucion = registroEncontrado.Resolucion;
            model.Id_Autoridad = registroEncontrado.IdAutoridad;
            model.Fecha = Convert.ToDateTime(registroEncontrado.Fecha);
            model.Id_Aspecto = registroEncontrado.IdAspecto;
            model.Id_Componente = registroEncontrado.IdComponente;
            model.Id_Gerencia = registroEncontrado.IdGerencia;
            model.Id_Area = registroEncontrado.IdArea;
            model.Frecuencia = registroEncontrado.Frecuencia;
            model.Id_Entidad = registroEncontrado.IdEntidad;
            model.Id_Responsable1 = registroEncontrado.IdResponsable1;
            model.Id_Responsable2 = registroEncontrado.IdResponsable2;
            model.Frecuencia_reporte = registroEncontrado.Frecuencia_reporte;
            model.Sustentos = registroEncontrado.Sustentos;
            model.Estado = registroEncontrado.Estado;
            model.Id_Aprueba = registroEncontrado.Aprueba;
            model.Medio_verificacion = registroEncontrado.Medio_verificacion;

            model.Ano = registroEncontrado.Ano;
            model.Duplicado = registroEncontrado.Duplicado;
            model.Id_TipoCompromiso = registroEncontrado.Id_TipoCompromiso;
            model.Actividad = registroEncontrado.Actividad;
            model.Id_ComponenteAux = registroEncontrado.Id_ComponenteAux;
            model.Id_FactorAmbiental = registroEncontrado.Id_FactorAmbiental;
            model.Vigencia= registroEncontrado.Vigencia;
            model.Motivo= registroEncontrado.Motivo;
            model.Comentario= registroEncontrado.Comentario;

            //Evidencia = registroEncontrado.Evidencia

            ViewBag.Areas = areasItems;
            ViewBag.Criterios = criteriosItems;
            ViewBag.Etapas = etapasItems;
            ViewBag.Origenes = origenesItems;
            ViewBag.Autoridades = autoridadesItems;
            ViewBag.Aspectos = aspectosItems;
            ViewBag.Componentes = componentesItems;
            ViewBag.Gerencias = GerenciasItems;
            ViewBag.Usuarios = UsuariosItems;

            ViewBag.Rol = userInfo.Rol.ToString();
            ViewBag.Usuario = userInfo.NombreUsuario;

            ViewBag.TipoCompromisos = TiposCompromisosItems;
            ViewBag.ComponentesAux = ComponentesAux;
            ViewBag.FactoresAmbientales = FactoresAmbientales;

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditarRegistroAsync(CompromisoModel model)
        {
            if (ModelState.IsValid)
            {
                await _editarRegistro.Execute(model.Id,
                    model.Codigo,
                    model.Id_Criterio,
                    model.Detalle_compromiso,
                    model.Id_Etapa,
                    model.Texto_compromiso,
                    model.Id_Origen,
                    model.Tomo,
                    model.Capitulo,
                    model.Seccion,
                    model.Pagina,
                    model.Resolucion,
                    model.Id_Autoridad,
                    model.Fecha,
                    model.Id_Compromiso,
                    model.Id_Aspecto,
                    model.Id_Componente,
                    model.Id_Gerencia,
                    model.Id_Area,
                    model.Frecuencia,
                    model.Id_Entidad,
                    model.Id_Responsable1,
                    model.Id_Responsable2,
                    model.Frecuencia_reporte,
                    model.Sustentos,
                    model.Estado,
                    model.Id_Aprueba,
                    model.Medio_verificacion,
                    model.Ano,
                    model.Duplicado,
                    model.Id_TipoCompromiso,
                    model.Actividad,
                    model.Id_ComponenteAux,
                    model.Id_FactorAmbiental,
                    model.Vigencia,
                    model.Motivo,
                    model.Comentario
                    );

                return RedirectToAction("EditarRegistro", new { id = model.Id });
            }
            return RedirectToAction("EditarRegistro", new { id = model.Id });
        }

        public async Task<IActionResult> GetSustento(int id)
        {
            var datos = await _listarSustentos.ObtenerSustento(id);
            //var previewFile = await PreviewFile(datos.Evidencia);
            var datosCompromiso = await _listarCompromisos.BuscarPorID(datos.IdCompromiso);
            dynamic response = new { Data = datos, DataCompromiso = datosCompromiso };
            return Json(response);
        }
        public async Task<IActionResult> GetSustentosByCompromiso(int IdCompromiso)
        {
            var datos = await _listarSustentos.GetSustentosByCompromiso(IdCompromiso);
            ////var previewFile = await PreviewFile(datos.Evidencia);
            var datosCompromiso = await _listarCompromisos.BuscarPorID(IdCompromiso);
            dynamic response = new { Data = datos, Data_Compromiso = datosCompromiso };
            return Json(response);
        }
        [HttpPost]
        public async Task<IActionResult> saveImplementar(ImplementarSustentoModel model)
        {
            if (ModelState.IsValid)
            {
                var nombreEvidencia = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_EvidenciaImplementacionCMCB" + Path.GetExtension(model.Evidencia.FileName);
                var newFilePath = await CargarDocumento(model.Evidencia, nombreEvidencia);
                await _editarSustento.ExecuteImplementar(model.Id,
                    model.Fecha_entrega.ToLongDateString(),
                    model.Sustento_cumpliento,
                    newFilePath,
                    model.Comentario_implementacion
                    );
                return RedirectToAction("ResumenSustentos");
            }
            return RedirectToAction("ResumenSustentos");

        }
        [HttpPost]
        public async Task<IActionResult> saveValidar(int id, DateTime fechaValidar, string ComentarioVerificacion)
        {
            var fecha = fechaValidar.Year + "-" + Convert.ToString(fechaValidar.Month).PadLeft(2, '0') + "-" + fechaValidar.Day;

            await _editarSustento.ExecuteVerificar(id, 1, fecha, ComentarioVerificacion, "medio");
            return RedirectToAction("ResumenSustentos");
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".csv", "text/csv"}
            };
        }
        private async Task<string> CargarDocumento(IFormFile file, string newFileName)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre-v2/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + newFileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    file.CopyTo(stream);

                    stream.Flush();

                    stream.Close();
                }
                var newFilePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/" + newFileName;
                //await EncryptFileAsync(fileName, encryptedArchiveFile);

                return await _gestorArchivos.UploadFTPFileAsync(newFilePath, newFileName,Tipo_Info_SGA.FTPConnectionCMCBImp);
            }
            else
                return "Error";
        }
        public async Task<IActionResult> PreviewFile(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                dynamic response;
                try
                {
                    var nombreDoc = fileName;

                    var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionCMCBImp);

                    var docsDownloadedPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Temp\DownloadedDocs");
                    if (Directory.Exists(docsDownloadedPath))
                    {
                        Directory.Delete(docsDownloadedPath, true);
                        Directory.CreateDirectory(Path.Combine(docsDownloadedPath, userInfo.NombreUsuario));
                    }
                    var fileStreamDownloaded = new FileStream(downloadedFile, FileMode.Open);
                    var docPath = Path.Combine(docsDownloadedPath, userInfo.NombreUsuario, nombreDoc);

                    using (var fileStream = new FileStream(docPath, FileMode.Create))
                    {
                        await fileStreamDownloaded.CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                    fileStreamDownloaded.Close();
                    response = new
                    {
                        result = "Ok",
                        path = "/Temp/DownloadedDocs/" + userInfo.NombreUsuario + "/" + nombreDoc,
                        extension = Path.GetExtension(nombreDoc)
                    };
                    return Ok(response);
                }
                catch (Exception e)
                {
                    response = new
                    {
                        result = "Error",
                        message = "No se encontró el archivo: " + e.Message
                    };
                    return Ok(response);
                }
            }
        }
        public IActionResult DownloadPreviewFile(string path)
        {
            var completePath = _hostingEnvironment.WebRootPath + "/" + path;
            FileStream stream = new FileStream(completePath, FileMode.Open);
            return File(stream, GetContentType(path), Path.GetFileName(path));
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

                var agregados = 0;
                foreach (var item in data)
                {
                    var id = await _nuevoRegistro.Execute(item.Codigo, (int)item.Id_Criterio, item.Detalle_compromiso, item.Id_Etapa, item.Texto_compromiso, item.Id_Origen, item.Tomo,
                   item.Capitulo, item.Seccion, item.Pagina, item.Resolucion, item.Id_Autoridad, item.Fecha, item.Id_Compromiso, item.Id_Aspecto, item.Id_Componente, item.Id_Gerencia, item.Id_Area,
                   item.Frecuencia, item.Id_Entidad, item.Id_Responsable1, item.Id_Responsable2, item.Frecuencia_reporte, item.Sustentos, item.Estado, item.Id_Aprueba,
                   item.Medio_verificacion,
                   item.Ano, item.Duplicado, item.Id_TipoCompromiso, item.Actividad, item.Id_ComponenteAux, item.Id_FactorAmbiental, item.Vigencia, item.Motivo, item.Comentario);
                    agregados++;
                }
                /*
                var dataFechas = await _getConsumosAguaFechas.ExecuteFechas(data.First().Fecha.ToString(), data.Last().Fecha.ToString(), model.IdResolucion);

                var validacion = await ValidarDatosExcel(data, dataFechas, model.IdResolucion, model.FilaInicio);
                if (validacion.Length == 0)
                {
                    var input = new List<Application.UseCases.ConsumoAgua.ConsumoAguaInput>();

                    foreach (var consumo in data)
                    {
                        if ((consumo.TiempoBombeo != 0) || (consumo.TiempoBombeo == 0 && consumo.LecturaInicial == consumo.LecturaFinal))
                            input.Add(new Application.UseCases.ConsumoAgua.ConsumoAguaInput
                            {
                                IdResolucionConsumoAgua = model.IdResolucion,
                                Fecha = consumo.Fecha,
                                TiempoBombeo = consumo.TiempoBombeo,
                                LecturaInicial = consumo.LecturaInicial,
                                LecturaFinal = consumo.LecturaFinal,
                                VolumenBombeado = consumo.LecturaFinal - consumo.LecturaInicial,
                                CaudalPromedio = consumo.TiempoBombeo != 0 ? (consumo.LecturaFinal - consumo.LecturaInicial) / (consumo.TiempoBombeo * 3600) : 0
                            });
                    }
                    var agregados = await _registrarConsumoAguaMasivo.Execute(input, userInfo.NombreUsuario);
                
                    return RedirectToAction("CargaDataExcel", new { message = "Se agregaron " + agregados + " registros", result = "Success" });
                }
                else
                {
                    return RedirectToAction("CargaDataExcel", new { message = "Ocurrió un error: \n" + validacion, result = "Error" });
                }*/
            }
            return RedirectToAction("CargaDataExcel", new { message = "" });
        }

        public async Task<IActionResult> saveCriterio(string nameCriterio, string crieterioDetalle)
        {
            var id = 0;
            if (nameCriterio.Length != null)
            {
                id = await _nuevoRegistroCriterio.Execute(nameCriterio, crieterioDetalle);

            }
            dynamic response = new { id = id, value = nameCriterio };
            return Json(response);
        }
        public async Task<IActionResult> saveEtapa(string nameEtapa)
        {
            var id = 0;
            if (nameEtapa.Length != null)
            {
                id = await _nuevoRegistroEtapa.Execute(nameEtapa);

            }
            dynamic response = new { id = id, value = nameEtapa };
            return Json(response);
        }
        public async Task<IActionResult> saveOrigen(string nameOrigen)
        {
            var id = 0;
            if (nameOrigen.Length != null)
            {
                id = await _nuevoRegistroOrigen.Execute(nameOrigen);

            }
            dynamic response = new { id = id, value = nameOrigen };
            return Json(response);
        }
        public async Task<IActionResult> saveAutoridad(string nameAutoridad)
        {
            var id = 0;
            if (nameAutoridad.Length != null)
            {
                id = await _nuevoRegistroAutoridad.Execute(nameAutoridad);

            }
            dynamic response = new { id = id, value = nameAutoridad };
            return Json(response);
        }
        public async Task<IActionResult> saveAspecto(string nameAspecto)
        {
            var id = 0;
            if (nameAspecto.Length != null)
            {
                id = await _nuevoRegistroAspecto.Execute(nameAspecto);

            }
            dynamic response = new { id = id, value = nameAspecto };
            return Json(response);
        }
        public async Task<IActionResult> saveComponente(string tipoComponente, string nameComponente, string servicioComponente)
        {
            var id = 0;
            if (nameComponente.Length != null && tipoComponente.Length != null)
            {
                id = await _nuevoRegistroComponente.Execute(tipoComponente, nameComponente, servicioComponente);

            }
            dynamic response = new { id = id, value = nameComponente };
            return Json(response);
        }
        public async Task<IActionResult> saveGerencia(string nameGerencia, string descripcionGerencia)
        {
            var id = 0;
            if (nameGerencia.Length != null)
            {
                id = await _nuevoRegistroGerencia.Execute(nameGerencia, descripcionGerencia);

            }
            dynamic response = new { id = id, value = nameGerencia };
            return Json(response);
        }
        public async Task<IActionResult> saveArea(string nameArea)
        {
            var id = 0;
            if (nameArea.Length != null)
            {
                id = await _nuevoRegistroArea.Execute(nameArea);

            }
            dynamic response = new { id = id, value = nameArea };
            return Json(response);
        }

    }
}
