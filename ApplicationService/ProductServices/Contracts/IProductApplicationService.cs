using ApplicationService.Dtos.ProductDtos;
using ResponseFramework;
namespace ApplicationService.ProductServices.Contracts; 

/// <summary>
/// Application service contract for managing <see cref="Product"/> entities.
/// Defines business-level operations for product management,
/// separating higher-level logic (controllers, APIs) from the repository layer.
/// </summary>
public interface IProductApplicationService
{
    /// <summary>
    /// Creates a new product in the system.
    /// </summary>
    /// <param name="postProductDto">DTO containing product details for creation.</param>
    /// <returns>
    /// A response indicating success or failure of the insert operation.
    /// </returns>
    Task<IResponse<bool>> Post(PostProductDto postProductDto);

    /// <summary>
    /// Updates an existing product with new values.
    /// </summary>
    /// <param name="putProductDto">DTO containing updated product details.</param>
    /// <returns>
    /// A response indicating success or failure of the update operation.
    /// </returns>
    Task<IResponse<bool>> Put(PutProductDto putProductDto);

    /// <summary>
    /// Deletes an existing product from the system.
    /// </summary>
    /// <param name="deleteProductDto">DTO containing the Id of the product to delete.</param>
    /// <returns>
    /// A response indicating success or failure of the delete operation.
    /// </returns>
    Task<IResponse<bool>> Delete(DeleteProductDto deleteProductDto);

    /// <summary>
    /// Retrieves all products and maps them into a collection DTO.
    /// </summary>
    /// <returns>
    /// A response containing <see cref="GetAllProductDto"/> with product details,
    /// or an error response if no products exist.
    /// </returns>
    Task<IResponse<GetAllProductDto>> GetAll();

    /// <summary>
    /// Retrieves a single product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// A response containing <see cref="GetByIdProductDto"/> if found,
    /// or an error response if the product is not found or the id is invalid.
    /// </returns>
    Task<IResponse<GetByIdProductDto>> GetById(Guid id);
}
