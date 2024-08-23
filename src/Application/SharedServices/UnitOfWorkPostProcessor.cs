using Domain.Command;
using Domain.Extensions;
using MediatR.Pipeline;

namespace Application.SharedServices
{
    public sealed class UnitOfWorkPostProcessor<TCommand, TResponse>(IUnitOfWork unitOfWork)
        : IRequestPostProcessor<TCommand, TResponse>
        where TCommand : IBaseDomainCommand
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task Process(
            TCommand request,
            TResponse response,
            CancellationToken cancellationToken
        )
        {
            return _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
