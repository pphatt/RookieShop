using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Contract.Abstracts.Queries;

public interface IQueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IQuery<TResponse>
{
}
