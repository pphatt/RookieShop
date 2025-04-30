using HeadphoneStore.Shared.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Shared.Abstracts.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
