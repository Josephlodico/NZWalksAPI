namespace NZWalks.API.Models.DTO
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public WalkDto Walk {get; set;}
    }
}
