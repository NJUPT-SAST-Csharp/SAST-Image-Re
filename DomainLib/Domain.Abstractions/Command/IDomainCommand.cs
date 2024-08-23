using MediatR;

namespace Domain.Command
{
    public interface IDomainCommand : IRequest, IBaseDomainCommand { }

    public interface IDomainCommand<TResult> : IRequest<TResult>, IBaseDomainCommand { }

    public interface IBaseDomainCommand { }
}
