using GestionAmbiental.Application.Repositories.Info_SGA;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA.EditarInfo_SGA
{
    public class EditarInfo_SGA : IEditarInfo_SGA
    {
        private readonly IInfo_SGAReadOnlyRepository _info_SGAReadOnlyRepository;

        public EditarInfo_SGA(IInfo_SGAReadOnlyRepository info_SGAReadOnlyRepository)
        {
            _info_SGAReadOnlyRepository = info_SGAReadOnlyRepository ?? throw new ArgumentNullException(nameof(info_SGAReadOnlyRepository));
        }

        public async Task Execute(int idInfo, string tipo, string info)
        {
            var encrypter = new Info_SGAOutput();
            var info_SGA = new Domain.ValueObjects.Info_SGA();
            info_SGA.Id_Info = idInfo;
            info_SGA.Tipo = tipo;
            info_SGA.Info = encrypter.GetEncryptedInfo(info.Replace("\\\"","\""));

            await _info_SGAReadOnlyRepository.UpdateInfo_SGA(info_SGA);
        }
    }
}
