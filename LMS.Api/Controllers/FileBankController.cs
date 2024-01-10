using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using LMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FileBankController : ControllerBase
    {
        private readonly IFileBankServices _fileBankServices;
        public FileBankController(IFileBankServices fileBankServices)
        { 
            _fileBankServices = fileBankServices;
        }

        [HttpPost]
        [Route("uploadfile")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileBankRequest uploadFileBankRequest)
        {
            return Ok(await _fileBankServices.UploadFileInFileBank(uploadFileBankRequest));
        }

        [HttpGet]
        [Route("getfileById")]
        public async Task<IActionResult> GetFileById(int id)
        {
            var data = await _fileBankServices.GetFileFromFileBank(id);
            return File(data.Path, data.Format);
        }

        [HttpGet]
        [Route("getAllfiles")]
        public async Task<IActionResult> GetAllFiles()
        {
            return Ok(await _fileBankServices.GetAllFileFromFileBank());
        }

        [HttpPost]
        [Route("deleteFile")]
        public async Task<IActionResult> RemoveFileFromFileBank(int id)
        {
            return Ok(await _fileBankServices.DeleteFileFromFileBank(id));
        }

        [HttpPost]
        [Route("updateFile")]
        public async Task<IActionResult> UpdateFileInFileBank(FileBankDTO fileBankDTO)
        {
            return Ok(await _fileBankServices.UpdateFileInFileBank(fileBankDTO));
        }
    }
}
