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

namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class CompromisosMCBController : Controller
    {
        private readonly INuevoRegistro _nuevoRegistro;
        private readonly ISelectItems _selectItems;
        private readonly IEditarRegistro _editarRegistro;
        private readonly Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros.IListarRegistros _listarCompromisos;
        private readonly Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros.IListarRegistros _listarSustentos;

        public CompromisosMCBController(
            INuevoRegistro nuevoRegistro,
            ISelectItems selectItems,
            IEditarRegistro editarRegistro,
            Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros.IListarRegistros listarCompromisos,
            Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros.IListarRegistros listarSustentos
            )
        {
            _nuevoRegistro = nuevoRegistro;
            _selectItems = selectItems;
            _listarCompromisos = listarCompromisos;
            _listarSustentos = listarSustentos;
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



                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListarSustentos(int pagina, int rpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            var datosTabla = await _listarSustentos.Execute(pagina,rpp,gerencias,areas,criterios,estados);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                //Contenido de la tabla
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"#\"></a><button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button> </td>";
                    filasTabla += "<td>" + fila.IdCompromiso + "</td>";
                    filasTabla += "<td>" + fila.IdResponsable1 + "</td>";
                    filasTabla += "<td>" + fila.IdResponsable2 + "</td>";
                    filasTabla += "<td>" + fila.IdAsesor + "</td>";
                    filasTabla += "<td>" + fila.Medio_verificacion + "</td>";
                    filasTabla += "<td>" + fila.Detalle_compromiso + "</td>";
                    filasTabla += "<td>" + fila.Sustento_cumplimiento + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
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

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(CompromisoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _nuevoRegistro.Execute(model.Codigo, (int)model.Id_Criterio, model.Detalle_compromiso, model.Id_Etapa, model.Texto_compromiso, model.Id_Origen, model.Tomo,
                    model.Capitulo, model.Seccion, model.Pagina, model.Resolucion, model.Id_Autoridad, model.Fecha, model.Id_Compromiso, model.Id_Aspecto, model.Id_Componente, model.Id_Gerencia, model.Id_Area,
                    model.Frecuencia, model.Id_Entidad, model.Id_Responsable1, model.Id_Responsable2, model.Frecuencia_reporte, model.Sustentos, model.Estado, model.Id_Aprueba,
                    model.Medio_verificacion);

                return RedirectToAction("Resumen");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Listar(int pagina, int rpp)
        {
            var datosTabla = await _listarCompromisos.Execute(pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                //Contenido de la tabla
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/CompromisosMCB/EditarRegistro?id=" + fila.Id + "\">" + fila.Id + "</a>  <button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button> </td>";
                    filasTabla += "<td>" + fila.Codigo + "</td>";
                    filasTabla += "<td>" + fila.NomCriterio + "</td>";
                    filasTabla += "<td>" + fila.Detalle_compromiso + "</td>";
                    filasTabla += "<td>" + fila.NomEtapa + "</td>";
                    filasTabla += "<td>" + fila.NomAspecto + "</td>";
                    filasTabla += "<td>" + fila.NomComponente + "</td>";
                    filasTabla += "<td>" + fila.NomGerencia + "</td>";
                    filasTabla += "<td>" + fila.NomArea + "</td>";
                    filasTabla += "<td>" + fila.Frecuencia + "</td>";
                    filasTabla += "<td>" + fila.NomResponsable1 + "</td>";
                    filasTabla += "<td>" + fila.Frecuencia_reporte + "</td>";
                    filasTabla += "<td>" + fila.Sustentos + "</td>";
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
        public async Task<IActionResult> GetCompromiso(int idCompromiso)
        {

            var datos = await _listarCompromisos.BuscarPorID(idCompromiso);
            dynamic response =new { Data = datos };
            
            return Ok(response);

        }
        public async Task<IActionResult> EditarRegistro(int id)
        {
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


            var registros = await _listarCompromisos.Execute(1, 100);

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
                    model.Medio_verificacion
                    );

                return RedirectToAction("EditarRegistro", new { id = model.Id });
            }
            return RedirectToAction("EditarRegistro", new { id = model.Id });
        }
    }
}
