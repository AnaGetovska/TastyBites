namespace TastyBytesReact.Models.Requests
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }
        public int PreparationTime { get; set; }
        public int Portions { get; set;}
        public string Description { get; set; }
        public IFormFile DisplayImage { get; set; }
        //public string[] Ingredients { get; set; }
        //public string[] Categories { get; set; }
        //public string[] Allergens { get; set; }
        //public IEnumerable<IFormFile> Images { get; set;}
    }
}
