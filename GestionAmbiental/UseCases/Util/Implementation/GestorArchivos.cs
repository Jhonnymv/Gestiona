using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Util.Implementation
{
    public class GestorArchivos : IGestorArchivos
    {
        private readonly IGetInfo_SGA _info_SGA;

        public GestorArchivos(IGetInfo_SGA info_SGA)
        {
            _info_SGA = info_SGA;
        }

        public async Task<string> UploadFTPFileAsync(string path, string newFileName, Tipo_Info_SGA tipoSFTP)
        {
            var serverInfo = await _info_SGA.Execute(tipoSFTP);
            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }

            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    sftp.UploadFile(fs, newFileName, true);
                }
                sftp.Dispose();

                return newFileName;
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName, string usuario, Tipo_Info_SGA tipoSFTP)
        {
            var serverInfo = await _info_SGA.Execute(tipoSFTP);
            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }
            /* Save file in application server to decrypt */
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "Descarga Temporal Gestiona/";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission/" + usuario + "/";
            var archivePath = SGATempPath + "zip/" + usuario + "/";

            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
            }
            if (Directory.Exists(archivePath))
            {
                var files = Directory.GetFiles(archivePath);
                Directory.Delete(archivePath, true);
            }

            Directory.CreateDirectory(filePath);
            Directory.CreateDirectory(archivePath);
            /*New file downloaded name*/
            var downloadedFilePath = archivePath + fileName;
            /*Copy data to new temp file*/
            var fileStream = System.IO.File.Create(downloadedFilePath);
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                //Descargar archivo de sftp a nuevo archivo temporal creado
                sftp.DownloadFile(fileName, fileStream);

                sftp.Dispose();
                fileStream.Dispose();
            }
            //fileStream.Close();

            return downloadedFilePath;
        }
        public async Task DeleteFTPFileAsync(string filename, Tipo_Info_SGA tipoSFTP)
        {
            var serverInfo = await _info_SGA.Execute(tipoSFTP);
            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                sftp.DeleteFile(filename);

                sftp.Dispose();
            }
        }
    }
}