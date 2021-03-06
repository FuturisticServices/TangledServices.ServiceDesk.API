﻿using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using TangledServices.ServiceDesk.API.Common;
using TangledServices.ServiceDesk.API.Entities;

namespace TangledServices.ServiceDesk.API.Managers
{
    public interface ITenantSubscriptionsManager
    {
        Task<DatabaseResponse> DeleteDatabase();
        Task<DatabaseResponse> CreateDatabase();
        Task<ContainerResponse> CreateContainer(Database database, string containerName, string partitionKeyName);
        Task<List<string>> GetContainers(List<string> containersToOmit);
    }

    public class TenantSubscriptionsManager : TenantBaseManager, ITenantSubscriptionsManager
    {
        private readonly ISystemTenantManager _systemTenantManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        internal IConfiguration _configuration;
        internal IWebHostEnvironment _webHostEnvironment;

        public TenantSubscriptionsManager(ISystemTenantManager systemTenantManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base("Subscriptions", systemTenantManager, httpContextAccessor, configuration, webHostEnvironment)
        {
            _systemTenantManager = systemTenantManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<DatabaseResponse> DeleteDatabase()
        {
            _databaseName = _webHostEnvironment.EnvironmentName == "Production" ? _configuration["cosmosDb.Production:TenantDatabaseName"] : _configuration["cosmosDb.Localhost:TenantDatabaseName"];

            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(_uri, _primaryKey);
            CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();
            Database database = client.GetDatabase(_databaseName);
            DatabaseResponse response = await database.DeleteAsync();

            return response;
        }

        public async Task<DatabaseResponse> CreateDatabase()
        {
            _databaseName = _webHostEnvironment.EnvironmentName == "Production" ? _configuration["cosmosDb.Production:TenantDatabaseName"] : _configuration["cosmosDb.Localhost:TenantDatabaseName"];

            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(_uri, _primaryKey);
            CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();
            DatabaseResponse response = await client.CreateDatabaseIfNotExistsAsync(_databaseName);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response = await DeleteDatabase();
                response = await client.CreateDatabaseAsync(_databaseName);
            }

            return response;
        }

        public async Task<ContainerResponse> CreateContainer(Database database, string containerName, string partitionKeyName)
        {
            ContainerProperties containerProperties = new ContainerProperties()
            {
                Id = containerName,
                PartitionKeyPath = string.Format("/{0}", partitionKeyName),
                IndexingPolicy = new IndexingPolicy()
                {
                    Automatic = false,
                    IndexingMode = IndexingMode.Lazy,
                }
            };

            ContainerResponse response = await database.CreateContainerIfNotExistsAsync(containerProperties);

            return response;
        }

        public async Task<List<string>> GetContainers(List<string> lookupGroupsToClone)
        {
            List<string> containers = new List<string>();

            Database database = _dbClient.GetDatabase(_databaseName);
            FeedIterator<ContainerProperties> iterator = database.GetContainerQueryIterator<ContainerProperties>();
            FeedResponse<ContainerProperties> tenantContainers = await iterator.ReadNextAsync().ConfigureAwait(false);

            foreach (var tenantContainer in tenantContainers) {
                if (lookupGroupsToClone.Contains(tenantContainer.Id)) {
                    containers.Add(tenantContainer.Id);
                }
            }

            return containers;
        }
    }
}