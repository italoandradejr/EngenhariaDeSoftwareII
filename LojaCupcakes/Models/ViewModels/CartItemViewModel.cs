namespace LojaCupcakes.Models.ViewModels
{
    // Esta classe simples serve apenas para transferir
    // os dados do carrinho (JSON) do JavaScript para o Controller.
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
    }
}