﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using FuturisticServices.ServiceDesk.API.Common;
using FuturisticServices.ServiceDesk.API.Entities;

namespace FuturisticServices.ServiceDesk.API.Services.System
{
    public interface ISystemSubscriptionsService
    {
        Task<Subscription> GetItemAsync(string id);
        Task<IEnumerable<Subscription>> GetItemsAsync();
        Task<IEnumerable<Subscription>> GetItemsAsync(bool includeThoseWithPromotionCode = false);
        Task<Subscription> GetItemByPromotionCodeAsync(string promotionCode);
    }

    public class SubscriptionsService : SystemBaseService, ISystemSubscriptionsService
    {
        internal IConfiguration _configuration;
        internal IWebHostEnvironment _webHostEnvironment;

        public SubscriptionsService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base("Subscriptions", configuration, webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Subscription> GetItemAsync(string id)
        {
            FeedIterator<Subscription> query = _container.GetItemQueryIterator<Subscription>(new QueryDefinition("SELECT * FROM c"));

            List<Subscription> subscriptions = new List<Subscription>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                subscriptions.AddRange(response.ToList());
            }

            Subscription results = subscriptions.SingleOrDefault(x => x.Id == id);

            return results;
        }

        public async Task<IEnumerable<Subscription>> GetItemsAsync()
        {
            FeedIterator<Subscription> query = _container.GetItemQueryIterator<Subscription>(new QueryDefinition("SELECT * FROM c"));

            List<Subscription> results = new List<Subscription>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<IEnumerable<Subscription>> GetItemsAsync(bool includeSubscriptionsWithPromotionCode = false)
        {
            FeedIterator<Subscription> query = query = _container.GetItemQueryIterator<Subscription>(new QueryDefinition("SELECT * FROM c"));

            List<Subscription> results = new List<Subscription>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            results = includeSubscriptionsWithPromotionCode == false ? results.Where(x => x.PromotionCode == null).ToList() : results;

            return results;
        }

        public async Task<Subscription> GetItemByPromotionCodeAsync(string promotionCode)
        {
            FeedIterator<Subscription> query = _container.GetItemQueryIterator<Subscription>(new QueryDefinition("SELECT * FROM c WHERE c.promotionCode = '" + promotionCode + "'"));

            List<Subscription> subscriptions = new List<Subscription>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                subscriptions.AddRange(response.ToList());
            }

            Subscription results = subscriptions.FirstOrDefault();

            return results;
        }
    }
}
