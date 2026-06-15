namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageURl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
