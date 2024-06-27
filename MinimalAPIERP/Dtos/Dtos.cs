namespace MinimalAPIERP.Dtos
{
    public class RaincheckDto
    {
        public string? Name { get; set; }
        public int Count { get; set; }
        public double SalePrice { get; set; }
        public StoreDto? Store { get; set; }
        public ProductDto? Product { get; set; }
    }

    public class StoreDto
    {
        public string? Name { get; set; }
    }

    public class ProductDto
    {
        public string? Name { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class CategoryDto
    {
        public string? Name { get; set; }
    }
}
