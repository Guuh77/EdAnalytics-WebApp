using System.ComponentModel.DataAnnotations;

namespace EdAnalytics.Application.ViewModels
{
    public class CursoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
        [Display(Name = "Título do Curso")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A área é obrigatória")]
        [StringLength(50, ErrorMessage = "A área não pode exceder 50 caracteres")]
        [Display(Name = "Área")]
        public string Area { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número de visualizações é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "O número de visualizações não pode ser negativo")]
        [Display(Name = "Visualizações")]
        public int Visualizacoes { get; set; }
    }
}