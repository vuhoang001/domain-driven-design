namespace ddd.API.Modules.Item;

public class CreateItemDto
{
    public string ItemName { get; set; }
    public string? ItemDesc { get; set; }
    public decimal Price { get; set; }
}