using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.UseCases.Home;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class GestorGestionaController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GestorGestionaController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Configuracion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> FiltrarTarjetas(string filtro, int pagina, int rpp)
        {
            var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
            var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var cards = JsonConvert.DeserializeObject<List<TarjetaModel>>(cardsText);
                        
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Título</th>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Imagen</th>";
            tabla += "<th>Tipo</th>";
            tabla += "<th>Ubicación de Imagen</th>";
            tabla += "<th>Ancho de Imagen</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = cards.Count % rpp != 0 ? (cards.Count / rpp) + 1 : cards.Count / rpp;
            if (cards.Count > 0)
            {
                foreach (var fila in cards)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Titulo + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Descripcion.Substring(0,25) + "</td>";
                    filasTabla += "<td style=\"width:150px;\"><img src=\"" + Path.Combine(_hostingEnvironment.WebRootPath, fila.Imagen) + "\" style =\"width:100%;height:auto\"/></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Tipo + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.LugarImg + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.AnchoImg + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorGestiona/DetalleTarjeta?IdTarjeta=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleTarjeta(int IdTarjeta)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
                var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var cards = JsonConvert.DeserializeObject<List<TarjetaModel>>(cardsText);

                var data = cards.Find(x => x.Id.Equals(IdTarjeta));
                var model = new GestorTablasMaestras.Gestiona.TarjetaModel();
                //var model = data;
                model.Id = data.Id;
                model.Orden = data.Orden;
                model.Titulo = data.Titulo;
                model.Descripcion = data.Descripcion;
                model.Tipo = data.Tipo;
                model.Imagen = data.Imagen;
                model.LugarImg = data.LugarImg;
                model.AnchoImg = data.AnchoImg;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTarjeta(TarjetaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
                var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var cards = JsonConvert.DeserializeObject<List<Home.TarjetaModel>>(cardsText);
                                
                var card = cards.FirstOrDefault(x => x.Id.Equals(model.Id));
                if(card != null)
                {
                    card.Orden = model.Orden;
                    card.Titulo = model.Titulo;
                    card.Descripcion = model.Descripcion;
                    card.Imagen = model.Imagen;
                    card.Tipo = model.Tipo;
                    card.LugarImg = model.LugarImg;
                    card.AnchoImg = model.AnchoImg;
                }

                string json = JsonConvert.SerializeObject(cards);
                System.IO.File.WriteAllText(cardsPath, json);

                return RedirectToAction("DetalleTarjeta", new { IdTarjeta = model.Id });
            }
        }
        public async Task<IActionResult> AgregarTarjeta()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;


                var configPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "configuraciones.json");
                var configText = System.IO.File.ReadAllText(configPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var config = JsonConvert.DeserializeObject<ConfiguracionesTarjeta>(configText);

                var tiposTarjeta = config.TiposTarjeta.Select(g => new SelectListItem() { Value = g.Nombre, Text = g.Nombre }).ToList();
                tiposTarjeta.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var posicionesImagen = config.PosicionesImagen.Select(g => new SelectListItem() { Value = g, Text = g }).ToList();
                posicionesImagen.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var anchosImagen = config.AnchosImagen.Select(g => new SelectListItem() { Value = g.Ancho.ToString(), Text = g.Porcentaje.ToString() }).ToList();
                anchosImagen.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Tipos = tiposTarjeta;
                ViewBag.Posiciones = posicionesImagen;
                ViewBag.Anchos = anchosImagen;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTarjeta(TarjetaModel model)
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
                    {
                        if (model.ImagenNueva != null && model.ImagenNueva.Length > 0)
                        {

                            var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
                            var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                            var cards = JsonConvert.DeserializeObject<List<Home.TarjetaModel>>(cardsText);

                            var cardsId = cards.Select(x => x.Id).ToList();
                            var nuevoId = 1;

                            while (cardsId.Contains(nuevoId))
                                nuevoId++;


                            var extension = Path.GetExtension(model.ImagenNueva.FileName);
                            var rutaNuevoArchivo = CargarImagen(model.ImagenNueva, nuevoId, extension).Result;

                            var card = new Home.TarjetaModel();
                            card.Id = nuevoId;
                            card.Orden = model.Orden;
                            card.Titulo = model.Titulo;
                            card.Descripcion = model.Descripcion;
                            card.Imagen = rutaNuevoArchivo;
                            card.Tipo = model.Tipo;
                            card.LugarImg = model.LugarImg;
                            card.AnchoImg = model.AnchoImg;

                            cards.Add(card);

                            string json = JsonConvert.SerializeObject(cards);
                            System.IO.File.WriteAllText(cardsPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                            return RedirectToAction("DetalleTarjeta", new { IdTarjeta = model.Id });
                        }
                    }
                    return RedirectToAction("AgregarTarjeta");
                }
            }
        }


        public async Task<string> CargarImagen(IFormFile archivo, int idTarjeta, string extension)
        {
            var nuevoNombre = CreateUniqueFileName(idTarjeta) + extension;

            var carpetaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "img", "uploads", "Home");
            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);
            }

            var rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "img", "uploads", "Home", nuevoNombre);

            using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(fileStream);

                return "\\" + Path.Combine("img", "uploads", "Home", nuevoNombre);
            }
        }
        private string CreateUniqueFileName(int idTarjeta) { 
            return  "Tarjeta_" + idTarjeta.ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
    }
}
