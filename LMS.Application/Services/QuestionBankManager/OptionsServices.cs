using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Models.Blob;
using LMS.Application.Request;
using LMS.Domain.Models;

namespace LMS.Application.Services.QuestionBankManager
{
    public class OptionsServices : IOptionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private readonly ImageStorageConfig _imageStorage;

        public OptionsServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService, IMapper mapper, IAzureService azureService, ImageStorageConfig imageStorage)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
            _azureService = azureService;
            _imageStorage = imageStorage;
        }

        public async Task<bool> AddOptionsToQuestion(List<OptionRequest> options)
        {
            if (options.Count <= 0 || options == null)
            {
                throw new ApplicationException("Options can not be null");
            }

            foreach (var option in options)
            {
                await CreateOptions(option);
            }
            return true;
        }

        private async Task CreateOptions(OptionRequest option)
        {
            var filePath = "";
            if (option.IsImageAttached)
            {
                if (option.ImageFile == null)
                {
                    throw new ApplicationException("Image is necessary !!");
                }
                var isImageUploaded = await _azureService.UploadAsync(option.ImageFile, _imageStorage.ImageStorageContainerName);

                if (isImageUploaded.Error)
                {
                    throw new ApplicationException($"{isImageUploaded.Status}");
                }
                filePath = isImageUploaded.Blob.Uri;
            }

            var data = _mapper.Map<Option>(option);
            data.ImagePath = filePath;

            await _unitOfWork.GetRepository<Option>().Add(data);
            var isDataAdded = await _unitOfWork.SaveChangesAsync();

            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Option - {option.OptionValue} not added");
            }
        }
    }
}
