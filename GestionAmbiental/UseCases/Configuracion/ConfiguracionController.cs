using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.EditarConfiguracion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.ObtenerConfiguracion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Configuracion
{
    public class ConfiguracionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IObtenerConfiguracion _obtenerConfiguracion;
        private readonly IEditarConfiguracion _editarConfiguracion;

        public ConfiguracionController(IHostingEnvironment hostingEnvironment, IObtenerConfiguracion obtenerConfiguracion, IEditarConfiguracion editarConfiguracion)
        {
            _hostingEnvironment = hostingEnvironment;
            _obtenerConfiguracion = obtenerConfiguracion;
            _editarConfiguracion = editarConfiguracion;
        }

        public IActionResult ResumenModulos()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var propiedadesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propiedadesText = System.IO.File.ReadAllText(propiedadesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

                ViewBag.ListaPropiedades = propiedadesText;


                return View();
            }
        }
        public IActionResult ResumenPlataforma()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario; 
                if (userInfo.Rol.Equals(InspeccionRol.Admin))
                    return View();
                else
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });                
            }
        }
        [HttpPost]
        public IActionResult FiltrarSlides(string filtro, int rpp, int pagina)
        {
            var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
            var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var slidesJson = JsonConvert.DeserializeObject<List<Slide>>(slidesText);

            var filasTabla = "";
            var paginador = "";
            if (filtro !=null)
                slidesJson = slidesJson.Where(x => x.Titulo.Contains(filtro) || x.Contenido.Contains(filtro)).ToList();
            int nPaginas = slidesJson.Count % rpp != 0 ? (slidesJson.Count / rpp) + 1 : slidesJson.Count / rpp;
            if (slidesJson.Count > 0)
            {
                foreach (var fila in slidesJson)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Titulo + "</td>";
                    filasTabla += "<td>" + fila.Contenido + "</td>";
                    filasTabla += "<td><img style=\"width:350px;height:auto;\" src=\"" + fila.Imagen + "\"></td>";
                    filasTabla += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSlide(" + fila.Id + ")\">Eliminar</button></td>";
                    filasTabla += "<td><a class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" href=\"/Configuracion/DetalleSlide?Id=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "</tr>";

                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarSlides(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarSlides(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarSlides(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarSlides(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";
            }
            else
            {
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }       
        [HttpGet]
        public async Task<IActionResult> ConsultarSlides()
        {
            dynamic response;
            try
            {
                var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
                var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var slides = JsonConvert.DeserializeObject<List<SlideModel>>(slidesText);
                foreach(var item in slides)
                {
                    if (item.Titulo == null)
                        item.Titulo = " ";
                    if (item.Contenido == null)
                        item.Contenido = " ";
                }
                response = new
                {
                    success = true,
                    data = JsonConvert.SerializeObject(slides)
                };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        public IActionResult AgregarSlide()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (userInfo.Rol.Equals(InspeccionRol.Admin))
                    return View();
                else
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});                
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarSlide(SlideModel model)
        {
            dynamic response;
            try
            {
                if (!(HttpContext.Session.GetString("SessionUser") != null))
                {
                    response = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                    ViewBag.Rol = userInfo.Rol.ToString();
                    ViewBag.Usuario = userInfo.NombreUsuario;
                    if (userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        //MODIFICAR ARCHIVO JSON Y TEMPLATE JS
                        //Buscar archivo json con las propiedades
                        var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
                        var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        var slidesJson = JsonConvert.DeserializeObject<List<Slide>>(slidesText);
                        //Si la imagen cambio
                        if (model.ImagenNueva != null && model.ImagenNueva.Length > 0)
                        {
                            //Asignar nombre a la imagen y cargarla en wwwroot/img
                            var rutaImagenAnterior = model.Imagen;
                            var extension = Path.GetExtension(model.ImagenNueva.FileName);
                            var rutaNuevoArchivo = CargarImagen(model.ImagenNueva, "Slide", extension).Result;

                            model.Imagen = rutaNuevoArchivo.Replace("\\", "/");

                            //Eliminar imagen anterior
                            EliminarImagen(rutaImagenAnterior);
                        }
                        var slidesId = slidesJson.Select(x => x.Id).ToList();
                        var nuevoId = 1;

                        while (slidesId.Contains(nuevoId))
                            nuevoId++;
                        slidesJson.Add(new Slide
                        {
                            Id = nuevoId,
                            Titulo = model.Titulo,
                            Contenido = model.Contenido,
                            Imagen = model.Imagen
                        });

                        System.IO.File.WriteAllText(slidesPath, JsonConvert.SerializeObject(slidesJson), System.Text.Encoding.GetEncoding("iso-8859-1"));
                        response = new
                        {
                            success = true
                        };
                    }
                    else
                        response = new
                        {
                            success = false,
                            message = "No Autorizado"
                        };
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        public IActionResult DetalleSlide(int Id)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName +"?Id="+Id});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (userInfo.Rol.Equals(InspeccionRol.Admin))
                {
                    var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
                    var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    var slidesJson = JsonConvert.DeserializeObject<List<SlideModel>>(slidesText);
                    var model = slidesJson.Find(x => x.Id.Equals(Id));
                    return View(model);
                }
                else
                {
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Id=" + Id });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarSlide(SlideModel model)
        {
            dynamic response;
            try
            {
                if (!(HttpContext.Session.GetString("SessionUser") != null))
                {
                    response = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                    ViewBag.Rol = userInfo.Rol.ToString();
                    ViewBag.Usuario = userInfo.NombreUsuario;
                    if (userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        //MODIFICAR ARCHIVO JSON Y TEMPLATE JS
                        //Buscar archivo json con las propiedades
                        var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
                        var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        var slidesJson = JsonConvert.DeserializeObject<List<Slide>>(slidesText);
                        //Si la imagen cambio
                        if (model.ImagenNueva != null && model.ImagenNueva.Length > 0)
                        {
                            //Asignar nombre a la imagen y cargarla en wwwroot/img
                            var rutaImagenAnterior = model.Imagen;
                            var extension = Path.GetExtension(model.ImagenNueva.FileName);
                            var rutaNuevoArchivo = CargarImagen(model.ImagenNueva, "Slide", extension).Result;

                            model.Imagen = rutaNuevoArchivo.Replace("\\", "/");
                            //propiedad.img_url = model.img_url;

                            //Eliminar imagen anterior
                            EliminarImagen(rutaImagenAnterior);
                        }
                        var nuevo = slidesJson.Find(x => x.Id.Equals(model.Id));
                        nuevo.Imagen = model.Imagen;
                        nuevo.Titulo = model.Titulo;
                        nuevo.Contenido = model.Contenido;

                        System.IO.File.WriteAllText(slidesPath, JsonConvert.SerializeObject(slidesJson), System.Text.Encoding.GetEncoding("iso-8859-1"));
                        response = new
                        {
                            success = true
                        };
                    }
                    else
                    {
                        response = new
                        {
                            success = false,
                            message = "No Autorizado"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarSlide(int Id)
        {
            dynamic result;
            try
            {
                if (!(HttpContext.Session.GetString("SessionUser") != null))
                {
                    result = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                    if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        result = new
                        {
                            success = false,
                            message = "No Autorizado"
                        };
                    }
                    else
                    {
                        var slidesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "slides.json");
                        var slidesText = System.IO.File.ReadAllText(slidesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        var slidesJson = JsonConvert.DeserializeObject<List<Slide>>(slidesText);

                        var slide = slidesJson.FirstOrDefault(x => x.Id.Equals(Id));

                        if (slide != null)
                        {
                            var rutaImagenAnterior = slide.Imagen;
                            EliminarImagen(rutaImagenAnterior);

                            slidesJson.Remove(slide);

                            string json = JsonConvert.SerializeObject(slidesJson);
                            System.IO.File.WriteAllText(slidesPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                            //await _editarConfiguracion.Exectue("slides.json", slidesPath, json);
                            result = new
                            {
                                success = true
                            };
                        }
                        else
                        {
                            result = new
                            {
                                success = false,
                                message = "No se encontró el slide Id = " + Id.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = new
                {
                    success = false,
                    message = "Ocurrió un error: " + e.Message
                };
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> GuardarColores(string json)
        {
            dynamic response;
            try
            {
                if (!(HttpContext.Session.GetString("SessionUser") != null))
                {
                    response = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                    ViewBag.Rol = userInfo.Rol.ToString();
                    ViewBag.Usuario = userInfo.NombreUsuario;
                    if (userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        var propiedadesJsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                        //convertir a json y guardarlo en archivo json
                        var propiedadesJson = JsonConvert.DeserializeObject<List<PropiedadModulo>>(json);
                        string jsonToSave = JsonConvert.SerializeObject(propiedadesJson);
                        System.IO.File.WriteAllText(propiedadesJsonPath, jsonToSave, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        //listar propiedades
                        //Buscar archivo js con las propiedades
                        var templateJsPath = Path.Combine(_hostingEnvironment.WebRootPath, "js", "template_script.js");
                        var templateJsText = System.IO.File.ReadAllText(templateJsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        foreach (var propiedad in propiedadesJson)
                        {
                            //Modificar JS     

                            //Modificar colores (r-id,g-id,b-id) ej: (r2,g2,b2)
                            //primera letra + id.length + " = ".lenght
                            var rStartIndex = templateJsText.IndexOf("r" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
                            //index of (primer valor de la variable) + index of(;)
                            var rLastIndex = rStartIndex + templateJsText.Substring(rStartIndex, 10).IndexOf(';');
                            //reemplazar valores
                            var newJs = templateJsText.Substring(0, rStartIndex + 1) + propiedad.r + templateJsText.Substring(rLastIndex);
                            //primera letra + id.length + " = ".lenght
                            var gStartIndex = newJs.IndexOf("g" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
                            //index of (primer valor de la variable) + index of(;)
                            var gLastIndex = gStartIndex + newJs.Substring(gStartIndex, 10).IndexOf(';');
                            //reemplazar valores
                            newJs = newJs.Substring(0, gStartIndex + 1) + propiedad.g + newJs.Substring(gLastIndex);
                            //primera letra + id.length + " = ".lenght
                            var bStartIndex = newJs.IndexOf("b" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
                            //index of (primer valor de la variable) + index of(;)
                            var bLastIndex = bStartIndex + newJs.Substring(bStartIndex, 10).IndexOf(';');
                            //reemplazar valores
                            newJs = newJs.Substring(0, bStartIndex + 1) + propiedad.b + newJs.Substring(bLastIndex);
                            //convertir a json y guardarlo en archivo json
                            System.IO.File.WriteAllText(templateJsPath, newJs, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        }
                        response = new
                        {
                            success = true,
                            propiedades = jsonToSave
                        };
                    }
                    else
                    {
                        response = new
                        {
                            success = false,
                            message = "No Autorizado"
                        };
                    }
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        [HttpGet]
        public IActionResult ConsultarImagenModulos()
        {
            dynamic response;
            try
            {
                var imagenesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "imagenes_template.json");
                var imagenesText = System.IO.File.ReadAllText(imagenesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var imagenes = JsonConvert.DeserializeObject<List<ImagenTemplate>>(imagenesText);
                var imagenModulo = imagenes.Find(x => x.Nombre.Equals("menu_img_bg"));
                if (imagenModulo!= null)
                {

                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(imagenModulo)
                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = "No se encontró la imagen"
                    };
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        [HttpPost]
        public IActionResult CambiarImagenModulo(IFormFile ImagenNueva)
        {
            dynamic response;
            try
            {
                //Buscar json con las propiedades
                var imagenesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "imagenes_template.json");
                var imagenesText = System.IO.File.ReadAllText(imagenesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var imagenes = JsonConvert.DeserializeObject<List<ImagenTemplate>>(imagenesText);

                //Buscar archivo js con las propiedades
                var templateJsPath = Path.Combine(_hostingEnvironment.WebRootPath, "js", "template_script.js");
                var templateJsText = System.IO.File.ReadAllText(templateJsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

                var imagenModulo = imagenes.Find(x => x.Nombre.Equals("menu_img_bg"));
                if (imagenModulo!= null && ImagenNueva!=null && ImagenNueva.Length>0)
                {
                    var extension = Path.GetExtension(ImagenNueva.FileName);
                    var rutaNuevoArchivo = CargarImagen(ImagenNueva, "Modulos", extension).Result.Replace('\\','/');
                    EliminarImagen(imagenModulo.Imagen);

                    //Modificar ruta $('.menu_img_bg').css('background-image', 'url("/img/menu-bg.png")'); => $('.menu_img_bg').css('background-image', 'url("rutaNuevoArchivo")');
                    //primera letra + id.length + " = ".lenght
                    var textToFind = "$(\'.menu_img_bg\').css(\'background-image\', \'url(\"";
                    var startIndex = templateJsText.IndexOf(textToFind);
                    //index of ")');
                    var lastIndex = startIndex + templateJsText.Substring(startIndex, 250).IndexOf("\")\');");
                    if (startIndex > lastIndex)
                    {
                        response = new
                        {
                            success = false,
                            message = "Error al reemplazar JS"
                        };
                        return Ok(response);
                    }
                    var newJs = templateJsText.Substring(0, startIndex +textToFind.Length) + rutaNuevoArchivo + templateJsText.Substring(lastIndex);                    
                    //guardarlo en archivo js
                    System.IO.File.WriteAllText(templateJsPath, newJs, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    //guardar en json
                    imagenModulo.Imagen = rutaNuevoArchivo;
                    
                    System.IO.File.WriteAllText(imagenesPath, JsonConvert.SerializeObject(imagenes), System.Text.Encoding.GetEncoding("iso-8859-1"));

                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(imagenModulo)
                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = "No se encontró la imagen"
                    };
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        public async Task<string> CargarImagen(IFormFile archivo, string modulo, string extension)
        {
            var nuevoNombre = "Modulo_" + modulo + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + extension;

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
        public bool EliminarImagen(string ruta)
        {
            var archivo = _hostingEnvironment.WebRootPath + ruta;
            if (System.IO.File.Exists(archivo))
            {
                System.IO.File.Delete(archivo);
                return true;
            }
            else
            {
                return false;
            }
        }        
        public class Slide
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Contenido { get; set; }
            public string Imagen { get; set; }
        }
        public class PropiedadModulo
        {
            public int id { get; set; }
            public string modulo { get; set; }
            public int r { get; set; }
            public int g { get; set; }
            public int b { get; set; }
            public string img_url { get; set; }
        }
        public class ImagenTemplate
        {
            public string Nombre { get; set; }
            public string Imagen { get; set; }
        }
    }
}