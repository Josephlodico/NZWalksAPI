using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{

    public class AddReviewRequestDto        
    {
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }

        [Required]
        public Guid WalkId { get; set; }    

    }
}
