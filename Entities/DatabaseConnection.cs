﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using FuturisticServices.ServiceDesk.API.Models;
using FuturisticServices.ServiceDesk.API.Common;

namespace FuturisticServices.ServiceDesk.API.Entities
{
    /// <summary>
    /// Physical email address.
    /// </summary>
    public class DatabaseConnection : EntityBase
    {
        public DatabaseConnection() { }

        public DatabaseConnection(LookupItem databasePlatform, string databaseName, string uri, string primaryKey)
        {
            Id = Guid.NewGuid().ToString();
            DatabasePlatform = databasePlatform;
            DatabaseName = databaseName;
            Uri = uri;
            PrimaryKey = primaryKey;
        }

        /// <summary>
        /// Database platform.
        /// </summary>
        [JsonProperty(PropertyName = "databasePlatform", Required = Required.Always)]
        [Required]
        public LookupItem DatabasePlatform { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [JsonProperty(PropertyName = "databaseName", Required = Required.Always)]
        [Required]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [JsonProperty(PropertyName = "uri", Required = Required.Always)]
        [Required]
        public string Uri { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [JsonProperty(PropertyName = "primaryKey", Required = Required.Always)]
        [Required]
        public string PrimaryKey { get; set; }
    }
}
