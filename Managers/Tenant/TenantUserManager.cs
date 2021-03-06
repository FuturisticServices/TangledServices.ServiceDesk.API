﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using TangledServices.ServiceDesk.API.Common;
using TangledServices.ServiceDesk.API.Entities;
using TangledServices.ServiceDesk.API.Managers;

namespace TangledServices.ServiceDesk.API.Managers
{
    public interface ITenantUserManager
    {
        //Task<LookupGroup> GetItemAsync(string lookupName);
        //Task<LookupItem> GetItemAsync(string lookupName, string name);
        //Task<IEnumerable<LookupGroup>> GetItemsAsync();
        Task<Entities.User> CreateItemAsync(Entities.User user);

}

public class TenantUserManager : TenantBaseManager, ITenantUserManager
    {
        internal ISystemTenantManager _systemCustomerService;
        internal IHttpContextAccessor _httpContextAccessor;
        internal IConfiguration _configuration;
        internal IWebHostEnvironment _webHostEnvironment;

        public TenantUserManager(ISystemTenantManager systemTenantService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base("Users", systemTenantService, httpContextAccessor, configuration, webHostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        //public async Task<LookupGroup> GetItemAsync(string groupName)
        //{
        //    Enums.LookupGroups lookupGroup;

        //    groupName = groupName.ToCamelCase();
        //    if (Enum.TryParse<Enums.LookupGroups>(groupName, true, out lookupGroup))
        //    {
        //        groupName = lookupGroup.GetDescription().ToCamelCase();

        //        QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.lookupName = @groupName")
        //        .WithParameter("@groupName", groupName);

        //        LookupGroup result = new LookupGroup();
        //        using (FeedIterator<LookupGroup> feedIterator = _container.GetItemQueryIterator<LookupGroup>(query))
        //        {
        //            while (feedIterator.HasMoreResults)
        //            {
        //                FeedResponse<LookupGroup> response = await feedIterator.ReadNextAsync();
        //                foreach (var item in response)
        //                {
        //                    result = item;
        //                }
        //            }
        //        }

        //        return result;
        //    }

        //    return null;
        //}

        //public async Task<LookupItem> GetItemAsync(string groupName, string itemName)
        //{
        //    var query = _container.GetItemLinqQueryable<LookupGroup>(true);
        //    LookupGroup lookupItems = query.Where<LookupGroup>(x => x.Name == groupName).AsEnumerable().FirstOrDefault();
        //    LookupItem lookupItem = lookupItems.Items.SingleOrDefault(x => x.Name == itemName);

        //    return lookupItem;
        //}

        //public async Task<IEnumerable<LookupGroup>> GetItemsAsync()
        //{
        //    QueryDefinition query = new QueryDefinition("SELECT * FROM c");

        //    List<LookupGroup> result = new List<LookupGroup>();
        //    using (FeedIterator<LookupGroup> feedIterator = _container.GetItemQueryIterator<LookupGroup>(query))
        //    {
        //        while (feedIterator.HasMoreResults)
        //        {
        //            FeedResponse<LookupGroup> response = await feedIterator.ReadNextAsync();
        //            foreach (var item in response)
        //            {
        //                result.Add(item);
        //            }
        //        }
        //    }

        //    return result;
        //}

        public async Task<Entities.User> CreateItemAsync(Entities.User user)
        {
            var results = await _container.CreateItemAsync<Entities.User>(user, new PartitionKey(user.EmployeeID));
            return results;
        }
    }
}
