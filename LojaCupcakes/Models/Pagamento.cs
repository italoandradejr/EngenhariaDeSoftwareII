using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaCupcakes.Models
{
    public class Pagamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
               
        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Required]
        [StringLength(50)]
        public string FormaPagamento { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}