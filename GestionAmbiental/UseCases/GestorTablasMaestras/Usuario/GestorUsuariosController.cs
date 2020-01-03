using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.AgregarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.EditarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Usuario
{
    public class GestorUsuariosController : Controller
    {
        private IMemoryCache _cache;
        private readonly IGetUsuarios _getUsuarios;
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarUsuario _editarUsuario;
        private readonly IAgregarUsuario _agregarUsuario;
        public GestorUsuariosController(
            IMemoryCache cache,
            IGetUsuarios getUsuarios,
            IGetUsuario getUsuario,
            IEditarUsuario editarUsuario,
            IAgregarUsuario agregarUsuario
            )
        {
            _cache = cache;
            _getUsuarios = getUsuarios ?? throw new ArgumentNullException(nameof(getUsuarios));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
            _editarUsuario = editarUsuario ?? throw new ArgumentNullException(nameof(editarUsuario));
            _agregarUsuario = agregarUsuario ?? throw new ArgumentNullException(nameof(agregarUsuario));
        }

        public IActionResult Usuarios()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Usuarios(string filtro, int pagina, int rpp)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var data = await _getUsuarios.Execute(filtro, pagina, rpp);
                var filasTabla = "";
                var paginador = "";
                int nPaginas = data.nUsuarios % rpp != 0 ? (data.nUsuarios / rpp) + 1 : data.nUsuarios / rpp;
                if (data.nUsuarios > 0)
                {
                    foreach (var usuario in data.Usuarios)
                    {

                        filasTabla += "<tr>";
                        filasTabla += "<td>" + usuario.Nombres + "</td>";
                        filasTabla += "<td>" + usuario.Apellidos + "</td>";
                        filasTabla += "<td>" + usuario.UsuarioRed + "</td>";
                        filasTabla += "<td>" + usuario.Email + "</td>";
                        filasTabla += "<td>" + usuario.Rol + "</td>";
                        filasTabla += "<td>" + usuario.Estado + "</td>";
                        filasTabla += "<td><a href=\"/GestorUsuarios/DetalleUsuario?IdUsuario=" + usuario.IdUsuario + "\">Detalle</a></td>";
                        filasTabla += "</tr>";
                    }

                    paginador += "<ul class=\"pagination\">";
                    for (int i = 1; i <= nPaginas; i++)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
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
        }
        public async Task<IActionResult> DetalleUsuario(int IdUsuario)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var usuario = await _getUsuario.Execute(IdUsuario);
                var usuarios = await _getUsuarios.Execute();

                var roles = new List<SelectListItem>
            {
                new SelectListItem("--SELECCIONE--", "0", true),
                new SelectListItem(InspeccionRol.Inspector.ToString(), ((int)InspeccionRol.Inspector).ToString(), false),
                new SelectListItem(InspeccionRol.AsesorMAP.ToString(), ((int)InspeccionRol.AsesorMAP).ToString(), false),
                new SelectListItem(InspeccionRol.AsesorSSO.ToString(), ((int)InspeccionRol.AsesorSSO).ToString(), false)
            };
                var listUsuarios = new List<SelectListItem>();
                listUsuarios.Add(new SelectListItem("Ninguno", "0", true));
                listUsuarios.AddRange(usuarios.Usuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList());



                ViewBag.Roles = roles;
                ViewBag.Usuarios = listUsuarios;


                var model = new UsuarioModel
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    Email = usuario.Email,
                    UsuarioRed = usuario.UsuarioRed,
                    IdRol = usuario.IdRol,
                    IdSuperior = usuario.IdSuperior,
                    Estado = usuario.Estado
                };

                return View(model);
            }
        }
        public async Task<IActionResult> AgregarUsuario()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var usuarios = await _getUsuarios.Execute();
                var roles = new List<SelectListItem>
                {
                new SelectListItem("--SELECCIONE--", "0", true),
                new SelectListItem(InspeccionRol.Inspector.ToString(), ((int)InspeccionRol.Inspector).ToString(), false),
                new SelectListItem(InspeccionRol.AsesorMAP.ToString(), ((int)InspeccionRol.AsesorMAP).ToString(), false),
                new SelectListItem(InspeccionRol.AsesorSSO.ToString(), ((int)InspeccionRol.AsesorSSO).ToString(), false)
                };
                var listUsuarios = new List<SelectListItem>();
                listUsuarios.Add(new SelectListItem("Ninguno", "0", true));
                listUsuarios.AddRange(usuarios.Usuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList());



                ViewBag.Roles = roles;
                ViewBag.Usuarios = listUsuarios;

                return View(new UsuarioModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsuarioModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var idUsuario = await _agregarUsuario.Execute(model.Nombres, model.Apellidos, model.IdRol, model.IdSuperior, model.Email, model.UsuarioRed);
                    return RedirectToAction("DetalleUsuario", new { IdUsuario = idUsuario });
                }
                return RedirectToAction("AgregarUsuario");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _editarUsuario.Execute(model.IdUsuario, model.Nombres, model.Apellidos, model.IdRol, model.IdSuperior, model.Email, model.UsuarioRed, model.Estado);
                }
                return RedirectToAction("DetalleUsuario", new { IdUsuario = model.IdUsuario });
            }
        }
    }
}