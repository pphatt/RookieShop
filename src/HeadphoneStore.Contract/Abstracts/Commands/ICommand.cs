using HeadphoneStore.Contract.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Contract.Abstracts.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
