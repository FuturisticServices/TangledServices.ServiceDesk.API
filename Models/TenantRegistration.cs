﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FuturisticServices.ServiceDesk.API.Entities;

namespace FuturisticServices.ServiceDesk.API.Models
{
    /// <summary>
    /// Contains information required to create/setup a tenant in the
    /// FuturisticServices-->Tenant container.
    /// </summary>
    public class TenantRegistration
    {
        /// <summary>
        /// Contains information related to the tenant's legal business entity.
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// Contains information related to tenant's primary contact.
        /// </summary>
        public PointOfContact PointOfContact { get; set; }

        /// <summary>
        /// The selected subscription ID.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Contains information to charge the tenant for subscription feees.
        /// </summary>
        public BillingInformation BillingInformation { get; set; }

        /// <summary>
        /// A unique abbreviation that identifies the tenant in the system.
        /// Included in each and every API request to identify which
        /// database the system will interact with.
        /// </summary>
        public string Moniker { get; set; }
    }
}
