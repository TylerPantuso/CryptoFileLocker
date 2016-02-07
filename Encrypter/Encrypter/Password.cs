using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter
{
    public class Password
    {
        public readonly byte[] Hash; // 31 bytes; 44 Base64 characters
        public readonly Guid Salt; // 24 Base64 characters

        public Password(byte[] hash, Guid salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}
