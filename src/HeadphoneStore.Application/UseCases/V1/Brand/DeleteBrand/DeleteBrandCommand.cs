using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.DeleteBrand;

public sealed record DeleteBrandCommand(Guid Id) : ICommand;
