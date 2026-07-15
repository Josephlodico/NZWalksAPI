namespace NZWalks.API.Models.Domain
{
    public class Review      
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public Guid WalkId { get; set; }

        // Navigation property
        public Walk Walk { get; set; }
    }
}
