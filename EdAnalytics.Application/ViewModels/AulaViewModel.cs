using System.ComponentModel.DataAnnotations;

namespace EdAnalytics.Application.ViewModels
{
    public class AulaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O conteúdo é obrigatório.")]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        public int CursoId { get; set; }
    }
}
