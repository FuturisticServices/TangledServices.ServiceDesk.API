﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using FuturisticServices.ServiceDesk.API.Entities;
using FuturisticServices.ServiceDesk.API.Models;

namespace FuturisticServices.ServiceDesk.API.Profiles
{
    public class TenantUserProfile : Profile
    {
        public TenantUserProfile()
        {
            //CreateMap<zTenantUser, zTenantUserModel>().ReverseMap();
        }
    }
}
