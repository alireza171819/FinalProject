namespace ApplicationService.Dtos.ProductDtos;

public class GetByIdProductDto
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
}
