using System.Diagnostics.CodeAnalysis;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions
{
    public sealed class CollaboratorsNotFoundException(Collaborators collaborators)
        : DomainException
    {
        public Collaborators Collaborators { get; } = collaborators;

        [DoesNotReturn]
        public static void Throw(Collaborators collaborators)
        {
            throw new CollaboratorsNotFoundException(collaborators);
        }
    }
}
