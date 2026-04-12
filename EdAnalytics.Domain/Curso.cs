namespace EdAnalytics.Domain
{
    public class Curso
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public int Visualizacoes { get; set; }

        public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();
    }
}