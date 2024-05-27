using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class ClientFactory : IClientFactory
    {
        private readonly IClientDataFactory _dataFactory;

        public ClientFactory(IClientDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public async Task<IClient> Get(ISettings settings, Guid id)
        {
            ClientData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            return data != null ? Create(data) : null;
        }

        private static Client Create(ClientData data) => new Client(data);
    }
}
