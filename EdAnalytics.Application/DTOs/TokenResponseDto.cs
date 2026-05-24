namespace EdAnalytics.Application.DTOs
{
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Type { get; set; } = "Bearer";
    }
}
