using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Request;
using LMS.Domain.Models;

namespace LMS.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegistrationRequest, Employee>().ReverseMap();
        }
    }
}
