using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.Items;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;
using ImageMagick;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;
using Serilog;

namespace GestionAmbiental.UseCases.MonitoreoCampoInterno.AvistamientoFlora
{
    public class AvistamientoFloraController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.Items.ISelectItems _selectItems;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.NuevoRegistro.INuevoRegistro _nuevoRegistro;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.ListarRegistros.IListarRegistros _listarRegistrosAvistamientoFlora;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroNombreAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroFenologiaAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroVigorAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroSaludAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroUsoAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.NuevoRegistro.INuevoRegistro _nuevoRegistroFaunaAsociadaAvistamiento;

        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.ListarRegistros.IListarRegistros _listarNombresCientificosByAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.ListarRegistros.IListarRegistros _listarFenologiasByAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.ListarRegistros.IListarRegistros _listarVigorByAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.ListarRegistros.IListarRegistros _listarSaludByAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.ListarRegistros.IListarRegistros _listarUsoByAvistamiento;
        private readonly Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.ListarRegistros.IListarRegistros _listarFaunaByAvistamiento;

        public AvistamientoFloraController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IGetInfo_SGA info_SGA,
            IWebHostEnvironment hostingEnvironment,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.Items.ISelectItems selectItems,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.NuevoRegistro.INuevoRegistro nuevoRegistro,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.ListarRegistros.IListarRegistros listarRegistrosAvistamientoFlora,


            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroNombreAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroFenologiaAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroVigorAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroSaludAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroUsoAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.NuevoRegistro.INuevoRegistro nuevoRegistroFaunaAsociadaAvistamiento,


            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento.ListarRegistros.IListarRegistros listarNombresCientificosByAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento.ListarRegistros.IListarRegistros listarFenologiasByAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento.ListarRegistros.IListarRegistros listarVigorByAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento.ListarRegistros.IListarRegistros listarSaludByAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento.ListarRegistros.IListarRegistros listarUsoByAvistamiento,
            Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento.ListarRegistros.IListarRegistros listarFaunaByAvistamiento
            )
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _info_SGA = info_SGA;
            _hostingEnvironment = hostingEnvironment;
            _selectItems = selectItems;
            _nuevoRegistro = nuevoRegistro;
            _listarRegistrosAvistamientoFlora = listarRegistrosAvistamientoFlora;


            _nuevoRegistroNombreAvistamiento = nuevoRegistroNombreAvistamiento;
            _nuevoRegistroFenologiaAvistamiento = nuevoRegistroFenologiaAvistamiento;
            _nuevoRegistroVigorAvistamiento = nuevoRegistroVigorAvistamiento;
            _nuevoRegistroSaludAvistamiento = nuevoRegistroSaludAvistamiento;
            _nuevoRegistroUsoAvistamiento = nuevoRegistroUsoAvistamiento;
            _nuevoRegistroFaunaAsociadaAvistamiento = nuevoRegistroFaunaAsociadaAvistamiento;

            _listarNombresCientificosByAvistamiento = listarNombresCientificosByAvistamiento;
            _listarFenologiasByAvistamiento = listarFenologiasByAvistamiento;
            _listarVigorByAvistamiento = listarVigorByAvistamiento;
            _listarSaludByAvistamiento = listarSaludByAvistamiento;
            _listarUsoByAvistamiento = listarUsoByAvistamiento;
            _listarFaunaByAvistamiento = listarFaunaByAvistamiento;
        }

        public async Task<IActionResult> Resumen()
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

                return View();
            }
        }
        public async Task<IActionResult> NuevoRegistro()
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
                var items = await _selectItems.Execute();

                var zonsaItems = items.ItemsZona.OrderBy(x => x.Nombre).Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Nombre }).ToList();
                zonsaItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var NombresCinetificos = items.ItemsNombreCientifico.ToList();
                var Fenologias = items.ItemsFenologia.ToList();
                var vigores = items.ItemsVigor.ToList();
                var saluds = items.ItemsSalud.ToList();
                var usos = items.ItemsUso.ToList();
                var fauna = items.ItemsFaunaAsociada.ToList();

                ViewBag.Zonas = zonsaItems;
                //ViewBag.nombresCientificos = NombresCinetificosItems;
                ViewBag.nombres = NombresCinetificos;
                ViewBag.fenologias = Fenologias;
                ViewBag.vigores = vigores;
                ViewBag.salud = saluds;
                //ViewBag.vigores = VigoresItems;
                //ViewBag.saluds = SaludItems;
                ViewBag.usos = usos;
                ViewBag.faunAsoc = fauna;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(AvistamientoFloraModel model)
        {
            var fotos = new List<string>();
            //for (int i = 0; i < model.Fotos.Count; i++)
            //{
            //    var filePath = "";

            //    var extension = Path.GetExtension(model.Fotos[i].FileName);
            //    var max_file_size = 8388608;

            //    var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

            //    if (allowed_extensions.Contains(extension) && model.Fotos[i].Length < max_file_size)
            //    {
            //        var newfileName = CreateUniqueFileName(i + 1) + extension;
            //        filePath = await CargarImagen(model.Fotos[i], newfileName);
            //        fotos.Add(filePath);
            //    }
            //}
            var IdAvistamiento = await _nuevoRegistro.Execute(model.Idficha,
                Convert.ToDateTime(model.Fechavalidación),
                model.IdZona,
                model.Ubicacion_Este.Split(",")[0],
                model.Ubicacion_Este.Split(",")[1],
                model.Ubicacion_Altitud,
                model.Geneto,
                model.Rameto,
                model.CondicionReubicacion,
                model.Sustrato,
               Convert.ToDecimal(model.CoberturaDiametroMayor),
               Convert.ToDecimal(model.CoberturaDiametroMenor),
               Convert.ToDecimal(model.CoberturaDiametroAltura),
               fotos.ToArray()
                );
            for (int i = 0; i < model.NombreCantidad.Count; i++)
            {
                if (model.NombreCantidad[i] > 0)
                {
                    await _nuevoRegistroNombreAvistamiento.Execute(
                        IdAvistamiento,
                        model.NombreId[i],
                        model.NombreCantidad[i],
                        "prueba"
                        );
                }
            }
            for (int i = 0; i < model.FenologiaCantidad.Count; i++)
            {
                if (model.FenologiaCantidad[i] > 0)
                {
                    await _nuevoRegistroFenologiaAvistamiento.Execute(
                        IdAvistamiento,
                        model.FenologiaId[i],
                        model.FenologiaCantidad[i],
                        "prueba"
                        );
                }
            }
            for (int i = 0; i < model.VigorCantidad.Count; i++)
            {
                if (model.VigorCantidad[i] > 0)
                {
                    await _nuevoRegistroVigorAvistamiento.Execute(
                        IdAvistamiento,
                        model.VigorId[i],
                        model.VigorCantidad[i],
                        "prueba"
                        );
                }
            }
            for (int i = 0; i < model.SaludCantidad.Count; i++)
            {
                if (model.SaludCantidad[i] > 0)
                {
                    await _nuevoRegistroSaludAvistamiento.Execute(
                        IdAvistamiento,
                        model.SaludId[i],
                        model.SaludCantidad[i],
                        "prueba"
                        );
                }
            }
            for (int i = 0; i < model.UsoCantidad.Count; i++)
            {
                if (model.UsoCantidad[i] > 0)
                {
                    await _nuevoRegistroUsoAvistamiento.Execute(
                        IdAvistamiento,
                        model.UsoId[i],
                        model.UsoCantidad[i],
                        "prueba"
                        );
                }
            }
            for (int i = 0; i < model.FaunaAsociadaCantidad.Count; i++)
            {
                if (model.FaunaAsociadaCantidad[i] > 0)
                {
                    await _nuevoRegistroFaunaAsociadaAvistamiento.Execute(
                        IdAvistamiento,
                        model.FaunaAsociadaId[i],
                        model.FaunaAsociadaCantidad[i],
                        "prueba"
                        );
                }
            }
            return RedirectToAction("NuevoRegistro");
        }

        [HttpPost]
        public async Task<IActionResult> listarAvistamientos(int pagina, int rpp)
        {
            var datosTabla = await _listarRegistrosAvistamientoFlora.Execute(pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"#\"></a>" +
                        "<button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button>" + " </td>";
                    filasTabla += "<td><span>" + fila.Id + "</span></br>" + "</td>";
                    filasTabla += "<td>" + fila.IdFicha + "</td>";
                    filasTabla += "<td>" + fila.Fechavalidacion + "</td>";
                    filasTabla += "<td>" + fila.NombreZona + "</td>";
                    filasTabla += "<td>" + fila.Ubicacion_Este + "," + fila.Ubicacion_Norte + "</td>";
                    filasTabla += "<td>" + fila.Ubicacion_Altitud + "</td>";
                    filasTabla += "<td>" + fila.Geneto + "</td>";
                    filasTabla += "<td>" + fila.Rameto + "</td>";
                    filasTabla += "<td>" + fila.Condicion_Reubicacion + "</td>";
                    filasTabla += "<td>" + fila.Sustrato + "</td>";
                    filasTabla += "<td>" + fila.Cobertura_Diametro_Mayor + "</td>";
                    filasTabla += "<td>" + fila.Cobertura_Diametro_Menor + "</td>";
                    filasTabla += "<td>" + fila.Cobertura_Diametro_Altura + "</td>";
                    filasTabla += "</tr>";
                }
                //Paginador
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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

        public async Task<ActionResult> GetAvistameinto(int id)
        {
            var datos = await _listarRegistrosAvistamientoFlora.BuscarPorID(id);
            var dataNombres = await _listarNombresCientificosByAvistamiento.BuscarPorIdAvistamiento(id);
            var dataFenologias = await _listarFenologiasByAvistamiento.BuscarPorIdAvistamiento(id);
            var dataVigores = await _listarVigorByAvistamiento.BuscarPorIdAvistamiento(id);
            var dataSaluds = await _listarSaludByAvistamiento.BuscarPorIdAvistamiento(id);
            var dataUsos = await _listarUsoByAvistamiento.BuscarPorIdAvistamiento(id);
            var faunas = await _listarFaunaByAvistamiento.BuscarPorIdAvistamiento(id);
            dynamic response = new
            {
                Data = datos,
                Nombres = dataNombres.Registros,
                Fenologias = dataFenologias.Registros,
                Vigores = dataVigores.Registros,
                Saluds = dataSaluds.Registros,
                Usosos = dataUsos.Registros,
                Faunas = faunas.Registros,
            };
            return Ok(response);
        }

        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Avistamiento_Fauna_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + nFile;
            return fileName;
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private async Task<string> CargarImagen(IFormFile file, string newFileName)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona V2/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    file.CopyTo(stream);

                    ImageOptimizer optimizer = new ImageOptimizer();

                    // Set the stream position to the beginning of the file.
                    stream.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        optimizer.LosslessCompress(stream);
                    }
                    catch (Exception e)
                    {
                        Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                        Log.Error(e.Message);
                    }
                    stream.Flush();

                    stream.Close();
                }
                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/" + newFileName;
                await _gestorEncriptado.EncryptFileAsync(fileName, encryptedArchiveFile);

                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, Tipo_Info_SGA.FTPConnectionAvistamientoFlora);
            }
            else
                return "Error";
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
    }
}
