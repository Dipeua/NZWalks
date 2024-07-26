namespace NZWalks.API.Models.DTOs
{
    public class LoginResponseDto
    {
        public int status { get; set; }
        public string JwtToken { get; set; }
    }
}
