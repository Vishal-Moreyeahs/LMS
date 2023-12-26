using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Contracts.Infrastructure
{
    public interface ICryptographyService
    {
        byte[] EncryptPassword(string password);

        bool ValidatePassword(byte[] p1, byte[] p2);
    }
}
