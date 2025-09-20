using Microsoft.EntityFrameworkCore;
using Model.DomainModels.ProductAggregates;
using Model.Services.Contracts;
using ResponseFramework;
using System.Net;

namespace Model.Services.Repositories
{
    /// <summary>
    /// Repository implementation for <see cref="Product"/> entity.
    /// Provides CRUD operations (Insert, Update, Delete, Select) using Entity Framework Core.
    /// This repository communicates with the database via <see cref="SinglePageApplicationContext"/>.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        #region [- Fields -]

        /// <summary>
        /// The database context for the application.
        /// </summary>
        private readonly SinglePageApplicationContext _context;

        #endregion

        #region [- Constructor -]

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The EF Core database context.</param>
        public ProductRepository(SinglePageApplicationContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        #region [- Insert -]
        /// <summary>
        /// Inserts a new product into the database.
        /// </summary>
        /// <param name="product">The product entity to insert.</param>
        /// <returns>A model of the Response framework that returns a type of bool .
        /// This model indicating success or failure of process .</returns>
        public async Task<IResponse<bool>> Insert(Product product)
        {
            if (product is null)
                return new Response<bool>("product is null .", HttpStatusCode.BadRequest);

            try
            {
                await _context.AddAsync(product);
                var response = await SaveChanges();
                if (!response.IsSuccessful)
                    return new Response<bool>(response.ErrorMessage, HttpStatusCode.InternalServerError);

                return new Response<bool>(true);
            }
            catch (Exception exp)
            {
                return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region [- Update -]
        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product entity with updated values.</param>
        /// <returns>A model of the Response framework that returns a type of bool .
        /// This model indicating success or failure of process .</returns>
        public async Task<IResponse<bool>> Update(Product product)
        {
            if (product is null)
                return new Response<bool>("product is null .", HttpStatusCode.BadRequest);

            var existing = (await SelectById(product.Id)).Result;
            if (existing is null)
                return new Response<bool>("Product not found.", HttpStatusCode.NotFound);

            try
            {
                _context.Update(product);
                var response = await SaveChanges();
                if (!response.IsSuccessful)
                    return new Response<bool>(response.ErrorMessage, HttpStatusCode.InternalServerError);

                return new Response<bool>(true);
            }
            catch (Exception exp)
            {
                return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region [- Delete -]
        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="product">The product entity to delete.</param>
        /// <returns>A model of the Response framework that returns a type of bool .
        /// This model indicating success or failure of process .</returns>
        public async Task<IResponse<bool>> Delete(Product product)
        {
            if (product is null)
                return new Response<bool>("product is null .", HttpStatusCode.BadRequest);

            try
            {
                _context.Remove(product);
                var response = await SaveChanges();
                if (!response.IsSuccessful)
                    return new Response<bool>(response.ErrorMessage, HttpStatusCode.InternalServerError);

                return new Response<bool>(true);
            }
            catch (Exception exp)
            {
                return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region [- Select All -]
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>Response containing a list of products, or an error message.</returns>
        public async Task<IResponse<IEnumerable<Product>>> SelectAll()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products is null)
                    return new Response<IEnumerable<Product>>("Products not found .Context is incorrect .", HttpStatusCode.NotFound);

                return new Response<IEnumerable<Product>>(products, true, "The process was completed successfully.", "", HttpStatusCode.OK);
            }
            catch (Exception exp)
            {
                return new Response<IEnumerable<Product>>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region [- Select By Id -]
        /// <summary>
        /// Retrieves a single product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>Response containing the product, or an error message.</returns>
        public async Task<IResponse<Product>> SelectById(Guid id)
        {
            if (id == Guid.Empty)
                return new Response<Product>("Id is empty .", HttpStatusCode.BadRequest);

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product is null)
                    return new Response<Product>("The product not found .", HttpStatusCode.NotFound);

                return new Response<Product>(product, true, "The process was completed successfully.", "", HttpStatusCode.OK);
            }
            catch (Exception exp)
            {
                return new Response<Product>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #endregion

        #region Private Methods

        #region [- SaveChanges -]

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        private async Task<IResponse<bool>> SaveChanges()
        {
            try
            {
                return new Response<bool>(await _context.SaveChangesAsync() > 0);
            }
            catch (Exception exp)
            {
                return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #endregion
    }
}
