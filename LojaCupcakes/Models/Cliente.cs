using System.ComponentModel.DataAnnotations;

namespace LojaCupcakes.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "E-mail inválido")] // Validação de formato (HU04)
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255)]
        [DataType(DataType.Password)] // Indica que é um campo de senha
        public string Senha { get; set; }

        // Propriedade de Navegação: Um cliente pode ter vários pedidos
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}