using System.Reflection.PortableExecutable;

namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageURl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }


        //Navigation properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }
}
