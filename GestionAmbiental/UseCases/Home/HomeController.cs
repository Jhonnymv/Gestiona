using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetRolUsuario _getRolUsuario;

        public HomeController(IGetRolUsuario getRolUsuario)
        {
            _getRolUsuario = getRolUsuario;
        }

        public async Task<IActionResult> Index()
        {
            /*
            var rol = await _getRolUsuario.Execute("kevin");

            var userInfo = new UserInfo { NombreUsuario = "kevin", Rol = rol.Value };
            HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));

            ViewBag.Rol = userInfo.Rol.ToString();
            ViewBag.Usuario = userInfo.NombreUsuario;
            */
            
            if (HttpContext.Session.GetString("SessionUser")!= null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult NoAutorizado()
        {
            return View();
        }
    }
}
