using System.Text;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Konscious.Security.Cryptography;

namespace Infrastructure.UserServices.Domain
{
    internal sealed class PasswordGenerator : IPasswordGenerator
    {
        public async Task<Password> GenerateAsync(
            PasswordInput password,
            CancellationToken cancellationToken
        )
        {
            byte[] salt = new byte[16];
            Random.Shared.NextBytes(salt);

            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(password.Value));
            argon.Iterations = 8;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = salt;
            byte[] hash = await argon.GetBytesAsync(32);

            return new(hash, salt);
        }
    }
}
