using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.EditarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarFoto;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.Items;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.RegistrarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.ValidarPermisos;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using ImageMagick;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.AvistamientoFauna
{
    public class AvistamientoFaunaController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IGetItems _getItems;
        private readonly IGetAvistamientosFauna _getAvistamientosFauna;
        private readonly IGetAvistamientoFauna _getAvistamientoFauna;

        private readonly IGetUsuario _getUsuario;
        private readonly Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario.IGetUsuario _getUsuarioId;
        private readonly IRegistrarAvistamientoFauna _registrarAvistamientoFauna;
        private readonly IEditarAvistamientoFauna _editarAvistamientoFauna;
        private readonly IEliminarAvistamientoFauna _eliminarAvistamientoFauna;

        private readonly IEliminarFoto _eliminarFoto;
        private readonly IValidarPermisos _validarPermisos;

        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public AvistamientoFaunaController(
            IHostingEnvironment hostingEnvironment,
            IGetItems getItems,
            IGetAvistamientosFauna getAvistamientosFauna,
            IGetAvistamientoFauna getAvistamientoFauna,
            IGetUsuario getUsuario, 
            Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario.IGetUsuario getUsuarioId,
            IGetInfo_SGA info_SGA,
            IRegistrarAvistamientoFauna registrarAvistamientoFauna,
            IEliminarAvistamientoFauna eliminarAvistamientoFauna,
            IDataProtectionProvider dataProtectionProvider,
            IEliminarFoto eliminarFoto,
            IValidarPermisos validarPermisos,
            IEditarAvistamientoFauna editarAvistamientoFauna,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _hostingEnvironment = hostingEnvironment;
            _getItems = getItems;
            _getAvistamientosFauna = getAvistamientosFauna;
            _getAvistamientoFauna = getAvistamientoFauna;
            _getUsuario = getUsuario;
            _getUsuarioId = getUsuarioId;
            _registrarAvistamientoFauna = registrarAvistamientoFauna;
            _eliminarAvistamientoFauna = eliminarAvistamientoFauna;
            _editarAvistamientoFauna = editarAvistamientoFauna;
            _eliminarFoto = eliminarFoto;
            _validarPermisos = validarPermisos;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.AvistamientoFaunaIdRouteValue);
        }
        public async Task<IActionResult> PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Fauna"));
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
        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugaresReferencia = items.LugaresReferencia.OrderBy(x =>x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugaresReferencia.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());
                
                var mensajeContactos = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.WebRootPath, "ConfigFiles", "AvistamientoFauna.txt"), System.Text.Encoding.GetEncoding("iso-8859-1"));
                mensajeContactos = mensajeContactos.Substring(10);

                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.LugarReferencia = lugaresReferencia;
                ViewBag.Mensaje = mensajeContactos;
                ViewBag.Contactos = contactosEmergencia;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int tipo_fauna, int estado_fauna, int lugar_referencia, int rpp, int pagina,string usuario_red)
        {
            var datosTabla = await _getAvistamientosFauna.Execute(fecha, tipo_fauna, estado_fauna, lugar_referencia, rpp, pagina);
            var usuario = await _getUsuario.Execute(usuario_red);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NAvistamientos % rpp != 0 ? (datosTabla.NAvistamientos / rpp) + 1 : datosTabla.NAvistamientos / rpp;
            if (datosTabla.NAvistamientos > 0)
            {
                foreach (var fila in datosTabla.Avistamientos)
                {
                    if (!fila.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()))
                    {
                        var IdEncriptado = _protector.Protect(fila.IdAvistamientoFauna.ToString());
                        filasTabla += "<tr>";                        
                        if ((fila.Estado.Equals(EstadosAvistamiento.Registrado.ToString()) || fila.Estado.Equals(EstadosAvistamiento.Completado.ToString())) && !usuario.IdRol.Equals((int)InspeccionRol.Admin))
                        {
                            if(fila.FechaAvistamiento > DateTime.Now.AddDays(-1))
                                filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                            else
                                filasTabla += "<td></td>";
                            filasTabla += "<td><a href=\"/AvistamientoFauna/DetalleRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        }
                        else if(fila.Estado.Equals(EstadosAvistamiento.Pendiente.ToString()) || usuario.IdRol.Equals((int)InspeccionRol.Admin))
                        {
                            filasTabla += "<td></td>";
                            filasTabla += "<td><a href=\"/AvistamientoFauna/EditarRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        }
                        filasTabla += "<td>" + fila.FechaAvistamiento.ToString("dd/MM/yyyy") + "</td>";
                        filasTabla += "<td>" + fila.FechaAvistamiento.ToString("HH:mm") + "</td>";
                        filasTabla += "<td>" + fila.Ubicacion + "</td>";
                        filasTabla += "<td>" + fila.Cantidad + "</td>";
                        filasTabla += "<td>" + fila.TipoFauna + "</td>";
                        filasTabla += "<td>" + fila.NombreComun + "</td>";
                        filasTabla += "<td>" + fila.EstadoFauna + "</td>";
                        filasTabla += "<td>" + fila.TipoAvistamiento + "</td>";
                        filasTabla += "</tr>";
                    }
                }

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
                var nCol = 10;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> CatalogoFauna()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> CargarCatalogo()
        {
            var items = await _getItems.Execute();
            var tiposFauna = items.TiposFauna;
            var nombresComunes = items.NombresComunes;

            var catalogo = "";
            foreach (var tipoFauna in tiposFauna)
            {
                var tfDescripcion = tipoFauna.Descripcion.Replace(" ", String.Empty);
                catalogo += "<a class='catalog-item' data-bs-toggle='modal' data-bs-target='#faunaModal" + tfDescripcion + "'>";
                catalogo += "<div class='catalog-item-img'>";
                catalogo += "<img src='/img/fauna" + tipoFauna.IdTipoFauna + ".png'>";
                catalogo += "</div>";
                catalogo += "<div class='catalog-item-cover'>";
                catalogo += "<span>" + tipoFauna.Descripcion + "</span>";
                catalogo += "</div>";
                catalogo += "</a>";


                catalogo += "<div class='modal fade' id='faunaModal" + tfDescripcion + "' tabindex='-1' aria-labelledby='faunaModal" + tfDescripcion + "' aria-hidden='true'>";
                catalogo += "<div class='modal-dialog modal-lg modal-dialog-centered'>";
                catalogo += "<div class='modal-menu modal-fauna'>";
                catalogo += "<div class='modal-content'>";
                catalogo += "<div class='fauna'>";
                catalogo += "<div class='image-header-container'>";
                catalogo += "<div class='header-title fauna-opacity-color'>";
                catalogo += "<h3>Catálogo de " + tipoFauna.Descripcion + "</h3>";
                catalogo += "<img src='/img/logo-white.png' alt='Logo Marcobre'>";
                catalogo += "</div>";
                catalogo += "<button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "<div class='modal-body'>";
                catalogo += "<div class='fauna'>";
                catalogo += "<div class='catalog-items'>";

                var especiesPorTipo = nombresComunes.Where(x => x.TipoFauna.IdTipoFauna.Equals(tipoFauna.IdTipoFauna)).ToList();
                foreach (var nombreComun in especiesPorTipo)
                {
                    catalogo += "<a class='catalog-item'>";
                    catalogo += "<div class='catalog-item-img'>";
                    catalogo += "<img src='" + nombreComun.Foto + "' alt='" + nombreComun.Foto + "'>";
                    catalogo += "</div>";
                    catalogo += "<div class='catalog-item-detail-cover'>";
                    catalogo += "<span>" + nombreComun.Descripcion + "</span>";
                    catalogo += "</div>";
                    catalogo += "</a>";
                }

                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "<div class='modal-footer'>";
                catalogo += "<button class='btn btn-mc' data-bs-dismiss='modal' aria-label='Close'>Cerrar</button>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
            }
            dynamic response = new
            {
                catalogo = catalogo
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

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var model = new AvistamientoFaunaModel();
                var usuario = await _getUsuario.Execute(userInfo.NombreUsuario);
                model.IdUsuario = usuario.IdUsuario;
                model.Nombres_Apellidos = usuario.Nombres + " " + usuario.Apellidos;
                if (usuario.Id_Empresa != null)
                    model.IdEmpresa = usuario.Id_Empresa.Value;
                if (usuario.Id_Area_Trabajo != null)
                    model.IdAreaTrabajo = usuario.Id_Area_Trabajo.Value;
                model.DNI = usuario.DNI;
                model.IdUsuario = usuario.IdUsuario;

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAvistamiento = items.TiposAvistamiento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();
                tipoAvistamiento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresas = items.Empresas.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var areas = items.AreasTrabajo.Select(g => new SelectListItem() { Value = g.IdAreaTrabajo.ToString(), Text = g.Descripcion }).ToList();
                areas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugares = items.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugares.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nombresComunes = new List<SelectListItem>();
                foreach (var grupo in items.TiposFauna)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    nombresComunes.AddRange(items.NombresComunes.Where(y => y.TipoFauna.IdTipoFauna.Equals(grupo.IdTipoFauna)).OrderBy(x => x.Descripcion).Select(i => new SelectListItem() { Value = i.IdNombreComunFauna.ToString(), Text = i.Descripcion, Group = nuevoGrupo }).ToList());
                }
                nombresComunes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                ViewBag.Contactos = contactosEmergencia;
                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.NombreComun = nombresComunes;
                ViewBag.TipoAvistamiento = tipoAvistamiento;
                ViewBag.Empresa = empresas;
                ViewBag.AreaTrabajo = areas;
                ViewBag.LugarReferencia = lugares;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(AvistamientoFaunaModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var fotos = new List<string>();
                for (int i = 0; i < model.Fotos.Count; i++)
                {
                    var filePath = "";

                    var extension = Path.GetExtension(model.Fotos[i].FileName);
                    var max_file_size = 8388608;
                    
                    var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

                    if (allowed_extensions.Contains(extension) && model.Fotos[i].Length < max_file_size)
                    {
                        var newfileName = CreateUniqueFileName(i+1) + extension;
                        filePath = await CargarImagen(model.Fotos[i], newfileName);
                        fotos.Add(filePath);
                    }
                }

                var IdAvistamiento = await _registrarAvistamientoFauna.Execute(userInfo.NombreUsuario,fotos.ToArray(),
                    model.Fecha_Avistamiento,
                    model.Hora_Avistamiento,
                    model.Ubicacion,
                    model.Cantidad,
                    model.IdNombreComun,
                    model.IdTipoAvistamiento,
                    model.IdEstadoFauna,
                    model.IdLugarReferencia,
                    model.IdUsuario,
                    model.IdEmpresa,
                    model.IdAreaTrabajo,
                    model.DNI,
                    model.Opcion);
                if (model.Opcion.Equals("GuardarEstado"))
                    return RedirectToAction("Resumen");
                else if (model.Opcion.Equals("Registrar"))
                    return RedirectToAction("NotificarNuevoAvistamiento", "NotificacionesAvistamientoFauna", new { IdAvistamiento = _protector.Protect(IdAvistamiento.ToString()) });
                
            }
            return RedirectToAction("NuevoRegistro");
        }
        public async Task<IActionResult> EditarRegistro(string IdAvistamiento)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAvistamiento=" + IdAvistamiento });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var idAvistamiento = Int32.Parse(_protector.Unprotect(IdAvistamiento));
                var avistamiento = await _getAvistamientoFauna.Execute(idAvistamiento);

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAvistamiento = items.TiposAvistamiento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();
                tipoAvistamiento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresas = items.Empresas.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var areas = items.AreasTrabajo.Select(g => new SelectListItem() { Value = g.IdAreaTrabajo.ToString(), Text = g.Descripcion }).ToList();
                areas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugares = items.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugares.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nombresComunes = new List<SelectListItem>();
                foreach (var grupo in items.TiposFauna)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    nombresComunes.AddRange(items.NombresComunes.Where(y => y.TipoFauna.IdTipoFauna.Equals(grupo.IdTipoFauna)).OrderBy(x => x.Descripcion).Select(i => new SelectListItem() { Value = i.IdNombreComunFauna.ToString(), Text = i.Descripcion, Group = nuevoGrupo }).ToList());
                }
                nombresComunes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                ViewBag.Contactos = contactosEmergencia;
                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.NombreComun = nombresComunes;
                ViewBag.TipoAvistamiento = tipoAvistamiento;
                ViewBag.Empresa = empresas;
                ViewBag.AreaTrabajo = areas;
                ViewBag.LugarReferencia = lugares;

                var model = new AvistamientoFaunaModel();
                model.IdAvistamientoFauna = avistamiento.IdAvistamientoFauna;
                model.Codigo = avistamiento.Codigo;
                model.IdEncriptado = IdAvistamiento;
                model.Fecha_Avistamiento = avistamiento.FechaAvistamiento.ToString("yyyy-MM-dd");
                model.Hora_Avistamiento = avistamiento.FechaAvistamiento.ToString("HH:mm");
                model.Ubicacion = avistamiento.Ubicacion;
                model.Cantidad = avistamiento.Cantidad;
                model.IdNombreComun = avistamiento.IdNombreComun;
                model.IdTipoAvistamiento = avistamiento.IdTipoAvistamiento;
                model.IdEstadoFauna = avistamiento.IdEstadoFauna;
                model.IdLugarReferencia = avistamiento.IdLugarReferencia;

                model.IdUsuario = avistamiento.IdUsuario;
                model.Nombres_Apellidos = avistamiento.Usuario;
                if (avistamiento.IdEmpresa != null)
                    model.IdEmpresa = avistamiento.IdEmpresa.Value;
                if (avistamiento.IdAreaTrabajo != null)
                    model.IdAreaTrabajo = avistamiento.IdAreaTrabajo.Value;
                model.DNI = avistamiento.DNI;

                model.FotosCargadas = avistamiento.Fotos != null ? JsonConvert.SerializeObject(avistamiento.Fotos) : null;
                model.Estado = avistamiento.Estado;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarRegistro(AvistamientoFaunaModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var fotos = new List<string>();

                if (model.FotosCargadas != null)
                {
                    var listaImagenes = new List<string>(model.FotosCargadas.Split(','));
                    fotos.AddRange(listaImagenes);
                    var vacios = new List<int>();
                    foreach (var foto in fotos)
                    {
                        if (foto.Length == 0)
                            vacios.Add(fotos.IndexOf(foto));
                    }
                    foreach(var vacio in vacios)
                    {
                        fotos.RemoveAt(vacio);
                    }
                }
                if (model.Fotos != null)
                    for (int i = 0; i < model.Fotos.Count; i++)
                    {
                        var filePath = "";

                        var extension = Path.GetExtension(model.Fotos[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

                        if (allowed_extensions.Contains(extension) && model.Fotos[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i+1) + extension;
                            filePath = await CargarImagen(model.Fotos[i], newfileName);
                            fotos.Add(filePath);
                        }
                    }
                
                await _editarAvistamientoFauna.Execute(userInfo.NombreUsuario,
                    model.IdAvistamientoFauna,
                    model.Codigo,
                    fotos.ToArray(),
                    model.Fecha_Avistamiento,
                    model.Hora_Avistamiento,
                    model.Ubicacion,
                    model.Cantidad,
                    model.IdNombreComun,
                    model.IdTipoAvistamiento,
                    model.IdEstadoFauna,
                    model.IdLugarReferencia,
                    model.IdUsuario,
                    model.IdEmpresa,
                    model.IdAreaTrabajo,
                    model.DNI,
                    model.Opcion,
                    model.Estado);

                if (model.Opcion.Equals("GuardarEstado")|| model.Opcion.Equals("EditarAdmin"))
                    return RedirectToAction("Resumen");
                else if (model.Opcion.Equals("Registrar"))
                    return RedirectToAction("NotificarNuevoAvistamiento", "NotificacionesAvistamientoFauna", new { IdAvistamiento = model.IdEncriptado });
            }
            return RedirectToAction("EditarRegistro", new { IdAvistamiento = model.IdEncriptado });
        }
        public async Task<IActionResult> DetalleRegistro(string IdAvistamiento)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAvistamiento=" + IdAvistamiento });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var idAvistamiento = Int32.Parse(_protector.Unprotect(IdAvistamiento));
                var avistamiento = await _getAvistamientoFauna.Execute(idAvistamiento);

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAvistamiento = items.TiposAvistamiento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();
                tipoAvistamiento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresas = items.Empresas.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var areas = items.AreasTrabajo.Select(g => new SelectListItem() { Value = g.IdAreaTrabajo.ToString(), Text = g.Descripcion }).ToList();
                areas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugares = items.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugares.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nombresComunes = new List<SelectListItem>();
                foreach (var grupo in items.TiposFauna)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    nombresComunes.AddRange(items.NombresComunes.Where(y => y.TipoFauna.IdTipoFauna.Equals(grupo.IdTipoFauna)).OrderBy(x => x.Descripcion).Select(i => new SelectListItem() { Value = i.IdNombreComunFauna.ToString(), Text = i.Descripcion, Group = nuevoGrupo }).ToList());
                }
                nombresComunes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                ViewBag.Contactos = contactosEmergencia;
                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.NombreComun = nombresComunes;
                ViewBag.TipoAvistamiento = tipoAvistamiento;
                ViewBag.Empresa = empresas;
                ViewBag.AreaTrabajo = areas;
                ViewBag.LugarReferencia = lugares;

                var model = new AvistamientoFaunaModel();
                model.IdAvistamientoFauna = avistamiento.IdAvistamientoFauna;
                model.Fecha_Avistamiento = avistamiento.FechaAvistamiento.ToString("yyyy-MM-dd");
                model.Hora_Avistamiento = avistamiento.FechaAvistamiento.ToString("HH:mm");
                model.Ubicacion = avistamiento.Ubicacion;
                model.Cantidad = avistamiento.Cantidad;
                model.IdNombreComun = avistamiento.IdNombreComun;
                model.IdTipoAvistamiento = avistamiento.IdTipoAvistamiento;
                model.IdEstadoFauna = avistamiento.IdEstadoFauna;
                model.IdLugarReferencia = avistamiento.IdLugarReferencia;
                
                model.IdUsuario = avistamiento.IdUsuario;
                model.Nombres_Apellidos = avistamiento.Usuario;
                if (avistamiento.IdEmpresa != null)
                    model.IdEmpresa = avistamiento.IdEmpresa.Value;
                if (avistamiento.IdAreaTrabajo != null)
                    model.IdAreaTrabajo = avistamiento.IdAreaTrabajo.Value;
                model.DNI = avistamiento.DNI;

                model.FotosCargadas = avistamiento.Fotos != null ? JsonConvert.SerializeObject(avistamiento.Fotos) : null;

                model.Descripcion_Accion = avistamiento.DescripcionAccion;
                model.Evidencia_Cargada = avistamiento.EvidenciaAccion != null ? JsonConvert.SerializeObject(avistamiento.EvidenciaAccion) : null;
                model.Estado = avistamiento.Estado;

                return View(model);
            }
        }        
        [HttpPost]
        public async Task<IActionResult> ObtenerImagenesCargadas(string imagenes)
        {
            dynamic response;
            if (HttpContext.Session.GetString("SessionUser") != null && imagenes != null)
            {
                var listaImagenes = new List<string>(imagenes.Split(','));
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var imagesSrc = new List<ImagenCargada>();
                for (int i = 0; i < listaImagenes.Count; i++)
                {
                    var encryptedFile = await DownloadFTPFileAsync(listaImagenes[i]);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-v2\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado" + i.ToString() + Path.GetExtension(listaImagenes[i]);
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    byte[] imageByteData = System.IO.File.ReadAllBytes(archivePath + decryptedFileName);
                    string imageBase64Data = Convert.ToBase64String(imageByteData);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                    imagesSrc.Add(new ImagenCargada
                    {
                        Nombre = listaImagenes[i],
                        Src = imageDataURL
                    });
                }

                response = new
                {
                    imagenes = JsonConvert.SerializeObject(imagesSrc)
                };
            }
            else
            {
                response = new
                {
                    imagenes = "[]"
                };
            }
            return Ok(response);
        }        
        public async Task<IActionResult> ObtenerImagenesReferencia()
        {
            var items = await _getItems.Execute();
            var nombresComunes = items.NombresComunes.ToList();
            dynamic response = new
            {
                Ids = nombresComunes.OrderBy(x=>x.IdNombreComunFauna).Select(x=>x.IdNombreComunFauna).ToArray(),
                Nombres = nombresComunes.OrderBy(x => x.IdNombreComunFauna).Select(x => x.Descripcion).ToArray(),
                Fotos = nombresComunes.OrderBy(x => x.IdNombreComunFauna).Select(x => x.Foto).ToArray()
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarImagen(int IdAvistamiento, string imagen)
        {
            await DeleteFTPFileAsync(imagen);

            await _eliminarFoto.Execute(IdAvistamiento, imagen);

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> EliminarRegistro(int IdAvistamiento,string imagenes)
        {

            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAvistamiento=" + IdAvistamiento + "&imagenes="+imagenes });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var avistamiento = await _getAvistamientoFauna.Execute(IdAvistamiento);
                var xd = await _validarPermisos.Editar(userInfo.NombreUsuario, avistamiento.IdUsuario);
                if (await _validarPermisos.Editar(userInfo.NombreUsuario, avistamiento.IdUsuario) || userInfo.Rol.Equals(InspeccionRol.Admin))
                {

                    var fotos = new List<string>();
                    if (imagenes != null)
                    {
                        var listaImagenes = new List<string>(imagenes.Split(','));
                        fotos.AddRange(listaImagenes);
                        var vacios = new List<int>();
                        foreach (var foto in fotos)
                        {
                            if (foto.Length == 0)
                                vacios.Add(fotos.IndexOf(foto));
                        }
                        foreach (var vacio in vacios)
                        {
                            fotos.RemoveAt(vacio);
                        }
                        foreach (var foto in fotos)
                        {
                            await EliminarImagen(IdAvistamiento, foto);
                        }
                    }
                    await _eliminarAvistamientoFauna.Execute(userInfo.NombreUsuario, IdAvistamiento);

                    return RedirectToAction("Resumen");
                }
                else
                {
                    var IdEncriptado = _protector.Protect(IdAvistamiento.ToString());
                    return RedirectToAction("EditarRegistro", new { IdAvistamiento = IdEncriptado });
                }
            }
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Avistamiento_Fauna_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")+ "_" + nFile;
            return fileName;
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private async Task<string> CargarImagen(IFormFile file, string newFileName)
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

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    file.CopyTo(stream);

                    ImageOptimizer optimizer = new ImageOptimizer();

                    // Set the stream position to the beginning of the file.
                    stream.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        optimizer.LosslessCompress(stream);
                    }
                    catch (Exception e) { }
                    stream.Flush();

                    stream.Close();
                }
                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/" + newFileName;
                await EncryptFileAsync(fileName, encryptedArchiveFile);

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
            else
                return "Error";
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFauna);
                string host = "", port = "", user = "", pass = "", dir = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value; break;
                    }
                }

                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);
                    using (FileStream fs = System.IO.File.OpenRead(path))
                    {
                        sftp.UploadFile(fs, newFileName, true);
                    }
                    sftp.Dispose();
                }
                return newFileName;
            }
            else
            {
                return "No Autorizado";
            }
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
        public async Task<IActionResult> Download(string fileName)
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
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-v2\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado" + Path.GetExtension(fileName);
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFauna);
                string host = "", port = "", user = "", pass = "", dir = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value; break;
                    }
                }
                /* Save file in application server to decrypt */
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
                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream = System.IO.File.Create(downloadedFilePath);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(fileName, fileStream);

                    sftp.Dispose();
                }
                fileStream.Close();

                return downloadedFilePath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        private async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            RijndaelManaged RMCrypto = new RijndaelManaged();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
        private async Task DeleteFTPFileAsync(string filename)
        {
            var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFauna);
            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                sftp.DeleteFile(filename);

                sftp.Dispose();
            }
        }
        private class ImagenCargada
        {
            public string Nombre { get; set; }
            public string Src { get; set; }
        }
    }
}