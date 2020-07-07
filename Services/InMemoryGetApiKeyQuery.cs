using DotnetCoreWebApiRedoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCoreWebApiRedoc.Services
{
    public interface IGetApiKeyQuery
    {
        Task<ApiKey> Execute(string providedApiKey);
    }

    public class InMemoryGetApiKeyQuery : IGetApiKeyQuery
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;

        public InMemoryGetApiKeyQuery()
        {
            var existingApiKeys = new List<ApiKey>
            {
                new ApiKey(
                    1, 
                    "Some Third Party", 
                    "FA872702-6396-45DC-89F0-FC1BE900591B", 
                    new DateTime(2019, 06, 01),
                    new List<string>
                    {
                        Roles.ThirdParty
                    })
            };

            _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            _apiKeys.TryGetValue(providedApiKey, out var key);
            return Task.FromResult(key);
        }
    }
}
