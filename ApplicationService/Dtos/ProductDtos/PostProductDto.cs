using System.Text.Json.Serialization;

namespace ApplicationService.Dtos.ProductDtos;

public class PostProductDto
{
    [JsonPropertyName("productName")]
    public string ProductName { get; set; }
    [JsonPropertyName("productDescription")]
    public string ProductDescription { get; set; }
    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }
}
