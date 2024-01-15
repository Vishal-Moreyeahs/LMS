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
        Task<BlobResponseDto> UploadAsync(IFormFile file, string containerName = null);

        Task<BlobDto> DownloadAsync(string blobFilename, string containerName = null);

        Task<BlobResponseDto> DeleteAsync(string blobFilename, string containerName = null);

        Task<List<BlobDto>> ListAsync(string containerName = null);
    }
}