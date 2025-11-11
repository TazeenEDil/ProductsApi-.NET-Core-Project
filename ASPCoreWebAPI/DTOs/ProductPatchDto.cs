namespace ProductsApi.DTOs
{
    public class ProductPatchDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
    }
}
