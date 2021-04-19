using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using Microsoft.Extensions.Caching.Memory;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace GestionAmbiental.UseCases.Autenticacion
{
    public class AutenticacionController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGetRolUsuario _getRolUsuario;

        public AutenticacionController(IGetInfo_SGA info_SGA, IGetRolUsuario getRolUsuario)
        {
            _info_SGA = info_SGA;
            _getRolUsuario = getRolUsuario;
        }

        public IActionResult Signin()
        {
            return View(new SigninModel());
        }
        [HttpPost]
        public async Task<IActionResult> Signin(SigninModel model)
        {
            if (ModelState.IsValid)
            {
                // Get LDAP connection data
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
                // Validate credentials
                using (var context = new PrincipalContext(ContextType.Domain, ldap_domain))
                {
                    if (context.ValidateCredentials(model.User, model.Password, ContextOptions.Negotiate))
                    {
                        var rol = await _getRolUsuario.Execute(model.User);
                        if (!rol.HasValue)
                            return RedirectToAction("NoAutorizado", "Home");
                        else
                        {
                            var userInfo = new UserInfo { NombreUsuario = model.User, Rol = rol.Value };
                            HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        return View(new SigninModel { MensajeAut = "El usuario o la contraseña son incorrectas" });
                }
            }
            return View(new SigninModel());
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(SigninModel model)
        { 
            if (ModelState.IsValid)
            {
                dynamic result;
                var rol = await _getRolUsuario.Execute(model.User);
                if (!rol.HasValue)
                {
                    model.MensajeAut = "El usuario o la contraseña son incorrectas";
                    result = new
                    {
                        success = false,
                        mensaje = "El usuario o la contraseña son incorrectas"
                    };
                    
                }
                else
                {
                    var userInfo = new UserInfo { NombreUsuario = model.User, Rol = rol.Value };
                    HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));
                    result = new
                    {
                        success = true
                    };
                }
                return Json(result);
                //// Get LDAP connection data
                //var info = await _info_SGA.Execute(Tipo_Info_SGA.LDAPConnection);
                //string ldap_path = "", ldap_domain = "";
                //foreach (var item in info.Info)
                //{
                //    switch (item.Key)
                //    {
                //        case "ldap_path": ldap_path = item.Value; break;
                //        case "ldap_domain": ldap_domain = item.Value; break;
                //    }
                //}
                //// Validate credentials
                //using (var context = new PrincipalContext(ContextType.Domain, ldap_domain))
                //{
                //    if (context.ValidateCredentials(model.User, model.Password, ContextOptions.Negotiate))
                //    {
                //        var rol = await _getRolUsuario.Execute(model.User);
                //        if (!rol.HasValue)
                //            return RedirectToAction("NoAutorizado", "Home");
                //        else
                //        {
                //            var userInfo = new UserInfo { NombreUsuario = model.User, Rol = rol.Value };
                //            HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));
                //            return RedirectToAction("Index", "Home");
                //        }
                //    }
                //    else
                //        return View(new SigninModel { MensajeAut = "El usuario o la contraseña son incorrectas" });
                //}
            }
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index", "Home");
        }    
    }
}