using System.ComponentModel.DataAnnotations;

namespace EdAnalytics.Domain
{
    public class Aula
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }
    }
}
