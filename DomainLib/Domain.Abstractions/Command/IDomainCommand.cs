using MediatR;

namespace Domain.Command
{
    public interface IDomainCommand : IRequest { }

    public interface IDomainCommand<TResult> : IRequest<TResult> { }
}
