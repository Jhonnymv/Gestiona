using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.AgregarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.EditarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios;
using GestionAmbiental.Application.UseCases.GestorUsuarios.UpdateUsuarios;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Usuario
{
    public class GestorUsuariosController : Controller
    {
        private IMemoryCache _cache;
        private readonly IGetUsuarios _getUsuarios;
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarUsuario _editarUsuario;
        private readonly IAgregarUsuario _agregarUsuario;
        private readonly IUpdateUsuarios _updateUsuarios;

        private readonly IGetInfo_SGA _info_SGA;
        public GestorUsuariosController(
            IMemoryCache cache,
            IGetUsuarios getUsuarios,
            IGetUsuario getUsuario,
            IEditarUsuario editarUsuario,
            IAgregarUsuario agregarUsuario,
            IUpdateUsuarios updateUsuarios,
            IGetInfo_SGA info_SGA
            )
        {
            _cache = cache;
            _getUsuarios = getUsuarios ?? throw new ArgumentNullException(nameof(getUsuarios));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
            _editarUsuario = editarUsuario ?? throw new ArgumentNullException(nameof(editarUsuario));
            _agregarUsuario = agregarUsuario ?? throw new ArgumentNullException(nameof(agregarUsuario));
            _updateUsuarios = updateUsuarios ?? throw new ArgumentNullException(nameof(updateUsuarios));
            _info_SGA = info_SGA ?? throw new ArgumentNullException(nameof(info_SGA));
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
                new SelectListItem(InspeccionRol.AsesorSSO.ToString(), ((int)InspeccionRol.AsesorSSO).ToString(), false),
                new SelectListItem(InspeccionRol.Admin.ToString(), ((int)InspeccionRol.Admin).ToString(), false)
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
        /*public async Task<IActionResult> AgregarUsuario()
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
                new SelectListItem(InspeccionRol.AsesorSSO.ToString(), ((int)InspeccionRol.AsesorSSO).ToString(), false),
                new SelectListItem(InspeccionRol.Admin.ToString(), ((int)InspeccionRol.Admin).ToString(), false)
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
        }*/
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
        [HttpPost]
        public async Task<IActionResult> ActualizarInformacion()
        {
            var info = await _info_SGA.Execute(Tipo_Info_SGA.LDAPConnection);
            string ldap_path = "", ldap_domain = "";
            foreach (var item in info.Info)
            {
                switch (item.Key)
                {
                    case "ldap_path": ldap_path = item.Value; break;
                    case "ldap_domain": ldap_domain = item.Value; break;
                }
            }
            DirectoryEntry adSearchRoot = new DirectoryEntry(ldap_path);
            DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);

            adSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            adSearcher.PropertiesToLoad.Add("samaccountname");
            adSearcher.PropertiesToLoad.Add("mail");
            adSearcher.PropertiesToLoad.Add("givenName");
            adSearcher.PropertiesToLoad.Add("sn");
            adSearcher.PropertiesToLoad.Add("usergroup");
            adSearcher.PropertiesToLoad.Add("company");
            adSearcher.PropertiesToLoad.Add("department");
            adSearcher.PropertiesToLoad.Add("telephoneNumber");
            adSearcher.PropertiesToLoad.Add("mobile");
            adSearcher.PropertiesToLoad.Add("displayname");
            SearchResult result;
            SearchResultCollection iResult = adSearcher.FindAll();


            ADUserModel usuario;
            var rst = new List<ADUserModel>();
            dynamic response;
            if (iResult != null)
            {
                for (int counter = 0; counter < iResult.Count; counter++)
                {
                    result = iResult[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        usuario = new ADUserModel();
                        usuario.UserName = (String)result.Properties["samaccountname"][0];

                        if (result.Properties.Contains("displayname"))
                        {
                            usuario.DisplayName = (String)result.Properties["displayname"][0];
                        }
                        if (result.Properties.Contains("givenName"))
                        {
                            usuario.FirstName = (String)result.Properties["givenName"][0];
                        }
                        if (result.Properties.Contains("sn"))
                        {
                            usuario.LastName = (String)result.Properties["sn"][0];
                        }
                        if (result.Properties.Contains("mail"))
                        {
                            usuario.Email = (String)result.Properties["mail"][0];
                        }

                        if (result.Properties.Contains("company"))
                        {
                            usuario.Company = (String)result.Properties["company"][0];
                        }

                        if (result.Properties.Contains("title"))
                        {
                            usuario.JobTitle = (String)result.Properties["title"][0];
                        }

                        if (result.Properties.Contains("department"))
                        {
                            usuario.Deparment = (String)result.Properties["department"][0];
                        }

                        if (result.Properties.Contains("telephoneNumber"))
                        {
                            usuario.Phone = (String)result.Properties["telephoneNumber"][0];
                        }

                        if (result.Properties.Contains("mobile"))
                        {
                            usuario.Mobile = (String)result.Properties["mobile"][0];
                        }
                        rst.Add(usuario);
                    }
                }
            }
            else
            {
                response = new
                {
                    Mensaje = "No se encontraron usuarios.",
                    Success = false
                };
                return Ok(response);
            }

            adSearcher.Dispose();
            adSearchRoot.Dispose();
            await _updateUsuarios.Execute(JsonConvert.SerializeObject(rst));

            response = new
            {
                Mensaje = "Se actualizó la información de los usuarios",
                Success = true
            };
            return Ok(response);
        }
    }
}