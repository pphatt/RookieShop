using HeadphoneStore.Contract.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Contract.Abstracts.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
