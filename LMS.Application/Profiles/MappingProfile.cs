using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegistrationRequest, Employee>().ReverseMap();
            CreateMap<CompanyRequest, Company>().ReverseMap();
            CreateMap<CompanyData, Company>().ReverseMap();
            CreateMap<DomainRequest, Domains>().ReverseMap();
            CreateMap<UserData, Employee>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<CourseRequest, Course>().ReverseMap();
            CreateMap<CourseContentDto, CourseContent>().ReverseMap();
            CreateMap<CourseContentRequest, CourseContent>().ReverseMap();
            CreateMap<DomainDTO, Domains>().ReverseMap();
            CreateMap<FileBankResponse, FileBank>().ReverseMap();
            CreateMap<FileBankDTO, FileBank>().ReverseMap();
            CreateMap<SubDomainDTO, SubDomain>().ReverseMap();
            CreateMap<SubDomainRequest, SubDomain>().ReverseMap();
            CreateMap<UploadFileBankRequest, FileBank>().ReverseMap();
            CreateMap<QuestionBankRequest, QuestionBank>().ReverseMap();
            CreateMap<QuestionBankDto, QuestionBank>().ReverseMap();
            CreateMap<QuestionBankResponse, QuestionBank>().ReverseMap();
            CreateMap<OptionRequest, Option>().ReverseMap();
        }
    }
}