using ApplicationService.Dtos.ProductDtos;
using ApplicationService.ProductServices.Contracts;
using Model.DomainModels.ProductAggregates;
using Model.Services.Contracts;
using ResponseFramework;
using System.Net;

namespace ApplicationService.ProductServices;
/// <summary>
/// Application service for managing <see cref="Product"/> entities.
/// Acts as a bridge between the repository layer (<see cref="IProductRepository"/>)
/// and higher-level layers such as controllers or APIs.
/// Provides business logic and DTO mapping for CRUD operations.
/// </summary>
public class ProductApplicationService : IProductApplicationService
{
    #region Privet Fields

    /// <summary>
    /// Repository for performing database operations on products.
    /// </summary>
    private readonly IProductRepository _productRepository;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductApplicationService"/> class.
    /// </summary>
    /// <param name="productRepository">Repository for product persistence operations.</param>
    public ProductApplicationService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    #endregion

    #region Public Methods

    #region [- Post -]
    /// <summary>
    /// Creates a new product and persists it using the repository.
    /// </summary>
    /// <param name="postProductDto">DTO containing product data to be inserted.</param>
    /// <returns>
    /// Response indicating success or failure of the insert operation.
    /// </returns>
    public async Task<IResponse<bool>> Post(PostProductDto postProductDto)
    {
        if (postProductDto is null)
            return new Response<bool>("Model is null .", HttpStatusCode.BadRequest);

        var product = new Product();
        product.ProductName = postProductDto.ProductName;
        product.ProductDescription = postProductDto.ProductDescription;
        product.UnitPrice = postProductDto.UnitPrice;

        var response = await _productRepository.Insert(product);

        if (!response.IsSuccessful)
            return new Response<bool>(response.ErrorMessage, HttpStatusCode.InternalServerError);

        return new Response<bool>(true);
    }
    #endregion

    #region [- Put -]
    /// <summary>
    /// Updates an existing product with new values.
    /// </summary>
    /// <param name="putProductDto">DTO containing updated product data.</param>
    /// <returns>
    /// Response indicating success or failure of the update operation.
    /// </returns>
    public async Task<IResponse<bool>> Put(PutProductDto putProductDto)
    {
        if (putProductDto is null)
            return new Response<bool>("Model is null .", HttpStatusCode.BadRequest);

        Product product = new();
        product.Id = putProductDto.Id;
        product.ProductName = putProductDto.ProductName;
        product.ProductDescription = putProductDto.ProductDescription;
        product.UnitPrice = putProductDto.UnitPrice;

        var response = await _productRepository.Update(product);

        if (!response.IsSuccessful)
            return new Response<bool>(response.ErrorMessage, HttpStatusCode.InternalServerError);

        return new Response<bool>(true);
    }
    #endregion

    #region [- Delete -]
    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="deleteProductDto">DTO containing the Id of the product to delete.</param>
    /// <returns>
    /// Response indicating success or failure of the delete operation.
    /// </returns>
    public async Task<IResponse<bool>> Delete(DeleteProductDto deleteProductDto)
    {
        if (deleteProductDto is null)
            return new Response<bool>("deleteProductDto is null .", HttpStatusCode.BadRequest);

        var response = await _productRepository.SelectById(deleteProductDto.Id);

        if (!response.IsSuccessful)
            return new Response<bool>(response.ErrorMessage, HttpStatusCode.NotFound);

        var responseDelete = await _productRepository.Delete(response.Result);

        if (!responseDelete.IsSuccessful)
            return new Response<bool>(responseDelete.ErrorMessage, HttpStatusCode.InternalServerError);

        return new Response<bool>(true);
    }
    #endregion

    #region [- Get All -]
    /// <summary>
    /// Retrieves all products from the repository and maps them into DTOs.
    /// </summary>
    /// <returns>
    /// Response containing <see cref="GetAllProductDto"/> with a list of products,
    /// or an error response if none are found.
    /// </returns>
    public async Task<IResponse<GetAllProductDto>> GetAll()
    {
        var response = await _productRepository.SelectAll();
        var products = response.Result;
        if (products is null || !products.Any())
            return new Response<GetAllProductDto>(response.ErrorMessage, HttpStatusCode.NotFound);

        GetAllProductDto getAllProductDto = new();
        List<GetByIdProductDto> productDtosList = new();
        foreach (var product in products)
        {
            var getByIdProductDto = new GetByIdProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                UnitPrice = product.UnitPrice
            };
            productDtosList.Add(getByIdProductDto);
        }
        getAllProductDto.GetByIdProductDtos = productDtosList;
        return new Response<GetAllProductDto>(getAllProductDto, true, "The process was completed successfully.", "", HttpStatusCode.OK);
    }

    #endregion

    #region [- Get By Id -]
    /// <summary>
    /// Retrieves a single product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>
    /// Response containing <see cref="GetByIdProductDto"/> if found,
    /// or an error response if not found or invalid.
    /// </returns>
    public async Task<IResponse<GetByIdProductDto>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            return new Response<GetByIdProductDto>("Id is empty .", HttpStatusCode.BadRequest);

        var response = await _productRepository.SelectById(id);

        if (response.Result is null)
            return new Response<GetByIdProductDto>(response.ErrorMessage, HttpStatusCode.NotFound);

        var product = response.Result;
        GetByIdProductDto productDto = new();
        productDto.Id = product.Id;
        productDto.ProductName = product.ProductName;
        productDto.ProductDescription = product.ProductDescription;
        productDto.UnitPrice = product.UnitPrice;

        return new Response<GetByIdProductDto>(productDto, true, "The process was completed successfully.", "", HttpStatusCode.OK);
    }

    #endregion

    #endregion
}
