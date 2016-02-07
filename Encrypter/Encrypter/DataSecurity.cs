using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encrypter
{
    public class DataSecurity
    {
        public Password Sha256Encrypt(SecureString securePassword, Guid salt)
        {
            foreach (byte saltByte in salt.ToByteArray())
            {
                securePassword.AppendChar(Convert.ToChar(saltByte));
            }

            securePassword.MakeReadOnly();

            IntPtr password = IntPtr.Zero;
            byte[] byteArray;
            byte[] hash;
            Password hashedPassword;

            try
            {
                password = Marshal.SecureStringToBSTR(securePassword);

                int passwordByteLength = Marshal.ReadInt32(password, -4);
                byteArray = new byte[passwordByteLength];

                Marshal.Copy(password, byteArray, 0, passwordByteLength);

                using (SHA256 sha256 = SHA256.Create())
                {
                    hash = sha256.ComputeHash(byteArray);
                }

                hashedPassword = new Password(hash, salt);
            }
            catch
            {
                throw;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(password);
            }

            return hashedPassword;
        }

        public static bool IsValidPassword(User user, SecureString entry)
        {
            DataSecurity security = new DataSecurity();
            Password encryptedEntry = security.Sha256Encrypt(entry, user.Password.Salt);

            return user.Password.Hash.SequenceEqual(encryptedEntry.Hash);
        }
    }
}
