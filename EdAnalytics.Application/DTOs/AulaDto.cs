namespace EdAnalytics.Application.DTOs
{
    public class AulaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public int CursoId { get; set; }
    }
}
