using HeadphoneStore.Shared.Abstracts.Commands;

namespace HeadphoneStore.Application.UseCases.V1.Brand.BulkDeleteBrand;

public sealed record BulkDeleteBrandCommand(List<Guid> Ids) : ICommand;
