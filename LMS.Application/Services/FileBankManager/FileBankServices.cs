using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Enums;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Application.Services.FileBankManager
{
    public class FileBankServices : IFileBankServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureService _azureService;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public FileBankServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService
                , IMapper mapper, IAzureService azureService)
        { 
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task<Response<FileBankResponse>> DeleteFileFromFileBank(int id)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var fileDetails = await _unitOfWork.GetRepository<FileBank>().Get(id);
            if (fileDetails == null || !fileDetails.IsActive || fileDetails.Company_Id != loggedInUser.CompanyId)
            {
                throw new ApplicationException("File Not Found");
            }

            fileDetails.IsActive = false;
            //fileDetails.UpdatedDate = DateTime.UtcNow;
            //fileDetails.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<FileBank>().Add(fileDetails);
            var isDeleted = await _unitOfWork.SaveChangesAsync();
            if (isDeleted <= 0)
            {
                throw new ApplicationException("Fail to delete data");
            }
            //var fileName = fileDetails.Path.Split("/").ToList().Last();
            //var isDeletedFromCloud = await _azureService.DeleteAsync(fileName);
            //if (isDeletedFromCloud.Error)
            //{
            //    throw new ApplicationException($"{isDeletedFromCloud.Status}");
            //}
            return new Response<FileBankResponse>
            {
                Status = true,
                Message = $"Deleted file - {id}",
                Data = _mapper.Map<FileBankResponse>(fileDetails)
            };
        }

        public async Task<Response<List<FileBankDTO>>> GetAllFileFromFileBank()
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var fileDetails = await _unitOfWork.GetRepository<FileBank>().GetAll();
            if (fileDetails == null)
            { 
                throw new ApplicationException("Files not found");
            }

            var files = fileDetails.Where(x => x.Company_Id == loggedInUser.CompanyId && x.IsActive).ToList();
            var response = new Response<List<FileBankDTO>>
            {
                Status = true,
                Message = "File Retreived successfully",
                Data = _mapper.Map<List<FileBankDTO>>(files)
            };
            return response;
        }

        public async Task<dynamic> GetFileFromFileBank(int id)
        {
            var fileDetails = await _unitOfWork.GetRepository<FileBank>().Get(id);
            
            if (fileDetails == null)
            {
                throw new ApplicationException("File Not Found");
            }

            var fileName = fileDetails.Path.Split("/").Last();
            var file = await _azureService.DownloadAsync(fileName);

            return file;
        }

        public async Task<Response<FileBankResponse>> UpdateFileInFileBank(FileBankDTO fileBankDTO)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var fileDetails = await _unitOfWork.GetRepository<FileBank>().Get(fileBankDTO.Id);

            if (fileDetails == null || !fileDetails.IsActive || fileDetails.Company_Id != loggedInUser.CompanyId )
            {
                throw new ApplicationException($"Data Not Found");
            }

            _mapper.Map(fileBankDTO, fileDetails);
            //fileDetails.UpdatedDate = DateTime.UtcNow;
            //fileDetails.UpdatedBy = loggedInUser.EmployeeId;

            var result = _unitOfWork.GetRepository<FileBank>().Update(fileDetails);
            var isUpdated = await _unitOfWork.SaveChangesAsync();
            if (isUpdated <= 0)
            {
                throw new ApplicationException("Data updation failed.");
            }
            var response = new Response<FileBankResponse>
            {
                Status = true,
                Message = $"User with id - {fileDetails.Id} Updated Successfully",
                Data = _mapper.Map<FileBankResponse>(fileDetails)
            };
            return response;
        }

        public async Task<Response<bool>> UploadFileInFileBank(UploadFileBankRequest fileBankRequest)
        {
            if (fileBankRequest.File == null || fileBankRequest.File.Length == 0)
            {
                throw new ApplicationException("Invalid file");
            }
            try
            {
                var isUploaded = await _azureService.UploadAsync(fileBankRequest.File);

                if (isUploaded.Error)
                {
                    throw new ApplicationException($"{isUploaded.Status}");
                }

                var loggedInUser =await _authenticatedUserService.GetLoggedInUser();
                long fileSizeInBytes = fileBankRequest.File.Length;
                double fileSizeInKb = fileSizeInBytes / 1024.0;
                byte[] file;
                using (var memoryStream = new MemoryStream())
                {
                    await fileBankRequest.File.CopyToAsync(memoryStream);
                    file = memoryStream.ToArray();
                    
                }
                var entity = _mapper.Map<FileBank>(fileBankRequest);
                entity.Size = fileSizeInKb.ToString();
                entity.Format = fileBankRequest.File.ContentType;
                entity.Path = isUploaded.Blob.Uri;
                entity.Company_Id = loggedInUser.CompanyId;
                //entity.UpdatedBy = loggedInUser.EmployeeId;
                //entity.CreatedBy = loggedInUser.EmployeeId;
                //entity.CreatedDate = DateTime.UtcNow;
                //entity.UpdatedDate = DateTime.UtcNow;
                entity.IsActive = true;

                await _unitOfWork.GetRepository<FileBank>().Add(entity);
                var isFileUploaded = await _unitOfWork.SaveChangesAsync();
                if (isFileUploaded <= 0)
                {
                    throw new ApplicationException("File Can not uploaded. please try after sometime.");
                }
                var response = new Response<bool>
                {
                    Status = true,
                    Message = "File Uploaded Successfully.",
                    Data = true
                };

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
