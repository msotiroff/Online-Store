using MSToolKit.Core.Authentication.Abstraction;
using System.Security.Cryptography;
using System.Text;

namespace MSToolKit.Core.Authentication
{
    /// <summary>
    /// Provides a default implementation for MSToolKit.Core.Authentication.Abstraction.IPasswordHasher.
    /// </summary>
    internal class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Returns hashed representation of the supplied password.
        /// </summary>
        /// <param name="input">The password in plain text.</param>
        /// <returns>Hashed input.</returns>
        public string ComputeHash(string text)
        {
            string hashString;
            using (var sha256 = SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(Encoding.Default.GetBytes(text));
                hashString = ToHex(hash, false);
            }

            return hashString;
        }

        private string ToHex(byte[] bytes, bool upperCase)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }
    }
}