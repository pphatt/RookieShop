using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.ActiveBrand;

public sealed record ActivateBrandCommand(Guid Id) : ICommand;
