using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has be a minimum of 3 caracters")]
        [MaxLength(3, ErrorMessage = "Code has be a maximum of 3 caracters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Code has be a maximum of 100 caracters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
