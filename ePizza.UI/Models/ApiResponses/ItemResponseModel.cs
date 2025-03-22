namespace ePizza.UI.Models.ApiResponses
{
    public class ItemResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float UnitPrice { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int ItemTypeId { get; set; }

    }
}
