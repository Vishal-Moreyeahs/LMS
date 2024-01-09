﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Repositories
{
    public interface IFileBankServices
    {
        Task<Response<bool>> UploadFileInFileBank(UploadFileBankRequest fileBankRequest);
        Task<FileBank> GetFileFromFileBank(int id);
        Task<Response<FileBankResponse>> DeleteFileFromFileBank(int id);
        Task<Response<FileBankResponse>> UpdateFileInFileBank(FileBankDTO fileBankDTO);
    }
}
