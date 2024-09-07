using System.Text;
using Domain.UserDomain.Exceptions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Konscious.Security.Cryptography;

namespace Infrastructure.UserServices.Domain
{
    internal sealed class PasswordValidator : IPasswordValidator
    {
        public async Task ValidateAsync(
            Password password,
            PasswordInput input,
            CancellationToken cancellationToken
        )
        {
            using Argon2 argon = new Argon2id(Encoding.Default.GetBytes(input.Value));
            argon.Iterations = 8;
            argon.MemorySize = 4096;
            argon.DegreeOfParallelism = 1;
            argon.Salt = password.Salt;
            byte[] inputHash = await argon.GetBytesAsync(32);

            bool result = inputHash.SequenceEqual(password.Hash);

            if (result == false)
                throw new LoginException();
        }
    }
}
