using System.ComponentModel.DataAnnotations;
namespace ApiFuncional.Models
{
    public class Produto
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Nome pode ter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O Preço deve estar entre 0.01 e 10000.00.")]
        public double Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int QuantideEstoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200, ErrorMessage = "A Descrição pode ter no máximo 200 caracteres.")]
        public string? Descricao { get; set; }

    }
}
