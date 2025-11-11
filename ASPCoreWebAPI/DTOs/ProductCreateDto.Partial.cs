namespace ProductsApi.DTOs
{
    public partial class ProductCreateDto : ProductBase
    {
        
        public new string Name { get => base.Name!; set => base.Name = value; }
        public new string Description { get => base.Description!; set => base.Description = value; }
        public new double Price { get => base.Price ?? 0; set => base.Price = value; }
    }
}
