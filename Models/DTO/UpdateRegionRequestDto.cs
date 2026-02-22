
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
{
    [Required]
    [MaxLength(50, ErrorMessage ="Name cannot be more than 50 characters long")]
    public string Name {get; set;}

    [Required]
    [MinLength(3,ErrorMessage ="Code must be at least 3 characters long") ]
    [MaxLength(3, ErrorMessage ="Code cannot be more than 3 characters long")]
    public string Code {get; set;}

    public string? RegionImageUrl {get; set;}
    
}
    
}