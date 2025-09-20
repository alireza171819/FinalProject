namespace Model.DomainModels.ProductAggregates
{
    public class Product
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
