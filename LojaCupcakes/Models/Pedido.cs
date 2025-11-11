using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaCupcakes.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }
                
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Valor Total")]
        public decimal ValorTotal { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Processando";
                
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}