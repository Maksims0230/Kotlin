using KinoStudio_NET.Annotations;
using System;
using System.Security.Cryptography;
using System.Text;

namespace KinoStudio_NET.Helpers
{
    internal class MD5Helper
    {
        private readonly string _notEncryptedLine;

        public string GetEncryptedLine() =>
            Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(_notEncryptedLine)));

        public string GetNotEncryptedLine() => _notEncryptedLine;

        public MD5Helper([NotNull] string line) => _notEncryptedLine = line;

        public void Deconstruct([NotNull] out string notEncryptedLine) => notEncryptedLine = _notEncryptedLine;
    }
}