using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models;
using LMS.Application.Request;

namespace LMS.Application.Contracts.Repositories
{
    public interface IOptionServices
    {
        Task<bool> AddOptionsToQuestion(List<OptionRequest> options);
    }
}
