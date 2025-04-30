using HeadphoneStore.Shared.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Shared.Abstracts.Queries;

public interface IQueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IQuery<TResponse>
{
}
