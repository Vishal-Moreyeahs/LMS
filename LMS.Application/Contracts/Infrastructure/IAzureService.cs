using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Models.Blob;
using Microsoft.AspNetCore.Http;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface IAzureService
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file);

        Task<BlobDto> DownloadAsync(string blobFilename);

        Task<BlobResponseDto> DeleteAsync(string blobFilename);

        Task<List<BlobDto>> ListAsync();
    }
}