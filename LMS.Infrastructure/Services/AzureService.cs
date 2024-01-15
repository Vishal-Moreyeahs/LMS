﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Models.Blob;
using LMS.Application.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LMS.Infrastructure.Services
{
    public class AzureService : IAzureService
    {
        private readonly AzureStorageConfig _azureStorage;
        private readonly FileBankStorageConfig _fileStorage;
        public AzureService(IOptions<AzureStorageConfig> azureStorage, IOptions<FileBankStorageConfig> fileStorage)
        {
            _azureStorage = azureStorage.Value;
            _fileStorage = fileStorage.Value;
        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename, string containerName = null)
        {
            containerName = string.IsNullOrEmpty(containerName) ? _fileStorage.FileBankContainerName : containerName;

            BlobContainerClient client = new BlobContainerClient(_azureStorage.StorageConnectionString, containerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (Exception ex)
            {
                // File did not exist, log to console and return new response to requesting method
                //_logger.LogError($"File {blobFilename} was not found.");
                throw new ApplicationException($"File with name {blobFilename} not found.");
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }

        public async Task<BlobDto> DownloadAsync(string blobFilename, string containerName = null)
        {
            containerName = string.IsNullOrEmpty(containerName) ? _fileStorage.FileBankContainerName : containerName;

            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new BlobContainerClient(_azureStorage.StorageConnectionString, containerName);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (Exception ex)
            {
                // Log error to console
                //_logger.LogError($"File {blobFilename} was not found.");
                throw new ApplicationException($"File {blobFilename} was not found.");
            }

            // File does not exist, return null and handle that in requesting method
            return null;
        }

        public async Task<List<BlobDto>> ListAsync(string containerName = null)
        {
            containerName = string.IsNullOrEmpty(containerName) ? _fileStorage.FileBankContainerName : containerName;

            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_azureStorage.StorageConnectionString, containerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }


            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDto> ReplaceAsync(IFormFile file, string blobFilename, string containerName = null)
        {
            containerName = string.IsNullOrEmpty(containerName) ? _fileStorage.FileBankContainerName : containerName;

            if (!string.IsNullOrEmpty(blobFilename))
            { 
                // Delete the existing file
                var deleted = await DeleteAsync(blobFilename, containerName);

                if (deleted.Error)
                {
                    throw new ApplicationException($"{deleted.Status}");
                }
            }

            // Upload the new file
            return await UploadAsync(file, containerName);
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string containerName = null)
        {
            containerName = string.IsNullOrEmpty(containerName) ? _fileStorage.FileBankContainerName : containerName;

            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();



            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_azureStorage.StorageConnectionString, containerName);

            // Create the container if it does not exist
            await container.CreateIfNotExistsAsync();

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                //_logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            catch (RequestFailedException ex)
            {
                //_logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            return response;
        }
    }
}
