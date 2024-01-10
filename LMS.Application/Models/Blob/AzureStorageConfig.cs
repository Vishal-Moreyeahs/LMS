using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Models.Blob
{
    public class AzureStorageConfig
    {
        public string StorageConnectionString { get; set; }
        public string StorageContainerName { get; set; }
        public string StorageAccountKey { get; set; }
    }
}
