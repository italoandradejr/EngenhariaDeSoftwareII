/* ==============================================
   FUNÇÕES GLOBAIS
   ============================================== */

// Função chamada pelo botão "Adicionar ao Carrinho"
function adicionarAoCarrinho(id, nome, preco) {
    let carrinho = getCarrinho();
    let itemExistente = carrinho.find(item => item.id === id);

    if (itemExistente) {
        itemExistente.quantidade++;
    } else {
        carrinho.push({
            id: id,
            nome: nome,
            preco: preco,
            quantidade: 1
        });
    }
    saveCarrinho(carrinho);
    atualizarIconeCarrinho();
    alert(`${nome} foi adicionado ao carrinho!`);
}

// Função para buscar o carrinho do localStorage
function getCarrinho() {
    return JSON.parse(localStorage.getItem('carrinho')) || [];
}

// Função para salvar o carrinho no localStorage
function saveCarrinho(carrinho) {
    localStorage.setItem('carrinho', JSON.stringify(carrinho));
}

// Função para atualizar a contagem no ícone do carrinho
function atualizarIconeCarrinho() {
    let carrinho = getCarrinho();
    let totalItens = carrinho.reduce((total, item) => total + item.quantidade, 0);

    let cartIcon = document.getElementById('cart-count');
    if (cartIcon) {
        cartIcon.innerText = totalItens;
        cartIcon.style.display = totalItens > 0 ? 'inline-block' : 'none'; // 'inline-block' é melhor que 'block' para um span
    }
}

/* ==============================================
   FUNÇÕES DA PÁGINA DO CARRINHO
   ============================================== */

// Escuta o evento de "página carregada"
document.addEventListener('DOMContentLoaded', () => {
    // Se estivermos na página do carrinho (o tbody existe), carregue-a
    if (document.getElementById('cart-table-body')) {
        loadCartPage();
    }

    // Se o botão de finalizar existir, adicione o evento de clique
    const btnFinalizar = document.getElementById('btn-finalizar');
    if (btnFinalizar) {
        btnFinalizar.addEventListener('click', finalizarPedido);
    }

    // Atualiza o ícone do carrinho em todas as páginas
    atualizarIconeCarrinho();
});

// Carrega os itens do localStorage na tabela do carrinho
function loadCartPage() {
    const carrinho = getCarrinho();
    const tableBody = document.getElementById('cart-table-body');
    tableBody.innerHTML = ''; // Limpa a tabela

    if (carrinho.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="5">Seu carrinho está vazio.</td></tr>';
        updateCartTotal(); // Garante que o total seja R$ 0,00
        return;
    }

    carrinho.forEach(item => {
        const subtotal = item.preco * item.quantidade;
        const row = `
            <tr>
                <td>${item.nome}</td>
                <td>${item.preco.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                <td>${item.quantidade}</td>
                <td>${subtotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="removerDoCarrinho(${item.id})">
                        Remover (HU07)
                    </button>
                </td>
            </tr>
        `;
        tableBody.innerHTML += row;
    });

    updateCartTotal();
}

// Atualiza o valor total na página do carrinho
function updateCartTotal() {
    const carrinho = getCarrinho();
    const total = carrinho.reduce((acc, item) => acc + (item.preco * item.quantidade), 0);

    const totalElement = document.getElementById('cart-total');
    if (totalElement) {
        totalElement.innerText = total.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    }
}

// Remove um item do carrinho (HU07)
function removerDoCarrinho(id) {
    if (confirm('Tem certeza que deseja remover este item?')) {
        let carrinho = getCarrinho();
        carrinho = carrinho.filter(item => item.id !== id);
        saveCarrinho(carrinho);

        // Recarrega a tabela e o ícone
        loadCartPage();
        atualizarIconeCarrinho();
    }
}

// Envia o pedido para o back-end (HU03)
async function finalizarPedido() {
    const carrinho = getCarrinho();

    if (carrinho.length === 0) {
        alert('Seu carrinho está vazio!');
        return;
    }

    const dadosParaEnviar = carrinho.map(item => ({
        id: item.id,
        quantidade: item.quantidade
    }));

    try {
        const response = await fetch('/Pedido/Finalizar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                // Adicionar o token anti-falsificação se necessário
            },
            body: JSON.stringify(dadosParaEnviar)
        });

        if (response.ok) {
            const data = await response.json();
            saveCarrinho([]); // Limpa o carrinho
            atualizarIconeCarrinho();
            window.location.href = data.redirectUrl; // Redireciona
        } else {
            alert('Erro ao finalizar o pedido. Tente novamente.');
        }
    } catch (error) {
        console.error('Erro:', error);
        alert('Ocorreu um erro de rede.');
    }
}