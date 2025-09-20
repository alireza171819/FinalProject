using Model.DomainModels.ProductAggregates;
using ResponseFramework;

namespace Model.Services.Contracts
{
    /// <summary>
    /// Repository contract for managing <see cref="Product"/> entities.
    /// Defines persistence-level operations (CRUD) for product data.
    /// This interface abstracts database access and allows swapping data sources
    /// without changing the application or domain layers.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Inserts a new product into the data store.
        /// </summary>
        /// <param name="product">The product entity to insert.</param>
        /// <returns>
        /// A response indicating success or failure of the insert operation.
        /// </returns>
        Task<IResponse<bool>> Insert(Product product);

        /// <summary>
        /// Updates an existing product in the data store.
        /// </summary>
        /// <param name="product">The product entity with updated values.</param>
        /// <returns>
        /// A response indicating success or failure of the update operation.
        Task<IResponse<bool>> Update(Product product);

        /// <summary>
        /// Deletes a product from the data store.
        /// </summary>
        /// <param name="product">The product entity to delete.</param>
        /// <returns>
        /// A response indicating success or failure of the delete operation.
        /// </returns>
        Task<IResponse<bool>> Delete(Product product);

        /// <summary>
        /// Selects all products from the data base.
        /// </summary>
        /// <returns>
        /// A response containing an <see cref="IEnumerable{Product}"/> collection
        /// or an error response if no products are found.
        /// </returns>
        Task<IResponse<IEnumerable<Product>>> SelectAll();

        /// <summary>
        /// Select a single product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// A response containing the <see cref="Product"/> if found,
        /// or an error response if the product does not exist.
        /// </returns>
        Task<IResponse<Product>> SelectById(Guid id);
    }
}
