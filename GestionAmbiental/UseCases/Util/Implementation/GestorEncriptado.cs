using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;

namespace GestionAmbiental.UseCases.Util.Implementation
{
    public class GestorEncriptado : IGestorEncriptado
    {
        private readonly IGetInfo_SGA _info_SGA;

        public GestorEncriptado(IGetInfo_SGA info_SGA)
        {
            _info_SGA = info_SGA;
        }

        public async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            System.Security.Cryptography.RijndaelManaged RMCrypto = new System.Security.Cryptography.RijndaelManaged();

            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                System.Security.Cryptography.CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        public async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            System.Security.Cryptography.RijndaelManaged RMCrypto = new System.Security.Cryptography.RijndaelManaged();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                System.Security.Cryptography.CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
    }
}