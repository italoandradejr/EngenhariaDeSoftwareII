using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaCupcakes.Models
{
    public class ItemPedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        // Propriedade de Navegação: Link para o Pedido
        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Required]
        public int CupcakeId { get; set; }

        // Propriedade de Navegação: Link para o Cupcake
        [ForeignKey("CupcakeId")]
        public Cupcake? Cupcake { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Preço Unitário")]
        public decimal PrecoUnitario { get; set; } // Preço do cupcake no momento da compra
    }
}