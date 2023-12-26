using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LMS.Application.Contracts.Infrastructure;

namespace LMS.Infrastructure.Services
{
    public class CryptographyService : ICryptographyService
    {
        public byte[] EncryptPassword(string password)
        {
            // Initialize a SHA256 hash object
            SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool ValidatePassword(byte[] p1, byte[] p2)
        {
            if (p1 == null && p2 == null)
            {
                return true;
            }
            // Check if one of the arrays is null (but not both)
            if (p1 == null || p2 == null)
            {
                return false;
            }
            // Check if arrays have the same length
            if (p1.Length != p2.Length)
            {
                return false;
            }

            // Compare each byte in the arrays
            for (int i = 0; i < p1.Length; i++)
            {
                if (p1[i] != p2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
