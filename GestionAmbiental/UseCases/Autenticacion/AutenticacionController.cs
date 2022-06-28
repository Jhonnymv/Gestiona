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
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(SigninModel model)
        {
            dynamic result;
            result = new
            {
                success = false,
                mensaje = "Error en login"
            };
            if (ModelState.IsValid)
            {
                string returnUrl = TempData["returnUrl"] as string;
                // Get LDAP connection data
                var info = await _info_SGA.Execute(Tipo_Info_SGA.LDAPConnection);
                string ldap_path = "", ldap_domain = "";
                foreach (var item in info.Info)
                {
                   switch (item.Key)
                   {
                       case "Path": ldap_path = item.Value; break;
                       case "Domain": ldap_domain = item.Value; break;
                   }
                }
                // Validate credentials
                using (var context = new PrincipalContext(ContextType.Domain, ldap_domain))
                {
                   if (context.ValidateCredentials(model.User, model.Password, ContextOptions.Negotiate))
                   {
                       var rol = await _getRolUsuario.Execute(model.User);
                       if (!rol.HasValue)
                           result = new
                           {
                               success = false,
                               mensaje = "Usuario no autorizado"
                           };
                       else
                       {
                           var userInfo = new UserInfo { NombreUsuario = model.User, Rol = rol.Value };
                           HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));
                           result = new
                           {
                               success = true,
                               returnUrl = returnUrl
                           };
                       }
                   }
                   else
                       result = new
                       {
                           success = false,
                           mensaje = "El usuario o la contraseña son incorrectos"
                       };
                }
                // var rol = await _getRolUsuario.Execute(model.User);
                // if (!rol.HasValue)
                // {
                //     model.MensajeAut = "El usuario o la contraseña son incorrectas";
                //     result = new
                //     {
                //         success = false,
                //         mensaje = "El usuario o la contraseña son incorrectas"
                //     };

                // }
                // else
                // {
                //     var userInfo = new UserInfo { NombreUsuario = model.User, Rol = rol.Value };
                //     HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));
                //     result = new
                //     {
                //         success = true,
                //         returnUrl = returnUrl
                //     };
                // }
            }
            return Ok(result);
        }
        public IActionResult SignOut()
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index", "Home");
        }    
    }
}