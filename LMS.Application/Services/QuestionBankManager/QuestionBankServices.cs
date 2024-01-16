using AutoMapper;
using LMS.Application.Contracts.Infrastructure;
using LMS.Application.Contracts.Persistence;
using LMS.Application.Contracts.Repositories;
using LMS.Application.Models;
using LMS.Application.Models.Blob;
using LMS.Application.Request;
using LMS.Application.Response;
using LMS.Domain.Enums;
using LMS.Domain.Models;
using Microsoft.Extensions.Options;

namespace LMS.Application.Services.QuestionBankManager
{
    public class QuestionBankServices : IQuestionBankServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private readonly ImageStorageConfig _imageStorage;
        public QuestionBankServices(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUserService, IMapper mapper, IOptions<ImageStorageConfig> imageStorage,
                                    IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUserService = authenticatedUserService;
            _mapper = mapper;
            _imageStorage = imageStorage.Value;
            _azureService = azureService;
        }
        public async Task<Response<dynamic>> GetAllQuestions()
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var questions = _unitOfWork.GetRepository<QuestionBank>().GetAllRelatedEntity();

            if (questions == null)
            {
                throw new ApplicationException("Questions not found in question bank");
            }

            questions = questions.Where(x => x.IsActive);

            var response = new Response<dynamic> { 
                Status = true,
                Message = "All Questions Retreived",
                Data = questions
            };

            return response;
        }

        public async Task<Response<QuestionBankResponse>> CreateQuestion(QuestionBankRequest question)
        {
            var filePath = "";
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            if (loggedInUser == null || loggedInUser.RoleId != (int)RoleEnum.Admin)
            {
                throw new ApplicationException($"Logged in user does not have Permission to Add Sub-Domain");
            }

            if (question.IsImageAttached)
            {
                if (question.ImageFile == null)
                {
                    throw new ApplicationException("Image is necessary !!");
                }
                var isImageUploaded = await _azureService.UploadAsync(question.ImageFile, _imageStorage.ImageStorageContainerName);

                if (isImageUploaded.Error)
                {
                    throw new ApplicationException($"{isImageUploaded.Status}");
                }
                filePath = isImageUploaded.Blob.Uri;
            }

            var data = _mapper.Map<QuestionBank>(question);
            data.ImagePath = filePath;
            //data.CreatedDate = DateTime.UtcNow;
            //data.UpdatedDate = DateTime.UtcNow;
            //data.UpdatedBy = loggedInUser.EmployeeId;
            //data.CreatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<QuestionBank>().Add(data);
            var isDataAdded = await _unitOfWork.SaveChangesAsync();

            if (isDataAdded <= 0)
            {
                throw new ApplicationException($"Question should not be added");
            }

            var response = new Response<QuestionBankResponse> { Status = true, Message = "Question Added Successfully", Data = _mapper.Map<QuestionBankResponse>(data) };

            return response;
        }

        public async Task<Response<QuestionBankResponse>> GetQuestionsById(int id)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var questions = _unitOfWork.GetRepository<QuestionBank>().GetAllRelatedEntity();

            var question = questions.Where(x => x.Id == id && x.IsActive).FirstOrDefault();

            if (question == null)
            {
                throw new ApplicationException($"Question with id - {id} not found");
            }

            var response = new Response<QuestionBankResponse> { Status = true, Message = "Question Retreived Successfully", Data = _mapper.Map<QuestionBankResponse>(question) };

            return response;
        }

        public async Task<Response<dynamic>> DeleteQuestionById(int id)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();

            var questions = _unitOfWork.GetRepository<QuestionBank>().GetAllRelatedEntity();

            var question = questions.Where(x => x.Id == id && x.IsActive).FirstOrDefault();

            if (question == null)
            {
                throw new ApplicationException($"Question with id - {id} not found");
            }

            question.IsActive = false;
            //question.UpdatedDate = DateTime.UtcNow;
            //question.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<QuestionBank>().Add(question);
            var isDeleted = await _unitOfWork.SaveChangesAsync();
            if (isDeleted <= 0)
            {
                throw new ApplicationException("Fail to delete data");
            }

            var response = new Response<dynamic> { Status = true, Message = "Question Deleted Successfully" };

            return response;
        }

        public async Task<Response<QuestionBankResponse>> UpdateQuestion(QuestionBankDto question)
        {
            var loggedInUser = await _authenticatedUserService.GetLoggedInUser();
            var newFilePath = "";
            var questionData = await _unitOfWork.GetRepository<QuestionBank>().Get(question.Id);

            if (question.IsImageAttached)
            {
                if(question.ImageFile == null)
                {
                    throw new ApplicationException("Please attach file, file can not be empty");
                }

                var fileName = questionData.IsImageAttached ? questionData.ImagePath.Split("/").ToList().Last() : "";

                var isImageUploaded = await _azureService.ReplaceAsync(question.ImageFile, fileName, _imageStorage.ImageStorageContainerName);

                if (isImageUploaded.Error)
                {
                    throw new ApplicationException($"{isImageUploaded.Status}");
                }
                newFilePath = isImageUploaded.Blob.Uri;
            }

            _mapper.Map(question, questionData);
            questionData.ImagePath = newFilePath;
            //questionData.UpdatedDate = DateTime.UtcNow;
            //questionData.UpdatedBy = loggedInUser.EmployeeId;

            await _unitOfWork.GetRepository<QuestionBank>().Update(questionData);
            var isQuestionUpdated = await _unitOfWork.SaveChangesAsync();

            if (isQuestionUpdated <= 0)
            {
                throw new ApplicationException($"Sub Domain with Id - {question.Id} should not Update");
            }

            var response = new Response<QuestionBankResponse> { Status = true, Message = "Question Updated Successfully", Data = _mapper.Map<QuestionBankResponse>(questionData) };

            return response;

        }
    }
}
