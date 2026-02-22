using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage ="Name cannot be more than 50 characters long")]
       public string Name { get; set; }
       [Required]
        [MaxLength(1000, ErrorMessage = "Description cannot be more than 100 characters long")]
        public string Description { get; set; }
        [Required]
        [Range(0.1, 50)]
        public double LengthInKm { get; set; }
        [Required]
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set;}
            
    }
    
}