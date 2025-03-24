using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, out string hmac);
        string Decrypt(string cipherText, string hmac);
    }
}
