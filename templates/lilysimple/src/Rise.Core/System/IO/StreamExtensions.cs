using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static string ComputeHash(this Stream stream)
        {
            using var hash = SHA1.Create();
            return ComputeHash(stream, hash);
        }

        public static string ComputeHash(this Stream stream, HashAlgorithm hash)
        {
            stream.Position = 0;
            var hashBytes = hash.ComputeHash(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
