using HeadphoneStore.Contract.Abstracts.Shared;

using MediatR;

namespace HeadphoneStore.Contract.Abstracts.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
