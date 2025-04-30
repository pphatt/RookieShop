using HeadphoneStore.Shared.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Shared.Abstracts.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
