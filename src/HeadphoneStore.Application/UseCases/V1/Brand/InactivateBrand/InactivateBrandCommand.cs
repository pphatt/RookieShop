using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.InactiveBrand;

public sealed record InactivateBrandCommand(Guid Id) : ICommand;
