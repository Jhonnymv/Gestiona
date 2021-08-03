using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Serilog;

namespace GestionAmbiental.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetRolUsuario _getRolUsuario;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IGetRolUsuario getRolUsuario,
            IHostingEnvironment hostingEnvironment)
        {
            _getRolUsuario = getRolUsuario;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeModel();
            try
            {
                var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");

                var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

                var cards = JsonConvert.DeserializeObject<List<TarjetaModel>>(cardsText);

                model.Tarjetas = cards.OrderBy(x=>x.Fecha).ToList();
            }
            catch(Exception e)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                Log.Error(e.Message);
            }
                       
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
            }
            
            return View(model);
        }

        public async Task<IActionResult> Modulos()
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                return View();
            }
            else
            { 
                return RedirectToAction("NoAutorizado", new { returnUrl = "/"+ControllerContext.ActionDescriptor.ControllerName+"/"+ ControllerContext.ActionDescriptor.ActionName });
            }
        }
        public IActionResult NoAutorizado(string returnUrl)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
            }
            TempData["returnUrl"] = returnUrl;
            return View();
        }
    }
}
