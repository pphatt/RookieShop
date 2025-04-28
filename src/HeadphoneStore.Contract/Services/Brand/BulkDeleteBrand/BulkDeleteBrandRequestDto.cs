namespace HeadphoneStore.Contract.Services.Brand.BulkDelete;

public class BulkDeleteBrandRequestDto
{
    public required List<Guid> Ids { get; set; }
}
