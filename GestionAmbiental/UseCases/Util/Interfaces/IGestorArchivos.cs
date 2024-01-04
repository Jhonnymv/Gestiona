using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Domain.Enum;

namespace GestionAmbiental.UseCases.Util.Interfaces
{
    public interface IGestorArchivos
    {
        Task<string> UploadFTPFileAsync(string path, string newFileName, Tipo_Info_SGA tipoSFTP);
        Task<string> DownloadFTPFileAsync(string fileName, string usuario, Tipo_Info_SGA tipoSFTP);
        Task DeleteFTPFileAsync(string filename, Tipo_Info_SGA tipoSFTP);
    }
}