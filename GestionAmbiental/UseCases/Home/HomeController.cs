using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GestionAmbiental.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetRolUsuario _getRolUsuario;
        private IMemoryCache _cache;

        public HomeController(IGetRolUsuario getRolUsuario, IMemoryCache cache)
        {
            _getRolUsuario = getRolUsuario;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var rol = await _getRolUsuario.Execute(User.Identity.Name.Split("\\")[1]);
            if (!rol.HasValue)
                return RedirectToAction("NoAutorizado");
            else
            {
                var cache_value = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario);
                if (!cache_value.Equals(rol))
                {
                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromDays(1));

                    // Save data in cache.
                    _cache.Set(CacheKeys.RolUsuario, rol, cacheEntryOptions);
                }

                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();

                return View();
            }            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NoAutorizado()
        {
            return View();
        }
    }
}
