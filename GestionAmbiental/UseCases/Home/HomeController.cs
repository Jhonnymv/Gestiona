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
                var configPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "configuraciones.json");
                var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");

                var configText = System.IO.File.ReadAllText(configPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

                var config = JsonConvert.DeserializeObject<ConfiguracionesTarjeta>(configText);
                var cards = JsonConvert.DeserializeObject<List<TarjetaModel>>(cardsText);

                model.Configuraciones = config;
                model.Tarjetas = cards;
            }
            catch(Exception e)
            {

            }
            //using (StreamReader r = new StreamReader(jsonConfigPath))
            //{
            //    string json = r.ReadToEnd();
            //    string items = JsonConvert.DeserializeObject<string>(json);
            //}



            var rol = await _getRolUsuario.Execute("kevin");

             var userInfo = new UserInfo { NombreUsuario = "kevin", Rol = rol.Value };
             HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(userInfo));

             ViewBag.Rol = userInfo.Rol.ToString();
             ViewBag.Usuario = userInfo.NombreUsuario;
            /*
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
            }
            */
            return View(model);
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
