namespace HeadphoneStore.Contract.Dtos.Media;

public class FileRequiredParamsDto
{
    public Guid? productId { get; set; }

    public Guid? userId { get; set; }

    public string type { get; set; } = default!;
}
