using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.EditarUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios;
using GestionAmbiental.Application.UseCases.GestorUsuarios.UpdateUsuarios;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Usuario
{
    public class GestorUsuariosController : Controller
    {
        private readonly IGetUsuarios _getUsuarios;
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarUsuario _editarUsuario;
        private readonly IUpdateUsuarios _updateUsuarios;

        private readonly IGetInfo_SGA _info_SGA;
        public GestorUsuariosController(
            IGetUsuarios getUsuarios,
            IGetUsuario getUsuario,
            IEditarUsuario editarUsuario,
            IUpdateUsuarios updateUsuarios,
            IGetInfo_SGA info_SGA
            )
        {
            _getUsuarios = getUsuarios ?? throw new ArgumentNullException(nameof(getUsuarios));
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
            _editarUsuario = editarUsuario ?? throw new ArgumentNullException(nameof(editarUsuario));
            _updateUsuarios = updateUsuarios ?? throw new ArgumentNullException(nameof(updateUsuarios));
            _info_SGA = info_SGA ?? throw new ArgumentNullException(nameof(info_SGA));
        }

        public IActionResult Usuarios()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Usuarios(string filtro, int pagina, int rpp)
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
            }
        }
        public async Task<IActionResult> DetalleUsuario(int IdUsuario)
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
                    ViewBag.Rol = userInfo.Rol.ToString();
                    var usuario = await _getUsuario.Execute(IdUsuario);
                    var usuarios = await _getUsuarios.Execute();

                    var enumList = Enum.GetValues(typeof(RSRoles)).OfType<RSRoles>().ToList();

                    var roles = enumList.Select(g => new SelectListItem() { Value = ((int)g).ToString(), Text = g.ToString() }).ToList();
                    
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
        }
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioModel model)
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
                        await _editarUsuario.Execute(model.IdUsuario, model.Nombres, model.Apellidos, model.IdRol, model.IdSuperior, model.Email, model.UsuarioRed, model.Estado);
                    return RedirectToAction("DetalleUsuario", new { IdUsuario = model.IdUsuario });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarInformacion()
        {
            var info = await _info_SGA.Execute(Tipo_Info_SGA.LDAPConnection);
            string ldap_path = "", ldap_domain = "", user = "", password = "", groupName = "G_Users_Marcobre";
            var groupnames = new List<string>();

            foreach (var item in info.Info)
            {
                switch (item.Key)
                {
                    case "ldap_path": ldap_path = item.Value; break;
                    case "ldap_domain": ldap_domain = item.Value; break;
                    case "user": user = item.Value;break;
                    case "password": password = item.Value;break;
                    case "group_name":groupName = item.Value;break;
                }
            }
            groupnames.Add(groupName);
            groupnames.Add("G_Users_Gestiona_Externos");
            
                DirectoryEntry adSearchRoot;
            if (user != "" || password != "")
                adSearchRoot = new DirectoryEntry(ldap_path, user, password);
            else
                adSearchRoot = new DirectoryEntry(ldap_path);
            
            var rst = new List<ADUserModel>();

            dynamic response;

            foreach (var item in groupnames)
            {
                DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);
                adSearcher.Filter = "(&(objectClass=group)(cn=" + item + "))";

                SearchResult searchResult = adSearcher.FindOne();

                if (searchResult != null)
                {
                    DirectoryEntry deGroup;
                    if (user != "" || password != "")
                        deGroup = new DirectoryEntry(searchResult.Path, user, password, AuthenticationTypes.Secure);
                    else
                        deGroup = new DirectoryEntry(searchResult.Path);

                    // Get the group's property collection.
                    System.DirectoryServices.PropertyCollection propertyCollection = deGroup.Properties;

                    int propertyCount = propertyCollection["member"].Count;
                    for (int j = 0; j < propertyCount; j++)
                    {
                        DirectoryEntry deUser;
                        if (user != "" || password != "")
                            deUser = new DirectoryEntry(@"LDAP://" + ldap_domain + "/" + propertyCollection["member"][j].ToString(), user, password, AuthenticationTypes.Secure);
                        else
                            deUser = new DirectoryEntry(@"LDAP://" + ldap_domain + "/" + propertyCollection["member"][j].ToString());

                        var info_usuario = new ADUserModel();
                        info_usuario.UserName = GetValidProperty(deUser, "samaccountname");
                        info_usuario.DisplayName = GetValidProperty(deUser, "displayname");
                        info_usuario.Email = GetValidProperty(deUser, "mail");
                        info_usuario.FirstName = GetValidProperty(deUser, "givenName");
                        info_usuario.LastName = GetValidProperty(deUser, "sn");
                        info_usuario.Celular = GetValidProperty(deUser, "mobilePhone");
                        info_usuario.Puesto = GetValidProperty(deUser, "title");
                        info_usuario.Jefe = GetValidProperty(deUser, "manager");
                        info_usuario.Area = GetValidProperty(deUser, "department");
                        info_usuario.Empresa = GetValidProperty(deUser, "company");
                        info_usuario.DNI = GetValidProperty(deUser, "facsimileTelephoneNumber");

                        /*
                        info_usuario.Company = GetValidProperty(deUser, "company");
                        info_usuario.Deparment = GetValidProperty(deUser, "department");
                        info_usuario.Phone = GetValidProperty(deUser, "telephoneNumber");
                        info_usuario.Mobile = GetValidProperty(deUser, "mobile");
                        info_usuario.JobTitle = GetValidProperty(deUser, "title");*/
                        rst.Add(info_usuario);

                        deUser.Close();
                    }
                    deGroup.Close();
                    adSearchRoot.Close();
                    rst = rst.OrderBy(x => x.UserName).ToList();
                }

                adSearcher.Dispose();
                adSearchRoot.Dispose();
                await _updateUsuarios.Execute(JsonConvert.SerializeObject(rst));

                
            }
            if (rst.Count > 0)
            {
                response = new
                {
                    Mensaje = "Se actualizó la información de los usuarios",
                    Success = true
                };
                return Ok(response);
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
        }
        private string GetValidProperty(DirectoryEntry directoryEntry, string propertyName)
        {
            if (directoryEntry.Properties.Contains(propertyName))
            {
                return directoryEntry.Properties[propertyName].Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}