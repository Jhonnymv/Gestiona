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
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.ObtenerConfiguracion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.EditarConfiguracion;
using GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona.Models;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona
{
    public class GestorContenidoController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IObtenerConfiguracion _obtenerConfiguracion;
        private readonly IEditarConfiguracion _editarConfiguracion;

        public GestorContenidoController(IHostingEnvironment hostingEnvironment, IObtenerConfiguracion obtenerConfiguracion, IEditarConfiguracion editarConfiguracion)
        {
            _hostingEnvironment = hostingEnvironment;
            _obtenerConfiguracion = obtenerConfiguracion;
            _editarConfiguracion = editarConfiguracion;
        }
        public async Task<IActionResult> PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Tablas"));
                if (dataModulo != null)
                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(dataModulo)
                    };
                else
                    response = new
                    {
                        success = false,
                        message = "No se encontraron las propiedades del módulo"
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
        //public async Task<IActionResult> Resumen()
        //{
        //    if (!(HttpContext.Session.GetString("SessionUser") != null))
        //    {
        //        return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
        //    }
        //    else
        //    {
        //        var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

        //        ViewBag.Rol = userInfo.Rol.ToString();
        //        ViewBag.Usuario = userInfo.NombreUsuario;

        //        var propiedadesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
        //        var propiedadesText = System.IO.File.ReadAllText(propiedadesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

        //        ViewBag.ListaPropiedades = propiedadesText;


        //        return View();
        //    }
        //} 
        public async Task<IActionResult> ResumenTarjetas()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        //Propiedadaes módulos
        //[HttpPost]
        //public async Task<IActionResult> GuardarColores(string json)
        //{
        //    dynamic response;
        //    try
        //    {
        //        var propiedadesJsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
        //        //convertir a json y guardarlo en archivo json
        //        var propiedadesJson = JsonConvert.DeserializeObject<List<PropiedadModulo>>(json);
        //        string jsonToSave = JsonConvert.SerializeObject(propiedadesJson);
        //        System.IO.File.WriteAllText(propiedadesJsonPath, jsonToSave, System.Text.Encoding.GetEncoding("iso-8859-1"));
        //        //listar propiedades
        //        //Buscar archivo js con las propiedades
        //        var templateJsPath = Path.Combine(_hostingEnvironment.WebRootPath, "js", "template_script.js");
        //        var templateJsText = System.IO.File.ReadAllText(templateJsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
        //        foreach (var propiedad in propiedadesJson)
        //        {
        //            //Modificar JS     

        //            //Modificar colores (r-id,g-id,b-id) ej: (r2,g2,b2)
        //            //primera letra + id.length + " = ".lenght
        //            var rStartIndex = templateJsText.IndexOf("r" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
        //            //index of (primer valor de la variable) + index of(;)
        //            var rLastIndex = rStartIndex + templateJsText.Substring(rStartIndex, 10).IndexOf(';');
        //            //reemplazar valores
        //            var newJs = templateJsText.Substring(0, rStartIndex + 1) + propiedad.r + templateJsText.Substring(rLastIndex);
        //            //primera letra + id.length + " = ".lenght
        //            var gStartIndex = newJs.IndexOf("g" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
        //            //index of (primer valor de la variable) + index of(;)
        //            var gLastIndex = gStartIndex + newJs.Substring(gStartIndex, 10).IndexOf(';');
        //            //reemplazar valores
        //            newJs = newJs.Substring(0, gStartIndex + 1) + propiedad.g + newJs.Substring(gLastIndex);
        //            //primera letra + id.length + " = ".lenght
        //            var bStartIndex = newJs.IndexOf("b" + propiedad.id + " = ") + (propiedad.id.ToString().Length + 3);
        //            //index of (primer valor de la variable) + index of(;)
        //            var bLastIndex = bStartIndex + newJs.Substring(bStartIndex, 10).IndexOf(';');
        //            //reemplazar valores
        //            newJs = newJs.Substring(0, bStartIndex + 1) + propiedad.b + newJs.Substring(bLastIndex);
        //            //convertir a json y guardarlo en archivo json
        //            System.IO.File.WriteAllText(templateJsPath, newJs, System.Text.Encoding.GetEncoding("iso-8859-1"));
        //        }
        //        response = new
        //        {
        //            success = true,
        //            propiedades = jsonToSave
        //        };
        //    }
        //    catch(Exception e)
        //    {
        //        response = new
        //        {
        //            success = false,
        //            message = e.Message
        //        };
        //    }
        //    return Ok(response);
        //}
        [HttpPost]
        public async Task<IActionResult> FiltrarPropiedades(string filtro, int pagina, int rpp)
        {
            var propiedadesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
            var propiedadesText = System.IO.File.ReadAllText(propiedadesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var propiedades = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propiedadesText);
                        
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Módulo</th>";
            tabla += "<th>Color</th>";
            tabla += "<th>Imagen Fondo</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = propiedades.Count % rpp != 0 ? (propiedades.Count / rpp) + 1 : propiedades.Count / rpp;
            if (propiedades.Count > 0)
            {
                if(filtro!=null)
                    propiedades = propiedades.Where(x=>x.modulo.ToLower().Contains(filtro.ToLower())).ToList();

                foreach (var fila in propiedades)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.modulo + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><div class=\"shadow\" style=\"height:30px; width:auto; background-color:rgb(" + fila.r +","+ +fila.g + "," + +fila.b+");\"></div></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><img src=\""+fila.img_url+ "\" style=\"height:50px; width:auto;\"></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorContenido/DetallePropiedades?Modulo=" + fila.modulo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                var nCol = tabla.Split("</th>").Count()-1;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\""+ nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
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
        public async Task<IActionResult> DetallePropiedades(string Modulo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Modulo=" + Modulo });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var propiedadesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propiedadesText = System.IO.File.ReadAllText(propiedadesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var propiedades = JsonConvert.DeserializeObject<List<PropiedadModulo>>(propiedadesText);
                var propiedad = propiedades.Find(x => x.modulo.Equals(Modulo));

                var model = new PropiedadModulos();
                //var model = data;
                model.id = propiedad.id;
                model.modulo = propiedad.modulo;
                model.r = propiedad.r;
                model.g = propiedad.g;
                model.b = propiedad.b;
                model.img_url = propiedad.img_url;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarPropiedad(ModelEditarPropiedades model)
        {
            dynamic response;
            try
            {
                //MODIFICAR ARCHIVO JSON Y TEMPLATE JS
                //Buscar archivo json con las propiedades
                var propiedadesJsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var templateJsPath = Path.Combine(_hostingEnvironment.WebRootPath, "js", "template_script.js");
                var propiedadesJsonNuevo = JsonConvert.DeserializeObject<List<PropiedadModulo>>(model.Json);

                for (int i = 0; i < propiedadesJsonNuevo.Count; i++)
                {
                    var propiedadesJsonText = System.IO.File.ReadAllText(propiedadesJsonPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    var propiedadesJson = JsonConvert.DeserializeObject<List<PropiedadModulo>>(propiedadesJsonText);
                    //Buscar archivo js con las propiedades
                    var templateJsText = System.IO.File.ReadAllText(templateJsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                   
                    if (propiedadesJson[i].id == propiedadesJsonNuevo[i].id)
                    {
                        //Modifiar los datos de la propiedad por los traidos del modelo
                        propiedadesJson[i].id = propiedadesJsonNuevo[i].id;
                        propiedadesJson[i].modulo = propiedadesJsonNuevo[i].modulo;
                        propiedadesJson[i].r = propiedadesJsonNuevo[i].r;
                        propiedadesJson[i].g = propiedadesJsonNuevo[i].g;
                        propiedadesJson[i].b = propiedadesJsonNuevo[i].b;
                        //Modificar colores (r-id,g-id,b-id) ej: (r2,g2,b2)
                        //primera letra + id.length + " = ".lenght
                        var rStartIndex = templateJsText.IndexOf("r" + propiedadesJson[i].id + " = ") + (propiedadesJson[i].id.ToString().Length + 3);
                        //index of (primer valor de la variable) + index of(;)
                        var rLastIndex = rStartIndex + templateJsText.Substring(rStartIndex, 10).IndexOf(';');
                        //reemplazar valores
                        var newJs = templateJsText.Substring(0, rStartIndex + 1) + propiedadesJsonNuevo[i].r + templateJsText.Substring(rLastIndex);

                        //primera letra + id.length + " = ".lenght
                        var gStartIndex = newJs.IndexOf("g" + propiedadesJson[i].id + " = ") + (propiedadesJson[i].id.ToString().Length + 3);
                        //index of (primer valor de la variable) + index of(;)
                        var gLastIndex = gStartIndex + newJs.Substring(gStartIndex, 10).IndexOf(';');
                        //reemplazar valores
                        newJs = newJs.Substring(0, gStartIndex + 1) + propiedadesJsonNuevo[i].g + newJs.Substring(gLastIndex);

                        //primera letra + id.length + " = ".lenght
                        var bStartIndex = newJs.IndexOf("b" + propiedadesJson[i].id + " = ") + (propiedadesJson[i].id.ToString().Length + 3);
                        //index of (primer valor de la variable) + index of(;)
                        var bLastIndex = bStartIndex + newJs.Substring(bStartIndex, 10).IndexOf(';');
                        //reemplazar valores
                        newJs = newJs.Substring(0, bStartIndex + 1) + propiedadesJsonNuevo[i].b + newJs.Substring(bLastIndex);

                        IFormFile nuevaImagen;
                        switch (propiedadesJson[i].modulo)
                        {
                            case "Inspection": nuevaImagen = model.InspectionFile; break;
                            case "RAC": nuevaImagen = model.RacFile; break;
                            case "Residuos": nuevaImagen = model.ResiduosFile; break;
                            case "Incidentes": nuevaImagen = model.IncidentesFile; break;
                            case "Fauna": nuevaImagen = model.FaunaFile; break;
                            case "Agua": nuevaImagen = model.AguaFile; break;
                            case "SGA": nuevaImagen = model.SGAFile; break;
                            case "Tablas": nuevaImagen = model.TablasFile; break;
                            default: nuevaImagen = null; break;
                        }
                        //Si la imagen cambio
                        if (nuevaImagen != null && nuevaImagen.Length > 0)
                        {
                            //Asignar nombre a la imagen y cargarla en wwwroot/img
                            var rutaImagenAnterior = propiedadesJson[i].img_url;
                            var extension = Path.GetExtension(nuevaImagen.FileName);
                            var rutaNuevoArchivo = CargarImagen(nuevaImagen, propiedadesJson[i].modulo, extension).Result;

                            propiedadesJsonNuevo[i].img_url = rutaNuevoArchivo.Replace("\\", "/");
                            //propiedad.img_url = model.img_url;

                            //Eliminar imagen anterior
                            //EliminarImagen(rutaImagenAnterior);

                            //Modificar imagen $('.modulo .image-header-container').css('background-image', 'url("nueva-url")') ej: $('.inspection .image-header-container').css('background-image', 'url("/img/back-inspection.png")');
                            var imgTextToFind = "$(\'." + propiedadesJson[i].modulo.ToLower() + " .image-header-container\').css('background-image', 'url(\"";
                            var imgStartIndex = newJs.IndexOf(imgTextToFind) + imgTextToFind.Length;
                            var imgLastIndex = imgStartIndex + newJs.Substring(imgStartIndex, 150).IndexOf('\"');
                            newJs = newJs.Substring(0, imgStartIndex) + propiedadesJsonNuevo[i].img_url + newJs.Substring(imgLastIndex);

                            //Modificar imagen $('.modal-menu-modulo .modal-content .modal-image-header-container').css('background-image', 'url("nueva-url")'); ej: $('.modal-menu-inspection .modal-content .modal-image-header-container').css('background-image', 'url("/img/back-inspection.png")');
                            var img2TextToFind = "$(\'.modal-menu-" + propiedadesJsonNuevo[i].modulo.ToLower() + " .modal-content .modal-image-header-container').css('background-image', 'url(\"";
                            var img2StartIndex = newJs.IndexOf(img2TextToFind) + img2TextToFind.Length;
                            var img2LastIndex = img2StartIndex + newJs.Substring(img2StartIndex, 150).IndexOf('\"');
                            newJs = newJs.Substring(0, img2StartIndex) + propiedadesJsonNuevo[i].img_url + newJs.Substring(img2LastIndex);
                        }
                        //Convertir las propiedades al formato que guarda el json
                        var toJson = propiedadesJsonNuevo.Select(x => new PropiedadModulo
                        {
                            id = x.id,
                            modulo = x.modulo,
                            r = x.r,
                            g = x.g,
                            b = x.b,
                            img_url = x.img_url
                        }).ToList();

                        //convertir a json y guardarlo en archivo json
                        string json = JsonConvert.SerializeObject(toJson);
                        System.IO.File.WriteAllText(propiedadesJsonPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                        //guardar nuevo js
                        System.IO.File.WriteAllText(templateJsPath, newJs, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    }
                }
                response = new
                {
                    success = true,
                    propiedades = JsonConvert.SerializeObject(propiedadesJsonNuevo)
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
        //Tarjetas
        [HttpPost]
        public async Task<IActionResult> FiltrarTarjetas(string filtro, int pagina, int rpp)
        {
            var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
            var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var cards = JsonConvert.DeserializeObject<List<Home.TarjetaModel>>(cardsText);
                        
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Título</th>";
            tabla += "<th>Contenido</th>";
            tabla += "<th>Imagen</th>";
            tabla += "<th>Fecha</th>";
            tabla += "<th>Publicador</th>";
            tabla += "<th></th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = cards.Count % rpp != 0 ? (cards.Count / rpp) + 1 : cards.Count / rpp;
            if (cards.Count > 0)
            {
                if(filtro!=null)
                    cards = cards.Where(x=>x.Titulo.ToLower().Contains(filtro.ToLower()) || x.Contenido.ToLower().Contains(filtro.ToLower())).ToList();

                cards = cards.OrderBy(x => x.Fecha).ToList();
                foreach (var fila in cards)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Titulo + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + (fila.Contenido.Length > 250 ? fila.Contenido.Substring(0, 250) + "..." : fila.Contenido) + "</td>";
                    filasTabla += "<td style=\"width:150px;\"><img src=\"" + Path.Combine(_hostingEnvironment.WebRootPath, fila.Imagen) + "\" style =\"width:100%;height:auto\"/></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Fecha + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Publicador + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorContenido/DetalleTarjeta?IdTarjeta=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><button type=\"button\" class=\"btn btn-danger\" onclick=\"EliminarTarjeta("+fila.Id+")\">Eliminar</button></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTarjetas(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                var nCol = tabla.Split("</th>").Count()-1;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\""+ nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTarjeta=" + IdTarjeta });
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
                
                model.Id = data.Id;
                model.Titulo = data.Titulo;
                model.Contenido = data.Contenido;
                model.Imagen = data.Imagen;
                model.Publicador = data.Publicador;
                model.Fecha = data.Fecha;
                model.Tipo = data.Tipo;
                model.Tamanio = data.Tamanio;
                model.PosicionE = data.PosicionE;
                model.PosicionM = data.PosicionM;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTarjeta(TarjetaModel model)
        {
            dynamic result;
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
                    result = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                else
                {

                    if (model.Titulo != null && model.Fecha != null && model.Contenido != null && model.Tipo != null)
                    {
                        var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
                        var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        var cards = JsonConvert.DeserializeObject<List<Home.TarjetaModel>>(cardsText);

                        var card = cards.FirstOrDefault(x => x.Id.Equals(model.Id));

                        if (card != null)
                        {
                            if (model.ImagenNueva != null && model.ImagenNueva.Length > 0)
                            {
                                var rutaImagenAnterior = card.Imagen;
                                var extension = Path.GetExtension(model.ImagenNueva.FileName);
                                var rutaNuevoArchivo = CargarImagen(model.ImagenNueva, model.Id, extension).Result;

                                card.Imagen = rutaNuevoArchivo;

                                EliminarImagen(rutaImagenAnterior);
                            }
                            card.Id = model.Id;
                            card.Titulo = model.Titulo;
                            card.Contenido = model.Contenido;
                            card.Fecha = model.Fecha;
                            card.Publicador = model.Publicador;
                            card.Tipo = model.Tipo;
                            card.Tamanio = model.Tamanio;
                            card.PosicionE = model.PosicionE;
                            card.PosicionM = model.PosicionM;
                        }
                        string json = JsonConvert.SerializeObject(cards);
                        System.IO.File.WriteAllText(cardsPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                        await _editarConfiguracion.Exectue("tarjetas.json", cardsPath, json);

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
                            message = "No Autorizado"
                        };
                    }
                }
            }
            return Ok(result);
        }
        public async Task<IActionResult> AgregarTarjeta()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
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
        public async Task<IActionResult> AgregarTarjeta(TarjetaModel model)
        {
            dynamic result;
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                result = new
                {
                    success = false,
                    message = "No Autorizado"
                };
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    result = new
                    {
                        success = false,
                        message = "No Autorizado"
                    };
                else
                {
                    if (model.Titulo != null && model.ImagenNueva != null && model.ImagenNueva.Length > 0 && model.Fecha != null && model.Contenido != null && model.Tipo != null)
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
                        card.Titulo = model.Titulo;
                        card.Contenido = model.Contenido;
                        card.Imagen = rutaNuevoArchivo;
                        card.Tipo = model.Tipo;
                        card.Fecha = model.Fecha;
                        card.Publicador = model.Publicador;
                        card.Tamanio = model.Tamanio;
                        card.PosicionE = model.PosicionE;
                        card.PosicionM = model.PosicionM;

                        cards.Add(card);

                        string json = JsonConvert.SerializeObject(cards);
                        System.IO.File.WriteAllText(cardsPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                        await _editarConfiguracion.Exectue("tarjetas.json", cardsPath, json);

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
                            message = "Datos Incompletos"
                        };
                    }
                }
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarTarjeta(int IdTarjeta)
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
                        var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
                        var cardsText = System.IO.File.ReadAllText(cardsPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        var cards = JsonConvert.DeserializeObject<List<Home.TarjetaModel>>(cardsText);

                        var card = cards.FirstOrDefault(x => x.Id.Equals(IdTarjeta));

                        if (card != null)
                        {
                            var rutaImagenAnterior = card.Imagen;
                            EliminarImagen(rutaImagenAnterior);

                            cards.Remove(card);

                            string json = JsonConvert.SerializeObject(cards);
                            System.IO.File.WriteAllText(cardsPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));

                            await _editarConfiguracion.Exectue("tarjetas.json", cardsPath, json);
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
                                message = "No se encontró la tarjeta Id = " + IdTarjeta.ToString()
                            };
                        }
                    }
                }
            }catch(Exception e)
            {
                result = new
                {
                    success = false,
                    message = "Ocurrió un error: " + e.Message
                };
            }
            return Ok(result);
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
        private string CreateUniqueFileName(int idTarjeta) { 
            return  "Tarjeta_" + idTarjeta.ToString() + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
        //Colores
        public async Task<IActionResult> FiltrarColores(string filtro, int pagina, int rpp)
        {
            var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
            var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Código Color</th>";
            tabla += "<th>Color</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = variables.Colores.Count % rpp != 0 ? (variables.Colores.Count / rpp) + 1 : variables.Colores.Count / rpp;
            if (variables.Colores.Count > 0)
            {
                if (filtro != null)
                    variables.Colores = variables.Colores.Where(x => x.Variable.ToLower().Contains(filtro.ToLower()) || x.Descripcion.ToLower().Contains(filtro.ToLower())).ToList();

                variables.Colores = variables.Colores.OrderBy(x => x.Variable).ToList();

                foreach (var fila in variables.Colores)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Descripcion + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Valor + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><div class=\"shadow\" style=\"height:30px; width:auto; background-color:" + fila.Valor + ";\"></div></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorContenido/DetalleColor?Id=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarColores(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarColores(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarColores(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarColores(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                var nCol = tabla.Split("</th>").Count() - 1;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
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
        public async Task<IActionResult> DetalleColor(int Id,string message)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Id=" + Id+ "&message="+ message });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                
                var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
                var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

                var data = variables.Colores.Find(x => x.Id.Equals(Id));
                var model = new GestorTablasMaestras.Gestiona.ColoresModel();
                model = data;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarColor(ColoresModel model)
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

                if (ModelState.IsValid)
                {
                    var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
                    var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

                    var variable = variables.Colores.FirstOrDefault(x => x.Id.Equals(model.Id));

                    if (variable != null)
                    {
                        try
                        {
                            variable.Id = model.Id;
                            variable.Descripcion = model.Descripcion;
                            variable.Variable = model.Variable;
                            variable.Valor = model.Valor;
                            //Actualizar json
                            string json = JsonConvert.SerializeObject(variables);
                            System.IO.File.WriteAllText(variablesPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));
                            //Actualizar css
                            ActualizarColoresCSS();
                            
                            await _editarConfiguracion.Exectue("variables-estilo.json", variablesPath, json);
                        }
                        catch(Exception e)
                        {
                            return RedirectToAction("DetalleColor", new { Id = model.Id, message= "No se pudo cambiar el color: "+e.Message });
                        }
                    }
                }
                return RedirectToAction("DetalleColor", new { Id = model.Id });
            }
        }
        public bool ActualizarColoresCSS()
        {
            //buscar json actualizado
            var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
            var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, "css", "site.css");
            var cssText = System.IO.File.ReadAllText(cssPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

            //root es el primero que contiene { }
            //buscar los índices de las primeras { }
            var firstPtsIndex = cssText.IndexOf('{');
            var lastPtsIndex = cssText.IndexOf('}');
            try
            {
                //buscar texto root contenido
                var rootText = cssText.Substring(firstPtsIndex, (lastPtsIndex - firstPtsIndex + 1));
         
                //actualizar valores de cada variable
                foreach(var variable in variables.Colores)
                {
                    if (rootText.Contains(variable.Variable))
                    {
                        //Buscar indice donde termina la variable (luego de cada valor hay un ;)
                        int variableIndex = rootText.IndexOf(variable.Variable);
                        //Substring temporal para hallar ; que determina hasta donde es el índice del valor de la variable
                        int first = rootText.Substring(variableIndex).IndexOf(':') + variableIndex;
                        int last = rootText.Substring(variableIndex).IndexOf(';') + variableIndex;
                        //Texto a reemplazar
                        string replaced = rootText.Substring(0, first+1) + variable.Valor + rootText.Substring(last);
                        //var toBeReplaced = rootText.Substring(first+1, (last - first)-1);
                        rootText = replaced;
                    }
                }
                var toBeReplaced2 = cssText.Substring(firstPtsIndex, (lastPtsIndex - firstPtsIndex)-1);
                string newCSS = cssText.Substring(0, firstPtsIndex) + rootText + cssText.Substring(lastPtsIndex+1);
                System.IO.File.WriteAllText(cssPath, newCSS, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
            catch (Exception e)
            { 
            }
            
            return true;
        }
        //Fuentes
        public async Task<IActionResult> FiltrarFuentes(string filtro, int pagina, int rpp)
        {
            var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
            var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Fuentes</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = variables.Colores.Count % rpp != 0 ? (variables.Colores.Count / rpp) + 1 : variables.Colores.Count / rpp;
            if (variables.Fuentes.Count > 0)
            {
                if (filtro != null)
                    variables.Fuentes = variables.Fuentes.Where(x =>x.Descripcion.ToLower().Contains(filtro.ToLower())).ToList();
                variables.Fuentes = variables.Fuentes.OrderBy(x => x.Id).ToList();
                foreach (var fila in variables.Fuentes)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><h4 style=\"font-family:\'"+ fila.Descripcion + "\'\">" + fila.Descripcion + "</h4></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorContenido/DetalleFuente?Id=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarFuentes(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarFuentes(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarFuentes(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarFuentes(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";
            }
            else
            {
                var nCol = tabla.Split("</th>").Count() - 1;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
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
        public async Task<IActionResult> DetalleFuente(int Id, string message)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Id=" + Id + "&message=" + message });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;          
                ViewBag.Mensaje = message;          

                var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
                var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

                var data = variables.Fuentes.Find(x => x.Id.Equals(Id));
                var model = new GestorTablasMaestras.Gestiona.FuentesTextoModel();
                model = data;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarFuente(FuentesTextoModel model)
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

                if (ModelState.IsValid)
                {
                    var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
                    var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                    var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

                    var variable = variables.Fuentes.FirstOrDefault(x => x.Id.Equals(model.Id));

                    if (variable != null)
                    {
                        try
                        {
                            variable.Id = model.Id;
                            variable.Descripcion = model.Descripcion;
                            //Actualizar json
                            string json = JsonConvert.SerializeObject(variables);
                            System.IO.File.WriteAllText(variablesPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));
                            //Actualizar css
                            ActualizarFuentesCSS();

                            await _editarConfiguracion.Exectue("variables-estilo.json", variablesPath, json);
                        }
                        catch (Exception e)
                        {
                            return RedirectToAction("DetalleFuente", new { Id = model.Id, message = "No se pudo cambiar el color: " + e.Message });
                        }
                    }
                }
                return RedirectToAction("DetalleFuente", new { Id = model.Id });
            }
        }
        public async Task<IActionResult> ObtenerFuentes()
        {
            var fuentesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "fuentes-texto.json");
            var fuentesText = System.IO.File.ReadAllText(fuentesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var fuentes = JsonConvert.DeserializeObject<List<string>>(fuentesText);

            var html = "";
            foreach(var item in fuentes)
            {
                html+="<option value=\""+item+ "\" style=\"font-family: \'"+item+"\';\">" + item + "</option>";
            }
            dynamic response = new
            {
                result = "Ok",
                fuentes = html
            };

            return Ok(response);
        }
        public bool ActualizarFuentesCSS()
        {
            //buscar json actualizado
            var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
            var variablesText = System.IO.File.ReadAllText(variablesPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
            var variables = JsonConvert.DeserializeObject<VariablesEstiloModel>(variablesText);

            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, "css", "site.css");
            var cssText = System.IO.File.ReadAllText(cssPath, System.Text.Encoding.GetEncoding("iso-8859-1"));

            //buscar los índices de body { }
            var firstPtsIndex = cssText.IndexOf("body");
            var colonsIndex = cssText.Substring(firstPtsIndex).IndexOf(':') + firstPtsIndex;
            var lastPtsIndex = cssText.Substring(firstPtsIndex).IndexOf('}')+ firstPtsIndex;
            try
            {
                //buscar texto font family contenido
                var fontText = cssText.Substring(firstPtsIndex, (lastPtsIndex - firstPtsIndex + 1));
                var newFonts = "";
                //actualizar valores de cada variable
                newFonts += "\'" + variables.Fuentes[0].Descripcion + "\',";
                newFonts += "\'" + variables.Fuentes[1].Descripcion + "\',";
                newFonts += "\'" + variables.Fuentes[2].Descripcion + "\';";

                string newCSS = cssText.Substring(0, colonsIndex+1) + newFonts + cssText.Substring(lastPtsIndex);
                System.IO.File.WriteAllText(cssPath, newCSS, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
            catch (Exception e)
            {
            }

            return true;
        }
        public async Task ActualizarColoresFuentes()
        {
            var variablesPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "variables-estilo.json");
            var variables = await _obtenerConfiguracion.Execute("variables-estilo.json");
            var json = variables.Texto;
            System.IO.File.WriteAllText(variablesPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));
            
        }
        public async Task ActualizarTarjetas()
        {
            var cardsPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "tarjetas.json");
            var cards = await _obtenerConfiguracion.Execute("tarjetas.json");
            var json = cards.Texto;
            System.IO.File.WriteAllText(cardsPath, json, System.Text.Encoding.GetEncoding("iso-8859-1"));
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
    }
}
