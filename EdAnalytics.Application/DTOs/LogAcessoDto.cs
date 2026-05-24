namespace EdAnalytics.Application.DTOs
{
    public class LogAcessoDto
    {
        public string? Id { get; set; }
        public int CursoId { get; set; }
        public string CursoTitulo { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string? UsuarioId { get; set; }
        public Dictionary<string, string>? Detalhes { get; set; }
    }
}
